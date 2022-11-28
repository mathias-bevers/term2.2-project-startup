using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Code.Interaction;
using System.Reflection;
using System.Linq;
using System.Configuration.Assemblies;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

[CustomPropertyDrawer(typeof(InteractableWith), true)]
public class InteractableWithEditor : PropertyDrawer
{
    static Type InteractableInterface => typeof(IInteractable);
    List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(s => s.GetTypes()).Where(p => InteractableInterface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToList();



    InteractableWith value;

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
    {
        if (value == null)
        {
            UnityEngine.Object targetObj = prop.serializedObject.targetObject;
            value = (InteractableWith)fieldInfo.GetValue(targetObj);
        }
        indentDown = 0;
        if (value == null) return;
        
        EditorGUI.BeginProperty(position, label, prop);
        //EditorGUI.indentLevel++;



        foreach (Type type in types)
        {
            bool curToggle = EditorGUI.Toggle(new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * indentDown), EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight), value.Contains(type.Name));
            if (curToggle) value.AddElement(type.Name);
            else value.RemoveElement(type.Name);
            EditorGUI.HelpBox(GetRect(position), type.Name, MessageType.None);
        }

        EditorGUI.EndProperty();
        prop.serializedObject.ApplyModifiedProperties();

       // base.OnGUI(position, prop, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return indentDown * EditorGUIUtility.singleLineHeight;
    }

    int indentDown = 0;

    public Rect GetRect(Rect position)
    {
        Rect r = new Rect(position.x + EditorGUIUtility.singleLineHeight, position.y + (EditorGUIUtility.singleLineHeight * indentDown), position.width - (EditorGUIUtility.singleLineHeight), EditorGUIUtility.singleLineHeight);
        indentDown++;
        return r;
    }

    public System.Object GetPropertyInstance(SerializedProperty property)
    {

        string path = property.propertyPath;

        System.Object obj = property.serializedObject.targetObject;
        var type = obj.GetType();

        var fieldNames = path.Split('.');
        for (int i = 0; i < fieldNames.Length; i++)
        {
            var info = type.GetField(fieldNames[i]);
            if (info == null)
                break;

            // Recurse down to the next nested object.
            obj = info.GetValue(obj);
            type = info.FieldType;
        }

        return obj;
    }

}
