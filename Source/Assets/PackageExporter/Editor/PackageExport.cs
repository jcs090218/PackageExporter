#if UNITY_EDITOR
/**
 * Copyright (c) Pixisoft. All rights reserved.
 * 
 * pixisoft.tw@gmail.com
 */
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace PackageExporter
{
    /// <summary>
    /// Small window that pop up and show all files that has been selected 
    /// (not ignored).
    /// </summary>
    public class PackageExport : EditorWindow
    {
        /* Variables */

        private ExportTreeView mTreeView;
        private TreeViewState mTreeViewState;

        public List<string> exportList = new List<string>();
        public string packageName = "";

        public bool exportNext = false;

        internal static class Styles
        {
            public static GUIStyle title = "LargeBoldLabel";
            public static GUIStyle bottomBarBg = "ProjectBrowserBottomBarBg";
            public static GUIStyle topBarBg = "OT TopBar";
            public static GUIStyle loadingTextStyle = "CenteredLabel";
            public static GUIContent allText = EditorGUIUtility.TrTextContent("All");
            public static GUIContent noneText = EditorGUIUtility.TrTextContent("None");
            public static GUIContent includeDependenciesText = EditorGUIUtility.TrTextContent("Include dependencies");
        }

        /* Setter & Getter */

        /* Functions */

        public PackageExport()
        {
            // Initial pos and minsize
            position = new Rect(100, 100, 400, 300);
            minSize = new Vector2(350, 350);
            titleContent = new GUIContent("Export package");
        }

        public static void ShowExportWindow(string name, List<string> exportList, bool exportNext)
        {
            var window = CreateInstance<PackageExport>();
            window.ShowUtility();

            window.packageName = name;
            window.exportList = exportList;
            window.exportNext = exportNext;
        }

        private void OnGUI()
        {
            TopArea();

            TreeViewArea();

            BottomArea();
        }

        private void OnDestroy()
        {
            if (exportNext)
                PackageExporter.ExportNext();
        }

        private void TopArea()
        {
            float totalTopHeight = 53f;
            Rect r = GUILayoutUtility.GetRect(position.width, totalTopHeight);

            // Background
            GUI.Label(r, GUIContent.none, Styles.topBarBg);

            // Header
            Rect titleRect = new Rect(r.x + 5f, r.yMin, r.width, r.height);
            GUI.Label(titleRect, EditorGUIUtility.TrTextContent("Items to Export (" + packageName + ")"), Styles.title);
        }

        private void BottomArea()
        {
            // Background
            GUILayout.BeginVertical(Styles.bottomBarBg);
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(EditorGUIUtility.TrTextContent("Export...")))
            {
                Export();

                Close();
                GUIUtility.ExitGUI();
            }
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        private void TreeViewArea()
        {
            Rect treeAreaRect = GUILayoutUtility.GetRect(1, 9999, 1, 99999);

            if (exportList.Count == 0)
            {
                GUI.Label(treeAreaRect, "Nothing to export!", Styles.loadingTextStyle);
                return;
            }

            if (mTreeViewState == null)
                mTreeViewState = new TreeViewState();

            if (mTreeView == null)
                mTreeView = new ExportTreeView(this, mTreeViewState);

            mTreeView.OnGUI(treeAreaRect);
        }

        private void Export()
        {
            string ext = "unitypackage";
            string savePath = EditorUtility.SaveFilePanel("Export package ...", "", packageName, ext);

            if (savePath == "")
                return;

            AssetDatabase.ExportPackage(
                exportList.ToArray(),
                savePath,
                ExportPackageOptions.Default);

            // show it in file explorer. (GUI)
            EditorUtility.RevealInFinder(savePath);
        }
    }
}
#endif
