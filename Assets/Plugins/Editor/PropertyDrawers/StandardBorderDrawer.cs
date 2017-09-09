using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(StandardBorder))]
    public class StandardBorderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty outerScale = property.FindPropertyRelative("outerScale");
	        outerScale.floatValue = Mathf.Max(outerScale.floatValue, 0);
            SerializedProperty innerScale = property.FindPropertyRelative("innerScale");
            SerializedProperty color = property.FindPropertyRelative("color");

			EditorGUI.PropertyField(position, color, new GUIContent(""));
            EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), outerScale);
            EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), innerScale);
        }
    }
}