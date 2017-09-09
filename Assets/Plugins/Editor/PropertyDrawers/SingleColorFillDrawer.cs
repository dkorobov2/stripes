using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(SingleColorFill))]
    public class SingleColorFillDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty color = property.FindPropertyRelative("color");

			EditorGUI.PropertyField(position, color, new GUIContent(""));
        }
    }
}