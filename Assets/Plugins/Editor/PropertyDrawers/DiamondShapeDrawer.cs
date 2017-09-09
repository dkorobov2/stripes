using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(DiamondShape))]
    public class DiamondShapeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty topWidth = property.FindPropertyRelative("topWidth");
            SerializedProperty middleHeight = property.FindPropertyRelative("middleHeight");

            EditorGUI.PropertyField(position, topWidth);
            EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), middleHeight);

			EditorGUILayout.GetControlRect(false, 15);
			
			EditorGUILayout.LabelField("Rounding", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, new Color(86.0f/255.0f, 86.0f/255.0f, 86.0f/255.0f,1));

            PropertyDrawerHelpers.ShapeRoundingsDrawer(position, property, label);
        }
    }
}