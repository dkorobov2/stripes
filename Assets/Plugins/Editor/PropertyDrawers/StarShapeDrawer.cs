using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(StarShape))]
    public class StarShapeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty resolution = property.FindPropertyRelative("resolution");
            SerializedProperty innerRadius = property.FindPropertyRelative("innerRadius");
            SerializedProperty roundType = property.FindPropertyRelative("roundType");

            EditorGUI.PropertyField(position, resolution);
			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), innerRadius);
			roundType.boolValue = GUILayout.Toggle(roundType.boolValue, "Round Dents", "Button");

			EditorGUILayout.GetControlRect(false, 15);

			EditorGUILayout.LabelField(new GUIContent("Rounding", "Adjust The Mesh Type"), EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, new Color(86.0f/255.0f, 86.0f/255.0f, 86.0f/255.0f,1));

            PropertyDrawerHelpers.ShapeRoundingsDrawer(position, property, label);
        }
    }
}