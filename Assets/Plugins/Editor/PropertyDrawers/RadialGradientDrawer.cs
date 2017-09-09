using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(RadialGradientFill))]
    public class RadialGradientFillDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty colorA = property.FindPropertyRelative("colorA");
            SerializedProperty colorB = property.FindPropertyRelative("colorB");
            SerializedProperty percentage = property.FindPropertyRelative("percentage");

			if (GUI.Button(position, "Switch Color Slots"))
			{
				SwitchColors(colorA, colorB);
			}

			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), colorA, new GUIContent(""));
			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), colorB, new GUIContent(""));
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