/**
 * Copyright (c) 2017-2025 Shen, Jen-Chieh
 * 
 * jcs090218@gmail.com
 */
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor;
using UnityEditor.Experimental;

namespace jcs090218.PackageExporter
{
    /// <summary>
    /// Renders tree view to `PackageExport` window.
    /// </summary>
    public class ExportTreeView : TreeView<int>
    {
        /* Variables */

        private PackageExport mPackageExport;

        internal static class Constants
        {
            public static Texture2D folderIcon = EditorGUIUtility.FindTexture(EditorResources.folderIconName);
        }

        /* Setter & Getter */

        /* Functions */

        public ExportTreeView(PackageExport exportWindow, TreeViewState<int> state)
            : base(state)
        {
            this.mPackageExport = exportWindow;
            Reload();

            ExpandAll();
        }

        protected override TreeViewItem<int> BuildRoot()
        {
            var root = new TreeViewItem<int> { id = 0, depth = -1, displayName = "Root" };
            var dirs = new Dictionary<string, TreeViewItem<int>>();
            int id = 1;

            // Sort by name, so it looks nicer
            var sorted = mPackageExport.exportList.OrderBy(f => f);

            foreach (var file in sorted)
            {
                // Get path to walk through
                string[] folders = file.Split('/');

                TreeViewItem<int> parent = root;
                string current = "";

                foreach (var folder in folders)
                {
                    current = Path.Combine(current, folder);

                    if (folder == "Assets")
                        continue;

                    if (dirs.ContainsKey(current))
                    {
                        parent = dirs[current];
                        continue;
                    }

                    var newItem = new TreeViewItem<int> { id = id, displayName = folder };

                    var icon = Constants.folderIcon;
                    if (File.Exists(current))
                    {
                        icon = (Texture2D)AssetDatabase.GetCachedIcon(current);

                        if (icon == null)
                            icon = InternalEditorUtility.GetIconForFile(current);
                    }

                    newItem.icon = icon;

                    dirs.Add(current, newItem);
                    parent.AddChild(newItem);

                    parent = newItem;
                    ++id;
                }
            }

            SetupDepthsFromParentsAndChildren(root);

            return root;
        }
    }
}
