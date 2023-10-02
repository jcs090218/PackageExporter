#if UNITY_EDITOR
/**
 * Copyright (c) 2017-2022 Shen, Jen-Chieh
 * 
 * jcs090218@gmail.com
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

namespace PackageExporter
{
    /// <summary>
    /// Main control panel for this plugin.
    /// </summary>
    public class PackageExporter : EditorWindow
    {
        /* Variables */

        public static PackageExporter instance = null;

        public const string NAME = "Package Exporter";

        private const string PACKAGE_FOLDER = "Assets";
        private const string DEFAULT_PACKAGE_NAME = "Empty Package Name";
        private const string DELIMITER = "_";
        private const string VERSION_SYMBOL = "v";

        private const string IGNORE_FILE_PATH = "PackageExporter/unityignore";
        private const string TEMPLATE_PATH = "PackageExporter/template";

        private const string IGNORE_FILE_EXT = ".unityignore";

        private const string IGNORE_FILE_TEMPLATE_FILE = "template.unityignore";

        private const int EXPORT_ALL_PACKAGES_BUTTON_COUNT = 2;

        private const string REPLACE_PACKAGE_NAME = "[PACKAGE_NAME]";
        private const string REPLACE_CREATION_DATE = "[CREATION_DATE]";
        private const string REPLACE_VERSION_NO = "[VERSION_NO]";

        private static int EXPORT_INDEX = 0;

        /// <summary>
        /// Structure of the export packages.
        /// </summary>
        [Serializable]
        public struct ExportPackageInfo
        {
            public string packageName;
            public string versionNo;
        };

        public ExportPackageInfo[] exportPackagesList = { };

        private SerializedObject mSerializedObject = null;
        private SerializedProperty mSerializedProperty = null;

        /* Setter & Getter */

        /* Functions */

        private void OnEnable()
        {
            instance = this;
            mSerializedObject = new SerializedObject(this);
            mSerializedProperty = mSerializedObject.FindProperty("exportPackagesList");
        }

        private void OnGUI()
        {
            OnEP_Editor();
        }

        private static string FormPackageName(string name, string version)
        {
            string finalPackageName = name + DELIMITER + VERSION_SYMBOL + version;

            if (version == "")
                finalPackageName = name;

            if (name == "")
                finalPackageName = DEFAULT_PACKAGE_NAME;

            return finalPackageName;
        }

        /// <summary>
        /// Intialize tile editor.
        /// </summary>
        private void OnEP_Editor()
        {
            GUILayout.Label("Packages Settings", EditorStyles.boldLabel);

            /* Export the whole list. */
            EditorUtil.CreateGroup(() =>
            {
                mSerializedObject.Update();

                EditorGUILayout.PropertyField(mSerializedProperty, true);

                mSerializedObject.ApplyModifiedProperties();
            });

            GUILayout.Label("Unity Ignore File", EditorStyles.boldLabel);

            EditorUtil.CreateGroup(() =>
            {
                if (GUILayout.Button("Generate Unity Ignore"))
                    GenerateUnityIgnoreFiles();
            });

            GUILayout.Label("Export Packages", EditorStyles.boldLabel);

            int buttonShown = 0;

            EditorUtil.CreateGroup(() =>
            {
                for (int index = 0; index < instance.exportPackagesList.Length; ++index)
                {
                    ExportPackageInfo eps = instance.exportPackagesList[index];

                    /* GUI Layout */
                    string name = eps.packageName;
                    string version = eps.versionNo;

                    string finalPackageName = FormPackageName(name, version);

                    ++buttonShown;

                    /* Assign export button. */
                    if (GUILayout.Button("Export -> " + finalPackageName))
                        Export(name, version);
                }

                if (buttonShown >= EXPORT_ALL_PACKAGES_BUTTON_COUNT)
                {
                    if (GUILayout.Button("Export All Packages"))
                        ExportAllPackages();
                }
            });
        }

        /// <summary>
        /// Export the package.
        /// </summary>
        /// <param name="name"> Name of the package. </param>
        /// <param name="version"> Version of the package. </param>
        /// <param name="savePath"> Output directory. (optional) </param>
        /// <param name="next"> True if iterate through the whole package list. </param>
        public static void Export(string name, string version, string savePath = null, bool next = false)
        {
            string finalPackageName = FormPackageName(name, version);

            string ignoreFilePath = Application.dataPath + "/" + IGNORE_FILE_PATH + "/";
            string ignoreFileName = name + IGNORE_FILE_EXT;
            string newIgnoreFullPath = ignoreFilePath + ignoreFileName;

            newIgnoreFullPath = newIgnoreFullPath.Replace("\\", "/");

            if (!File.Exists(newIgnoreFullPath))
                return;

            string[] wildcards = ReadAllLinesWithoutComment(newIgnoreFullPath);

            string[] exportList = GetAllFilesAndDirInPath();
            List<string> finalExportList = new List<string>();

            foreach (string path in exportList)
            {
                string fixedPath = path.Replace("\\", "/");

                if (IgnoreExportPath(fixedPath))
                    continue;

                fixedPath = MakeValidExportPath(fixedPath);

                // check if this path is ignore by the .unityignore file.
                if (MakeIgnore(fixedPath, wildcards))
                    continue;

                finalExportList.Add(fixedPath);
            }

            if (finalExportList.Count == 0)
            {
                Debug.Log("No file is exported, it seems like you have ignore all files");
                return;
            }

            if (savePath == null)
                PackageExport.ShowExportWindow(finalPackageName, finalExportList, next);
            else
            {
                string ext = ".unitypackage";
                savePath = Path.Combine(savePath, finalPackageName + ext);

                ExportPackage(finalExportList, savePath);
            }
        }

        private static void ExportAllPackages()
        {
            EXPORT_INDEX = -1;
            ExportNext();
        }

        public static void ExportNext()
        {
            ++EXPORT_INDEX;
            if (instance.exportPackagesList.Length <= EXPORT_INDEX)
                return;

            ExportPackageInfo eps = instance.exportPackagesList[EXPORT_INDEX];

            /* GUI Layout */
            string packageName = eps.packageName;
            string versionNo = eps.versionNo;

            Export(packageName, versionNo, null, true);
        }

        public static void ExportPackage(List<string> exportList, string savePath)
        {
            ExportPackage(exportList.ToArray(), savePath);
        }

        public static void ExportPackage(string[] exportList, string savePath)
        {
            AssetDatabase.ExportPackage(exportList, savePath, ExportPackageOptions.Default);

            // show it in file explorer. (GUI)
            EditorUtility.RevealInFinder(savePath);
        }

        private static void GenerateUnityIgnoreFiles()
        {
            string ignoreTemplatePath = Application.dataPath + "/" + TEMPLATE_PATH + "/";
            string ignoreFileTemplatePath = ignoreTemplatePath + IGNORE_FILE_TEMPLATE_FILE;
            string[] templateLines = File.ReadAllLines(ignoreFileTemplatePath);

            for (int index = 0; index < instance.exportPackagesList.Length; ++index)
            {
                ExportPackageInfo eps = instance.exportPackagesList[index];

                string packageName = eps.packageName;
                string versionNo = eps.versionNo;

                string ignoreFilePath = Application.dataPath + "/" + IGNORE_FILE_PATH + "/";
                string ignoreFileName = packageName + IGNORE_FILE_EXT;
                string newIgnoreFullPath = ignoreFilePath + ignoreFileName;

                ignoreFileTemplatePath = ignoreFileTemplatePath.Replace("\\", "/");
                newIgnoreFullPath = newIgnoreFullPath.Replace("\\", "/");


                if (!File.Exists(newIgnoreFullPath))
                {
                    FileStream fileStream = new FileStream(newIgnoreFullPath,
                                            FileMode.OpenOrCreate,
                                            FileAccess.ReadWrite,
                                            FileShare.None);

                    // make header, date, info, etc.
                    string[] decoratedTemplateLines = MakeDecoration(templateLines, packageName, versionNo);

                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        foreach (string line in decoratedTemplateLines)
                            sw.WriteLine(line);
                    }
                }
            }

            // reset asset database once.
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Replace the template's keyword to proper header.
        /// </summary>
        /// <param name="templateLines"></param>
        /// <param name="packageName"></param>
        /// <param name="versionNo"></param>
        /// <returns></returns>
        private static string[] MakeDecoration(string[] templateLines, string packageName, string versionNo)
        {
            List<string> decoratedTemplate = new List<string>();

            foreach (string line in templateLines)
            {
                string currentLine = line;

                if (currentLine.Contains(REPLACE_PACKAGE_NAME))
                    currentLine = currentLine.Replace(REPLACE_PACKAGE_NAME, packageName);

                if (currentLine.Contains(REPLACE_VERSION_NO))
                    currentLine = currentLine.Replace(REPLACE_VERSION_NO, versionNo);

                if (currentLine.Contains(REPLACE_CREATION_DATE))
                {
                    string dateAndTimeVar = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    currentLine = currentLine.Replace(REPLACE_CREATION_DATE, dateAndTimeVar);
                }

                decoratedTemplate.Add(currentLine);
            }

            return decoratedTemplate.ToArray();
        }

        /// <summary>
        /// Remove all the possible condition that we don't want to export.
        /// </summary>
        /// <returns>
        /// true: ignore it.
        /// false: don't ignore it, we want it to export.
        /// </returns>
        private static bool IgnoreExportPath(string path)
        {
            string ext = Path.GetExtension(path);

            if (ext == ".meta")
                return true;

            return false;
        }

        /// <summary>
        /// Everything we want to export must be under Assets.
        /// </summary>
        /// <returns></returns>
        private static string MakeValidExportPath(string path)
        {
            int index = path.IndexOf(PACKAGE_FOLDER);
            return path.Substring(index, path.Length - index);
        }


        /// <summary>
        /// Check if this path is ignore by the .unityignore file.
        /// </summary>
        /// <param name="path"> Path to check if we ignore this path? </param>
        /// <param name="ignoreList"> Ignore list to check ignore the file? </param>
        /// <returns>
        /// true: ignore it.
        /// false: don't ignore.
        /// </returns>
        private static bool MakeIgnore(string path, string[] wildcards)
        {
            // NOTE: Here is actually where we compare the path and ignore path.

            foreach (string wildcard in wildcards)
            {
                bool match = Regex.IsMatch(path, WildCardToRegular(wildcard));

                if (match)
                    return true;
            }

            // don't ignore
            return false;
        }

        /// <summary>
        /// Convert wildcard to RegEx!
        /// 
        /// If you want to implement both "*" and "?"
        /// 
        /// Copied from https://stackoverflow.com/questions/30299671/matching-strings-with-wildcard
        /// </summary>
        private static String WildCardToRegular(String value)
        {
            return Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*");
        }


        /// <summary>
        /// Show package exporter window.
        /// </summary>
        [MenuItem("Window/Package Exporter", false, 1500)]
        private static void ShowWindow()
        {
            var window = GetWindow<PackageExporter>(false, NAME, true);
            window.Show();
        }

        /// <summary>
        /// Get all the files and turn into string array.
        /// 
        /// SOURCE(jenchieh): https://stackoverflow.com/questions/12332451/list-all-files-and-directories-in-a-directory-subdirectories
        /// </summary>
        /// <returns></returns>
        private static string[] GetAllFilesAndDirInPath()
        {
            return Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Read all the lines from a text file without the comment character.
        /// </summary>
        /// <param name="path_to_file"></param>
        /// <returns></returns>
        private static string[] ReadAllLinesWithoutComment(string path_to_file)
        {
            string[] allLine = File.ReadAllLines(path_to_file);

            List<string> cleanLine = new List<string>();

            for (int count = 0; count < allLine.Length; ++count)
            {
                string line = allLine[count];

                // ignore comment.
                if (CheckIfComment(line))
                    continue;

                cleanLine.Add(line);
            }

            return cleanLine.ToArray();
        }

        /// <summary>
        /// Check the line is a comment.
        /// </summary>
        /// <param name="line"> line to check </param>
        /// <returns> 
        /// true : is a comment line / ignore it.
        /// false : is data value.
        /// </returns>
        public static bool CheckIfComment(string line)
        {
            if (line == "")
                return true;

            for (int index = 0; index < line.Length; ++index)
            {
                var ch = line[index];

                if (ch != ' ' && ch != '#')
                    return false;

                // check if first character the comment character.
                if (ch == '#')
                    return true;
            }

            return false;
        }
    }
}
#endif
