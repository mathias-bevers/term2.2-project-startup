using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.Utils
{
    public static class PropertyDrawersHelper
    {
#if UNITY_EDITOR

        public static string[] AllSceneNames()
        {
            List<string> temp = new List<string>();
            foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                    name = name.Substring(0, name.Length - 6);
                    temp.Add(name);
                }
            }

            return temp.ToArray();
        }

        public static string[] AllScenePaths()
        {
            List<string> temp = new List<string>();
            foreach (EditorBuildSettingsScene S in EditorBuildSettings.scenes)
            {
                if (S.enabled)
                {
                    string name = S.path;
                    temp.Add(name);
                }
            }

            return temp.ToArray();
        }

        public static string[] GetAllAxes()
        {
            List<string> allAxis = new();

            Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
            SerializedObject obj = new(inputManager);
            SerializedProperty axes = obj.FindProperty("m_Axes");

            if (axes.arraySize == 0) { Debug.LogWarning("No axes found!"); }

            for (int i = 0; i < axes.arraySize; ++i)
            {
                SerializedProperty axis = axes.GetArrayElementAtIndex(i);
                string axisName = axis.FindPropertyRelative("m_Name").stringValue;

                allAxis.Add(axisName);
			}

            return allAxis.ToArray();
        }
#endif
    }
}