using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(PivotModifier))]
    public class PivotModifierDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty pivotSnapLocation = property.FindPropertyRelative("pivotSnapLocation");
            SerializedProperty x = property.FindPropertyRelative("x");
            SerializedProperty y = property.FindPropertyRelative("y");

			EditorGUILayout.GetControlRect(false, -20);

			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), pivotSnapLocation, new GUIContent(""));
            if ((PivotSnapLocation)pivotSnapLocation.enumValueIndex == PivotSnapLocation.Custom)
            {
                EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), x);
                EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), y);
            }
        }
    }
}