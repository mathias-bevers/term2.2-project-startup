using UnityEditor;
using UnityEngine;

namespace FPSepController
{
    [CustomPropertyDrawer(typeof(PlusMinusAttribute))]
    public class PlusMinusDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUI.HelpBox(position, "This attribute only supports ints", MessageType.Error);
                return;
            }

            float fullWidth = position.width * 0.95f;

            EditorGUI.BeginProperty(position, label, property);
            
            Rect labelRect = new Rect(position.x, position.y, fullWidth * 0.5f, position.height);
            GUI.Label(labelRect, property.displayName);

            float otherPartsWidth = ((position.width * 0.95f) - labelRect.width) / 3;

            Rect partRect = new Rect(labelRect.width + position.width * 0.05f, position.y, otherPartsWidth * 0.9f, position.height);
            if (GUI.Button(partRect, "-"))
            {
                --property.intValue;
            }

            partRect.x += otherPartsWidth;
            EditorGUI.IntField(partRect, property.intValue);

            partRect.x += otherPartsWidth;
            if (GUI.Button(partRect, "+"))
            {
                ++property.intValue;
            }

			EditorGUI.EndProperty();

        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}