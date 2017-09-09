using UnityEngine;
using UnityEditor;

namespace QuickPoly
{
    public class PropertyDrawerHelpers
    {
        public static void ShapeRoundingsDrawer(Rect position, SerializedProperty property, GUIContent label)
        {
			SerializedProperty roundingsDistances = property.FindPropertyRelative("roundingsDistances");
			SerializedProperty maxRoudningDistance = property.FindPropertyRelative("maxRoudningDistance");
			
			int cornerIndex = 0;
	        bool isAny = false;

			if (QuickPolygonEditor.showRoundingSlider.Length != roundingsDistances.arraySize)
			{
				QuickPolygonEditor.showRoundingSlider = new bool[roundingsDistances.arraySize];
				for (int i = 0; i < roundingsDistances.arraySize; i++)
				{
					QuickPolygonEditor.showRoundingSlider[i] = false;
				}
			}
	        for (int i = 0; i < QuickPolygonEditor.showRoundingSlider.Length; i++)
	        {
		        if (QuickPolygonEditor.showRoundingSlider[i])
		        {
			        isAny = true;
			        cornerIndex = i;
			        break;
		        }
	        }
	        if (isAny)
			{
				float sliderValue = EditorGUILayout.Slider(new GUIContent("Resolution"),
					roundingsDistances.GetArrayElementAtIndex(cornerIndex).floatValue, 0f, maxRoudningDistance.floatValue);
				cornerIndex = 0;
				foreach (SerializedProperty rounding in roundingsDistances)
		        {
			        if (QuickPolygonEditor.showRoundingSlider[cornerIndex])
			        {
						rounding.floatValue = sliderValue;
					}
			        cornerIndex++;
				}
		    }

	        EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Select All Corners"))
				{
					for (int i = 0; i < QuickPolygonEditor.showRoundingSlider.Length; i++)
					{
						QuickPolygonEditor.showRoundingSlider[i] = true;
					}
				}
				if (GUILayout.Button("Deselect All Corners"))
				{
					for (int i = 0; i < QuickPolygonEditor.showRoundingSlider.Length; i++)
					{
						QuickPolygonEditor.showRoundingSlider[i] = false;
					}
				}
			EditorGUILayout.EndHorizontal();
        }
    }
}