/**
 * Copyright (c) 2017-2023 Shen, Jen-Chieh
 * 
 * jcs090218@gmail.com
 */
using UnityEditor;
using UnityEngine;

namespace jcs090218.PackageExporter
{
    /// <summary>
    /// Utility for Editor Layout
    /// </summary>
    public static class EditorUtil
    {
        public delegate void EmptyFunction();

        /* Variables */

        private const int INDENT_LEVEL = 0;  // default to 1

        /* Setter & Getter */

        /* Functions */

        public static string FormKey(string name)
        {
            return PackageExporter.NAME + "." + name;
        }

        public static void CreateGroup(EmptyFunction func, bool flexibleSpace = false)
        {
            BeginHorizontal(() =>
            {
                BeginVertical(() =>
                {
                    Indent(func);
                });
            },
            flexibleSpace);
        }

        public static void BeginHorizontal(EmptyFunction func, bool flexibleSpace = false)
        {
            GUILayout.BeginHorizontal();
            if (flexibleSpace) GUILayout.FlexibleSpace();
            func.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void BeginVertical(EmptyFunction func)
        {
            GUILayout.BeginVertical("box");
            func.Invoke();
            GUILayout.EndVertical();
        }

        public static void Indent(EmptyFunction func)
        {
            EditorGUI.indentLevel += INDENT_LEVEL;
            func.Invoke();
            EditorGUI.indentLevel -= INDENT_LEVEL;
        }
    }
}
