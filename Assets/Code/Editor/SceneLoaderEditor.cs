using Code.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneLoaderEditor : EditorWindow
{
    [MenuItem("Scenes/List", priority = 1, validate = false)]
    public static void Init()
    {
        EditorWindow window = GetWindow<SceneLoaderEditor>();
        window.position = new Rect(320, 50, 200f, 500f);
        window.Show();
    }

    bool initializedPosition = false;

    void OnGUI()
    {
        if (!initializedPosition)
        {
            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            position = new Rect(mousePos.x - 15, mousePos.y - 15, position.width, position.height);
            initializedPosition = true;
        }

        string[] scenes = PropertyDrawersHelper.AllScenePaths();
        foreach(string scene in scenes)
        {
            string sceneName = scene.Substring(scene.LastIndexOf('/') + 1);
            sceneName = sceneName.Substring(0, sceneName.Length - 6);

            if (GUILayout.Button(sceneName))
            {
                if (Application.isPlaying) EditorSceneManager.LoadScene(scene);
                else EditorSceneManager.OpenScene(scene);
                Close();
            }
        }
    }
}
