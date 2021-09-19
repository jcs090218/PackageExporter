#if UNITY_EDITOR
/**
 * Copyright (c) Pixisoft. All rights reserved.
 * 
 * pixisoft.tw@gmail.com
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

        private static string GetArg(string name)
        {
            var args = Environment.GetCommandLineArgs();
            for (int index = 0; index < args.Length; ++index)
            {
                if (args[index] == name && args.Length > index + 1)
                {
                    return args[index + 1];
                }
            }
            return null;
        }

        /// <summary>
        /// Export package by unityignore name.
        /// </summary>
        public static void Export()
        {
            string name = GetArg("--name");
            string version = GetArg("--version");
            string savePath = GetArg("--savePath");

            if (name == null || version == null || savePath == null)
                return;

            savePath = Path.GetFullPath(savePath);
            PackageExporter.Export(name, version, savePath);
        }
    }
}
#endif
