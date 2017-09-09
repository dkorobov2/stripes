using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(ProportionsModifier))]
    public class ProportionsModifierDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty width = property.FindPropertyRelative("width");
            SerializedProperty height = property.FindPropertyRelative("height");
            SerializedProperty isUniform = property.FindPropertyRelative("isUniform");

			EditorGUILayout.GetControlRect(false, -20);

			isUniform.boolValue = GUILayout.Toggle(isUniform.boolValue, "Uniform", "Button");

            if (!isUniform.boolValue)
            {
                EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), width);
                EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), height);
            }
            else
            {
                EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), width, new GUIContent("Scale"));
                height.floatValue = width.floatValue;
            }
        }
    }
}