using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    [CustomPropertyDrawer(typeof(FacingModifier))]
    public class FacingModifierDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty facingDirection = property.FindPropertyRelative("facingDirection");
			
			EditorGUILayout.GetControlRect(false, -20);

			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), facingDirection, new GUIContent(""));
        }
    }
}