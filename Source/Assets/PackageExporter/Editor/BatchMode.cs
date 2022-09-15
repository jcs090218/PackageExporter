#if UNITY_EDITOR
/**
 * Copyright (c) 2017-2022 Shen, Jen-Chieh
 * 
 * jcs090218@gmail.com
 */
using System;
using System.IO;

namespace PackageExporter
{
    /// <summary>
    /// Command line interface for Unity's batch execution.
    /// </summary>
    public static class BatchMode
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        private static string GetArg(string name, string defaultValue = null)
        {
            var args = Environment.GetCommandLineArgs();
            for (int index = 0; index < args.Length; ++index)
            {
                if (args[index] == name && args.Length > index + 1)
                {
                    return args[index + 1];
                }
            }
            return defaultValue;
        }

        private static bool CheckNull(params object[] vars)
        {
            foreach (var obj in vars)
            {
                if (obj == null)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Export package by unityignore name.
        /// </summary>
        public static void Export()
        {
            string name = GetArg("_name");
            string version = GetArg("_version");
            string savePath = GetArg("_savePath");

            if (CheckNull(name, version, savePath))
                return;

            savePath = Path.GetFullPath(savePath);
            PackageExporter.Export(name, version, savePath);
        }
    }
}
#endif
