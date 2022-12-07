using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Code.Interaction;
using System;
using System.Linq;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(InteractionHandler))]
public class InteractionHandlerEditor : Editor
{

    InteractionHandler self;

    static Type InteractableInterface => typeof(IInteractable);
    List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes()).Where(p => InteractableInterface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();

    SerializedProperty reachField;

    private void OnEnable()
    {
        reachField = serializedObject.FindProperty("reach");

        self = (InteractionHandler)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Settings", MessageType.None);
        EditorGUILayout.PropertyField(reachField);
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox("Can Pickup", MessageType.None);
        EditorGUILayout.BeginHorizontal();
        bool hasAll = CheckHasAll();
        if(hasAll !=  EditorGUILayout.Toggle(hasAll, GUILayout.Width(17)))
        {
            bool earlyCheck = CheckHasAll();
            self.interactableWith.Clear();
            if (!earlyCheck)
                foreach (Type type in types)
                    self.interactableWith.Add(type.Name);
        }
        EditorGUILayout.LabelField("Select All");
        EditorGUILayout.EndHorizontal();

        foreach (Type type in types)
        {
            GUILayout.BeginHorizontal();
            bool contains = self.interactableWith.Contains(type.Name);
            if (EditorGUILayout.Toggle(contains, GUILayout.Width(17)) != contains)
                if (contains) self.interactableWith.Remove(type.Name);
                else self.interactableWith.Add(type.Name);
            
            EditorGUILayout.HelpBox(type.Name, MessageType.None, true);
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.Space(5);
        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Items in Inventory", MessageType.None);
            foreach (Pickupable p in self.inventory.getInventory)
            {
                EditorGUILayout.HelpBox(p.GetType().Name, MessageType.None, true);
            }

        }



        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(self);
            EditorSceneManager.MarkSceneDirty(self.gameObject.scene);
        }
    }

    bool CheckHasAll()
    {
        foreach (Type type in types)
            if (!self.interactableWith.Contains(type.Name))
                return false;
        

            return true;
    }
}
