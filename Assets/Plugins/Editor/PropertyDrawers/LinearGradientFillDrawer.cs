using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(LinearGradientFill))]
    public class LinearGradientFillDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty colorA = property.FindPropertyRelative("colorA");
            SerializedProperty colorB = property.FindPropertyRelative("colorB");
            SerializedProperty angle = property.FindPropertyRelative("angle");
            SerializedProperty percentage = property.FindPropertyRelative("percentage");

			if (GUI.Button(position, "Switch Color Slots"))
			{
				SwitchColors(colorA, colorB);
			}

			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), colorA, new GUIContent(""));
			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), colorB, new GUIContent(""));
            EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), angle);
            EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), percentage);
        }

        private void SwitchColors(SerializedProperty colorA, SerializedProperty colorB)
        {
            Color temp = colorB.colorValue;
            colorB.colorValue = colorA.colorValue;
            colorA.colorValue = temp;
        }
    }
}