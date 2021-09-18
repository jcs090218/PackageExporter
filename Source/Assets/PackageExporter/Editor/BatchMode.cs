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
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        public static void Export()
        {
            string name = GetArg("-name");
            string version = GetArg("-version");
            string savePath = GetArg("-savePath");

            savePath = Path.GetFullPath(savePath);
            PackageExporter.Export(name, version, savePath);
        }
    }
}
#endif
