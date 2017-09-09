using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(CircleShape))]
    public class CircleShapeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty resolution = property.FindPropertyRelative("resolution");
			EditorGUI.PropertyField(position, resolution);
        }
    }
}