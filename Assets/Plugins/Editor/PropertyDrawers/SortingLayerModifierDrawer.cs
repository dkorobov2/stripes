using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickPoly
{
	[CustomPropertyDrawer(typeof(SortingLayerModifier))]
	public class SortingLayerModifierDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty sortingLayerName = property.FindPropertyRelative("sortingLayerName");
			SerializedProperty sortingLayerID = property.FindPropertyRelative("sortingLayerID");
			SerializedProperty sortingOrder = property.FindPropertyRelative("sortingOrder");
			
			EditorGUILayout.GetControlRect(false, -20);
			
			string[] options = GetSortingLayerNames().Concat<string>(new string[] { "New Layer" }).ToArray<string>();
			int selected = EditorGUI.Popup(EditorGUILayout.GetControlRect(), "Sorting", sortingLayerID.intValue, options);
			if (selected != options.Length - 1)
			{
				sortingLayerName.stringValue = options[selected];
				sortingLayerID.intValue = selected;
			}
			else
			{
				EditorApplication.ExecuteMenuItem("Edit/Project Settings/Tags and Layers");
			}
			
			EditorGUI.PropertyField(EditorGUILayout.GetControlRect(), sortingOrder, new GUIContent("Order"));
		}
		
		private string[] GetSortingLayerNames()
		{
			Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			return (string[])sortingLayersProperty.GetValue(null, new object[0]);
		}
	}
}