using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickPoly
{
	[CustomEditor(typeof(QuickPolygon))]
	[CanEditMultipleObjects]
	[Serializable]
	public class QuickPolygonEditor : Editor
	{

		public static string version = "1.2";

		#region MenuItems

		[MenuItem("Help/QuickPoly/Documentation")]
		private static void Documentation()
		{
			Application.OpenURL("https://www.quickpoly.com#documentation");
		}

		[MenuItem("Help/QuickPoly/Support")]
		private static void Support()
		{
			
			Application.OpenURL("https://www.quickpoly.com#support");
		}

		[MenuItem("Help/QuickPoly/Report")]
		private static void Report()
		{
			Application.OpenURL("https://www.quickpoly.com#contact");
		}

		[MenuItem("Help/QuickPoly/About")]
		private static void About()
		{
			AboutWindow window  = (AboutWindow)EditorWindow.GetWindow(typeof(AboutWindow), true);
			window.Show();
		}

		[MenuItem("GameObject/2D Object/Square/Create &#s")]
		private static void GenerateSquare()
		{
			QuickPolygon.Create("", MeshType.Square, false);
		}

		[MenuItem("GameObject/2D Object/Circle/Create &#c")]
		private static void GenerateCircle()
		{
			QuickPolygon.Create("", MeshType.Circle, false);
		}

		[MenuItem("GameObject/2D Object/Star/Create &#r")]
		private static void GenerateStar()
		{
			QuickPolygon.Create("", MeshType.Star, false);
		}

		[MenuItem("GameObject/2D Object/Diamond/Create &#d")]
		private static void GenerateSDiamond()
		{
			QuickPolygon.Create("", MeshType.Diamond, false);
		}

		[MenuItem("GameObject/2D Object/Triangle/Create &#t")]
		private static void GenerateTriangle()
		{
			QuickPolygon.Create("", MeshType.Triangle, false);
		}

		[MenuItem("GameObject/2D Object/Pentagon/Create &#p")]
		private static void GeneratePentagon()
		{
			QuickPolygon.Create("", MeshType.Pentagon, false);
		}

		[MenuItem("GameObject/2D Object/Hexagon/Create &#h")]
		private static void GenerateHexagon()
		{
			QuickPolygon.Create("", MeshType.Hexagon, false);
		}

		// Presets

		[MenuItem("GameObject/2D Object/Square/Create From Last Preset")]
		private static void GenerateSquareFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Square, false);
		}

		[MenuItem("GameObject/2D Object/Circle/Create From Last Preset")]
		private static void GenerateCircleFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Circle, false);
		}

		[MenuItem("GameObject/2D Object/Star/Create From Last Preset")]
		private static void GenerateStarFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Star, false);
		}

		[MenuItem("GameObject/2D Object/Diamond/Create From Last Preset")]
		private static void GenerateDiamondFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Diamond, false);
		}

		[MenuItem("GameObject/2D Object/Triangle/Create From Last Preset")]
		private static void GenerateTriangleFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Triangle, false);
		}

		[MenuItem("GameObject/2D Object/Pentagon/Create From Last Preset")]
		private static void GeneratePentagonFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Pentagon, false);
		}

		[MenuItem("GameObject/2D Object/Hexagon/Create From Last Preset")]
		private static void GenerateHexagonFromLastPreset()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Hexagon, false);
		}

		[MenuItem("GameObject/2D Object/Square/Create From Most Used Preset")]
		private static void GenerateSquareFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Square, false);
		}

		[MenuItem("GameObject/2D Object/Circle/Create From Most Used Preset")]
		private static void GenerateCircleFromMostUsedPresetGO()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Circle, false);
		}

		[MenuItem("GameObject/2D Object/Star/Create From Most Used Preset")]
		private static void GenerateStarFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Star, false);
		}

		[MenuItem("GameObject/2D Object/Diamond/Create From Most Used Preset")]
		private static void GenerateDiamondFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Diamond, false);
		}

		[MenuItem("GameObject/2D Object/Triangle/Create From Most Used Preset")]
		private static void GenerateTriangleFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Triangle, false);
		}

		[MenuItem("GameObject/2D Object/Pentagon/Create From Most Used Preset")]
		private static void GeneratePentagonFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Pentagon, false);
		}

		[MenuItem("GameObject/2D Object/Hexagon/Create From Most Used Preset")]
		private static void GenerateHexagonFromMostUsedPreset()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Hexagon, false);
		}

		// UI

		[MenuItem("GameObject/UI/Square/Create &#s")]
		private static void GenerateSquareUI()
		{
			QuickPolygon.Create("", MeshType.Square, true);
		}
		
		[MenuItem("GameObject/UI/Circle/Create &#c")]
		private static void GenerateCircleUI()
		{
			QuickPolygon.Create("", MeshType.Circle, true);
		}
		
		[MenuItem("GameObject/UI/Star/Create &#r")]
		private static void GenerateStarUI()
		{
			QuickPolygon.Create("", MeshType.Star, true);
		}
		
		[MenuItem("GameObject/UI/Diamond/Create &#d")]
		private static void GenerateSDiamondUI()
		{
			QuickPolygon.Create("", MeshType.Diamond, true);
		}
		
		[MenuItem("GameObject/UI/Triangle/Create &#t")]
		private static void GenerateTriangleUI()
		{
			QuickPolygon.Create("", MeshType.Triangle, true);
		}
		
		[MenuItem("GameObject/UI/Pentagon/Create &#p")]
		private static void GeneratePentagonUI()
		{
			QuickPolygon.Create("", MeshType.Pentagon, true);
		}
		
		[MenuItem("GameObject/UI/Hexagon/Create &#h")]
		private static void GenerateHexagonUI()
		{
			QuickPolygon.Create("", MeshType.Hexagon, true);
		}
		
		// UI Presets
		
		[MenuItem("GameObject/UI/Square/Create From Last Preset")]
		private static void GenerateSquareFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Square, true);
		}
		
		[MenuItem("GameObject/UI/Circle/Create From Last Preset")]
		private static void GenerateCircleFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Circle, true);
		}
		
		[MenuItem("GameObject/UI/Star/Create From Last Preset")]
		private static void GenerateStarFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Star, true);
		}
		
		[MenuItem("GameObject/UI/Diamond/Create From Last Preset")]
		private static void GenerateDiamondFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Diamond, true);
		}
		
		[MenuItem("GameObject/UI/Triangle/Create From Last Preset")]
		private static void GenerateTriangleFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Triangle, true);
		}
		
		[MenuItem("GameObject/UI/Pentagon/Create From Last Preset")]
		private static void GeneratePentagonFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Pentagon, true);
		}
		
		[MenuItem("GameObject/UI/Hexagon/Create From Last Preset")]
		private static void GenerateHexagonFromLastPresetUI()
		{
			QuickPolygon.CreateFromLastPreset("", MeshType.Hexagon, true);
		}
		
		[MenuItem("GameObject/UI/Square/Create From Most Used Preset")]
		private static void GenerateSquareFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Square, true);
		}
		
		[MenuItem("GameObject/UI/Circle/Create From Most Used Preset")]
		private static void GenerateCircleFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Circle, true);
		}
		
		[MenuItem("GameObject/UI/Star/Create From Most Used Preset")]
		private static void GenerateStarFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Star, true);
		}
		
		[MenuItem("GameObject/UI/Diamond/Create From Most Used Preset")]
		private static void GenerateDiamondFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Diamond, true);
		}
		
		[MenuItem("GameObject/UI/Triangle/Create From Most Used Preset")]
		private static void GenerateTriangleFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Triangle, true);
		}
		
		[MenuItem("GameObject/UI/Pentagon/Create From Most Used Preset")]
		private static void GeneratePentagonFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Pentagon, true);
		}
		
		[MenuItem("GameObject/UI/Hexagon/Create From Most Used Preset")]
		private static void GenerateHexagonFromMostUsedPresetUI()
		{
			QuickPolygon.CreateFromMostUsedPreset("", MeshType.Hexagon, true);
		}

		#endregion

		#region Unity

		private string PresetName = "";
		public static bool[] showRoundingSlider;
		
		private void Awake()
		{
			RefreshTargets();
			PresetsManager.RefreshPresetContainer();
			HideScriptIcon(false);
		}

		private void OnEnable()
		{
			SceneView.onSceneGUIDelegate += HandlePivots;
			SceneView.onSceneGUIDelegate += DestroyRoutine;
			Undo.undoRedoPerformed += RefreshTargets;
			if (showRoundingSlider == null || showRoundingSlider.Length != ((QuickPolygon)target).Shape.roundings.Length)
			{
				ResetRoundingSlider();
			}
		}

		private void OnDisable()
		{
			SceneView.onSceneGUIDelegate -= HandlePivots;
			SceneView.onSceneGUIDelegate -= DestroyRoutine;
			Undo.undoRedoPerformed -= RefreshTargets;
		}

		#endregion

		#region Inspector

		Color seperatorColor = new Color(86.0f/255.0f, 86.0f/255.0f, 86.0f/255.0f,1);

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUILayout.BeginVertical();
			EditorGUILayout.Separator();
			DrawUIInspector();
			EditorGUILayout.Separator();
			DrawPresetsInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawShapeInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawFillInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawBorderInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawProportionsInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawSortingLayersInspector();
			if (serializedObject.FindProperty("IsUI").boolValue == false) 
			{
				EditorGUILayout.GetControlRect(false, 15);
				DrawPivotInspector();
			}
			EditorGUILayout.GetControlRect(false, 15);
			DrawColliderInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawLightingInspector();
			EditorGUILayout.GetControlRect(false, 15);
			DrawFacingInspector();
			EditorGUILayout.Separator();
			// Check if inspector values have changed by user or by an undo operation.
			if (GUI.changed)
			{
				RefreshTargets();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawUIInspector()
		{
			SerializedProperty UIProp = serializedObject.FindProperty("IsUI");
			EditorGUILayout.LabelField("UI", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);

			UIProp.boolValue = GUILayout.Toggle(UIProp.boolValue, "UI Element", "Button");
		}

		private void DrawPresetsInspector()
		{
			EditorGUILayout.LabelField("Preset", EditorStyles.boldLabel);

			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);

			DrawPresetsSaveInspector();
			DrawPresetsLoadInspector();
		}

		private void DrawPresetsSaveInspector()
		{
			// Don't allow saving presets if multiobject editing.
			if (!serializedObject.isEditingMultipleObjects)
			{
				PresetName = EditorGUILayout.TextField("Name", PresetName);

				if (PresetsManager.IsPresetExists(PresetName))
				{
					if (GUILayout.Button("Update"))
					{
						if (!string.IsNullOrEmpty(PresetName))
						{
							PresetsManager.SavePreset(PresetName, (QuickPolygon) targets[0]);
							PresetName = "";
							GUI.FocusControl(null);
						}
					}
				}
				else
				{
					if (GUILayout.Button("Save"))
					{
						if (!string.IsNullOrEmpty(PresetName) && PresetName != "None")
						{
							PresetsManager.SavePreset(PresetName, (QuickPolygon) targets[0]);
							PresetName = "";
							GUI.FocusControl(null);
						}
					}
				}
				if (!string.IsNullOrEmpty(((QuickPolygon)targets[0]).CurrentPresetName) &&
				    ((QuickPolygon) targets[0]).CurrentPresetName != "None" &&
				    PresetsManager.IsPresetExists(((QuickPolygon) targets[0]).CurrentPresetName))
				{
					EditorGUILayout.LabelField("Selected", ((QuickPolygon) targets[0]).CurrentPresetName);
					if (GUILayout.Button("Update"))
					{
						if (!string.IsNullOrEmpty(((QuickPolygon) targets[0]).CurrentPresetName))
						{
							PresetsManager.SavePreset(((QuickPolygon) targets[0]).CurrentPresetName, (QuickPolygon) targets[0]);
							GUI.FocusControl(null);
						}
					}
				}
			}
			else
			{
				EditorGUILayout.HelpBox(MSG.Warnings.PRESET_MULTIOBJECT_SAVING_DISABLED, MessageType.Info);
			}
		}

		private void DrawPresetsLoadInspector()
		{
			// Check if all selected shapes share the same UI setup.
			bool allowMultiobjectLoading = true;
			bool isFirstShapeUI = ((QuickPolygon)targets[0]).IsUI;
			for (int i = 1; i < targets.Length && allowMultiobjectLoading; i++)
			{
				if (((QuickPolygon)targets[i]).IsUI != isFirstShapeUI)
				{
					allowMultiobjectLoading = false;
				}
			}
			
			if (allowMultiobjectLoading)
			{
				// Check if all selected shapes share the same selected preset.
				bool arePresetsMatching = true;
				string firstShapePresetName = ((QuickPolygon)targets[0]).CurrentPresetName;
				for (int i = 1; i < targets.Length && arePresetsMatching; i++)
				{
					if (((QuickPolygon)targets[i]).CurrentPresetName != firstShapePresetName)
					{
						arePresetsMatching = false;
					}
				}
				
				// Build popup options list.
				List<PresetsRecord> presets = PresetsManager.presetsContainer.PresetsGOList;
				if (presets.Any())
				{
					string[] presetNames = presets.OrderBy(p => p.name).Select(p => p.name).ToArray();
					string[] options = arePresetsMatching ? new string[] { "None" } : new string[] { "—", "None" };
					options = options.Concat(presetNames).ToArray();
				
					int selectedPreset = 0;
					if (arePresetsMatching)
					{
						selectedPreset = Array.IndexOf(options, firstShapePresetName);
						if (selectedPreset < 0)
						{
							selectedPreset = 0;
							((QuickPolygon) targets[0]).CurrentPresetName = "";
						}
					}
				
					int newPreset = EditorGUILayout.Popup("Load", selectedPreset, options);
				
					if (newPreset != selectedPreset && options[newPreset] != "—" && options[newPreset] != "None")
					{
						// Load the preset for all of the targets.
						Undo.RecordObjects(targets, "Preset Load");
						for (int i = 0; i < targets.Length; i++)
						{
							PresetsManager.LoadPresetFor(options[newPreset], (QuickPolygon)targets[i]);
						}
					}
				
					if (options[newPreset] == "None")
					{
						for (int i = 0; i < targets.Length; i++)
						{
							((QuickPolygon)targets[i]).CurrentPresetName = "None";
						}
					}

					SerializedProperty allowLoadGeometryProp = serializedObject.FindProperty("AllowLoadPresetGeometry");
					allowLoadGeometryProp.boolValue = GUILayout.Toggle(allowLoadGeometryProp.boolValue, "Load Mesh", "Button");
				
					if (options[selectedPreset] != "—" && options[selectedPreset] != "None")
					{
						if (GUILayout.Button("Delete"))
						{
							for (int i = 0; i < targets.Length; i++)
							{
								if (((QuickPolygon) targets[i]).CurrentPresetName == options[selectedPreset])
									((QuickPolygon) targets[i]).CurrentPresetName = "None";
							}
							PresetsManager.DeletePreset(options[selectedPreset]);
						}
					}
				}
			}
			else
			{
				EditorGUILayout.HelpBox(MSG.Warnings.PRESET_MULTIOBJECT_LOADING_DISABLED, MessageType.Info);
			}
		}

		private void DrawShapeInspector()
		{
			SerializedProperty meshTypeProp = serializedObject.FindProperty("ShapeMeshIndex");
			EditorGUILayout.LabelField("Mesh Type", EditorStyles.boldLabel);

			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);

			EditorGUILayout.PropertyField(meshTypeProp, new GUIContent(""));

			bool allowMultiobjectEditing = true;
			MeshType firstShapeMeshType = ((QuickPolygon) targets[0]).ShapeMeshIndex;
			for (int i = 1; i < targets.Length && allowMultiobjectEditing; i++)
			{
				if (((QuickPolygon) targets[i]).ShapeMeshIndex != firstShapeMeshType)
				{
					allowMultiobjectEditing = false;
				}
			}

			if (allowMultiobjectEditing)
			{
				SerializedProperty shapeProp;
				switch ((MeshType) meshTypeProp.enumValueIndex)
				{
					case MeshType.Circle:
						shapeProp = serializedObject.FindProperty("CircleShape");
						break;
					case MeshType.Star:
						shapeProp = serializedObject.FindProperty("StarShape");
						break;
					case MeshType.Diamond:
						shapeProp = serializedObject.FindProperty("DiamondShape");
						break;
					case MeshType.Triangle:
						shapeProp = serializedObject.FindProperty("TriangleShape");
						break;
					case MeshType.Pentagon:
						shapeProp = serializedObject.FindProperty("PentagonShape");
						break;
					case MeshType.Hexagon:
						shapeProp = serializedObject.FindProperty("HexagonShape");
						break;
					default:
						shapeProp = serializedObject.FindProperty("SquareShape");
						break;
				}
				EditorGUILayout.PropertyField(shapeProp);
			}
			else
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.HelpBox(MSG.Warnings.SHAPE_MULTIOBJECT_SETUP_DISABLED, MessageType.Info);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawFillInspector()
		{
			SerializedProperty fillTypeProp = serializedObject.FindProperty("ShapeFillIndex");
			EditorGUILayout.LabelField("Fill Type", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(fillTypeProp, new GUIContent(""));
			
			bool allowMultiobjectEditing = true;
			FillType firstShapeFillType = ((QuickPolygon) targets[0]).ShapeFillIndex;
			for (int i = 1; i < targets.Length && allowMultiobjectEditing; i++)
			{
				if (((QuickPolygon) targets[i]).ShapeFillIndex != firstShapeFillType)
				{
					allowMultiobjectEditing = false;
				}
			}
			
			if (allowMultiobjectEditing)
			{
				SerializedProperty fillProp;
				switch ((FillType) fillTypeProp.enumValueIndex)
				{
				case FillType.Unicolor:
					fillProp = serializedObject.FindProperty("SingleColorFill");
					EditorGUILayout.PropertyField(fillProp);
					break;
				case FillType.LinearGradient:
					fillProp = serializedObject.FindProperty("LinearGradientFill");
					EditorGUILayout.PropertyField(fillProp);
					break;
				case FillType.RadialGradient:
					fillProp = serializedObject.FindProperty("RadialGradientFill");
					EditorGUILayout.PropertyField(fillProp);
					break;
				default:
					break;
				}
			}
			else
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.HelpBox(MSG.Warnings.FILL_MULTIOBJECT_SETUP_DISABLED, MessageType.Info);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawBorderInspector()
		{
			SerializedProperty useBorderProp = serializedObject.FindProperty("UseShapeBorder");
			EditorGUILayout.LabelField("Border", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(useBorderProp, new GUIContent(""));
			
			bool allowMultiobjectEditing = true;
			BorderFillType firstShapeBorderType = ((QuickPolygon) targets[0]).UseShapeBorder;
			for (int i = 1; i < targets.Length && allowMultiobjectEditing; i++)
			{
				if (((QuickPolygon) targets[i]).UseShapeBorder != firstShapeBorderType)
				{
					allowMultiobjectEditing = false;
				}
			}
			
			if (allowMultiobjectEditing)
			{
				switch ((BorderFillType) useBorderProp.enumValueIndex)
				{
				case BorderFillType.Unicolor:
					SerializedProperty standardBorderProp = serializedObject.FindProperty("StandardBorder");
					EditorGUILayout.PropertyField(standardBorderProp);
					break;
				default:
					break;
				}
			}
			else
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.HelpBox(MSG.Warnings.BORDER_MULTIOBJECT_SETUP_DISABLED, MessageType.Info);
				EditorGUI.indentLevel--;
			}
		}

		private void DrawProportionsInspector()
		{
			SerializedProperty proportionsModifierProp = serializedObject.FindProperty("ProportionsModifier");
			EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(proportionsModifierProp);
		}

		private void DrawSortingLayersInspector()
		{
			SerializedProperty sortingLayerModifierProp = serializedObject.FindProperty("SortingLayerModifier");
			EditorGUILayout.LabelField("Layer", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(sortingLayerModifierProp);
		}

		private void DrawPivotInspector()
		{
			SerializedProperty pivotModifierProp = serializedObject.FindProperty("PivotModifier");
			EditorGUILayout.LabelField("Pivot", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(pivotModifierProp);
		}

		private void DrawColliderInspector()
		{
			SerializedProperty colliderProp = serializedObject.FindProperty("ShapeColliderIndex");
			EditorGUILayout.LabelField("Collider", EditorStyles.boldLabel);
			
			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);
			
			EditorGUILayout.PropertyField(colliderProp, new GUIContent(""));
			
			bool allowMultiobjectEditing = false;
			for (int i = 0; i < targets.Length && !allowMultiobjectEditing; i++)
			{
				if (((QuickPolygon) targets[i]).ShapeColliderIndex != ColliderType.None)
				{
					allowMultiobjectEditing = true;
				}
			}
			
			if (allowMultiobjectEditing)
			{
				if (GUILayout.Button("Remove Collider"))
				{
					Undo.RecordObjects(targets, "Remove Collider");
					for (int i = 0; i < targets.Length; i++)
					{
						ColliderManager.RemoveColliders((QuickPolygon) targets[i]);
					}
				}
			}
		}
		
		private void DrawLightingInspector()
		{
			SerializedProperty lightingProp = serializedObject.FindProperty("lighting");
			EditorGUILayout.LabelField("Lighting", EditorStyles.boldLabel);

			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);

			EditorGUILayout.PropertyField(lightingProp, new GUIContent(""));
		}

		private void DrawFacingInspector()
		{
			SerializedProperty facingModifierProp = serializedObject.FindProperty("FacingModifier");
			EditorGUILayout.LabelField("Facing Direction", EditorStyles.boldLabel);

			Rect rect = EditorGUILayout.GetControlRect(false, 2);
			rect.x = 0.0f;
			rect.y -= 1.0f;
			rect.width = Screen.width;
			EditorGUI.DrawRect(rect, seperatorColor);

			EditorGUILayout.PropertyField(facingModifierProp);
		}

		#endregion

		#region Helpers

		public void ResetRoundingSlider()
		{
			showRoundingSlider = new bool[((QuickPolygon)target).Shape.roundings.Length];
			for (int i = 0; i < showRoundingSlider.Length; i++)
			{
				showRoundingSlider[i] = false;
			}
		}
		private static void HideScriptIcon(bool gizmosOn)
		{
			int val = gizmosOn ? 1 : 0;
			Assembly asm = Assembly.GetAssembly(typeof(Editor));
			Type type = asm.GetType("UnityEditor.AnnotationUtility");
			if (type != null)
			{
				MethodInfo getAnnotations = type.GetMethod("GetAnnotations", BindingFlags.Static | BindingFlags.NonPublic);
				MethodInfo setIconEnabled = type.GetMethod("SetIconEnabled", BindingFlags.Static | BindingFlags.NonPublic);
				var annotations = getAnnotations.Invoke(null, null);
				foreach (object annotation in (IEnumerable)annotations)
				{
					Type annotationType = annotation.GetType();
					FieldInfo classIdField = annotationType.GetField("classID", BindingFlags.Public | BindingFlags.Instance);
					FieldInfo scriptClassField = annotationType.GetField("scriptClass", BindingFlags.Public | BindingFlags.Instance);
					if (classIdField != null && scriptClassField != null)
					{
						int classId = (int)classIdField.GetValue(annotation);
						string scriptClass = (string)scriptClassField.GetValue(annotation);
						if (scriptClass == "QuickPolygon")
							setIconEnabled.Invoke(null, new object[] { classId, scriptClass, val });
					}
				}
			}
		}
		private void DestroyRoutine(SceneView sceneView)
		{
			while (Helpers.destroyQueue.Count > 0)
			{
				if (Application.isPlaying)
				{
					Destroy(Helpers.destroyQueue.Dequeue());
				}
				else
				{
					DestroyImmediate(Helpers.destroyQueue.Dequeue());
				}
			}
		}

		private void RefreshTargets()
		{
			if (serializedObject.targetObject != null)
			{
				serializedObject.ApplyModifiedProperties();

				for (int i = 0; i < targets.Length; i++)
				{
					QuickPolygon shape = (QuickPolygon) targets[i];
					if (shape != null)
					{
						if (PrefabUtility.GetPrefabParent(shape.gameObject) != null ||
						    PrefabUtility.GetPrefabObject(shape.gameObject) == null)
						{
							shape.UpdateTransformType();
							shape.RecalculateMesh();
						}
						EditorUtility.SetDirty(shape);
					}
				}
			}
		}

		#endregion

		#region PivotHandles

		private QuickPolygon currentHandleShape;
		private static readonly Color pivotHandleBorderColor = new Color(244.0f/255.0f, 157.0f/255.0f, 0.0f/255.0f, 1);
		private static readonly Color pivotHandleFillColor = new Color(250.0f/255.0f, 250.0f/255.0f, 250.0f/255.0f, 1);
		private static readonly Color roundingHandleBorderColor = new Color(86.0f/255.0f, 86.0f/255.0f, 86.0f/255.0f, 1);
		private static readonly Color roundingHandleFillColor = new Color(239.0f/255.0f, 239.0f/255.0f, 239.0f/255.0f, 1);
		private const float pivotHandleSize = 0.075f;
		private const float pivotSnapRange = 0.1f;
		private bool dragging = false;

		private void HandlePivots(SceneView sceneView)
		{
			for (int i = 0; i < targets.Length; i++) 
			{
				currentHandleShape = (QuickPolygon) targets[i];
				if (currentHandleShape == null)
				{
					continue;
				}
				// Handle movement vector is just used to check if the user moved the handle in the last frame.
				Vector3 handlePosition = Handles.FreeMoveHandle(currentHandleShape.transform.position,
					Quaternion.identity, Mathf.Min(currentHandleShape.ProportionsModifier.width,
						currentHandleShape.ProportionsModifier.height)*pivotHandleSize, Vector3.zero,
					DrawPivotHandle);
				Vector3 handleMovement = handlePosition - currentHandleShape.transform.position;
				Plane plane;
				if (targets.Length == 1 && !currentHandleShape.IsCircle())
				{

					int[] list = currentHandleShape.Shape.uniform && !(currentHandleShape.Shape is StarShape) && currentHandleShape.Shape.roundings[0] <= 1
						? currentHandleShape.Shape.GetData().BorderVertices
						: currentHandleShape.Shape.GetData().RoundableVertices;
					bool rmb = (Event.current.type == EventType.MouseUp && Event.current.button == 0);
					bool drag = (Event.current.type == EventType.MouseDrag && Event.current.button == 0);

					if (drag)
					{
						dragging = true;
					}
					if (dragging && rmb)
					{
						rmb = false;
						dragging = false;
					}

					if (showRoundingSlider.Length != list.Length)
					{
						showRoundingSlider = new bool[list.Length];
						for (int j = 0; j < showRoundingSlider.Length; j++)
						{
							showRoundingSlider[j] = false;
						}
					}
					for (int j = 0; j < list.Length; j++)
					{
						Vector3 polygonCenter = currentHandleShape.transform.position - currentHandleShape.ToPivotVector()+ currentHandleShape.Shape.GetData().Vertices[0];
						Vector3 position =  polygonCenter + 0.95f * (-polygonCenter +currentHandleShape.transform.TransformPoint(currentHandleShape.LastData.Vertices[list[j]]));
						if (rmb)
						{
							plane = new Plane(currentHandleShape.transform.forward, currentHandleShape.transform.position);
							Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
							float enter = 100.0f;
							if (plane.Raycast(ray, out enter))
							{
								Vector3 mouseWorldPosition = ray.GetPoint(enter);
								if (Vector3.Distance(mouseWorldPosition, position) < currentHandleShape.GetUniformScale()*0.1f)
								{
									showRoundingSlider[j] = !showRoundingSlider[j];
									EditorUtility.SetDirty(target);
								}
							}
						}
						Color c = roundingHandleBorderColor;
						c.a = showRoundingSlider[j] ? 1 : 0.2f;
						Handles.color = c;
						Handles.DrawSolidDisc(position, currentHandleShape.transform.forward, Mathf.Min(currentHandleShape.ProportionsModifier.width, currentHandleShape.ProportionsModifier.height)*0.05f);
						c = roundingHandleFillColor;
						c.a = showRoundingSlider[j] ? 1 : 0.0f;
						Handles.color = c;
						Handles.DrawSolidDisc(position, currentHandleShape.transform.forward, Mathf.Min(currentHandleShape.ProportionsModifier.width, currentHandleShape.ProportionsModifier.height)*0.03f);

					}
				}
				if (GUI.changed && handleMovement.sqrMagnitude > 0.0f)
				{
					Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					plane = new Plane(currentHandleShape.transform.forward, currentHandleShape.transform.position);
					float enter = 100.0f;
					if (plane.Raycast(ray, out enter))
					{
						Vector3 pivotWorldPosition = ray.GetPoint(enter);
						Vector3 pivotLocalPosition = currentHandleShape.transform.InverseTransformPoint(pivotWorldPosition);

						Undo.RecordObjects(targets, "Pivot Modify");

						Vector2 pivotNormalized = PivotModifier.CalculatePivotNormalized(pivotLocalPosition,
							Helpers.CalculateBounds(currentHandleShape.LastData, currentHandleShape.HaveBorder()));

						for (int j = 0; j < targets.Length; j++)
						{
							QuickPolygon shape = (QuickPolygon) targets[j];
							shape.PivotModifier.Pivot = pivotNormalized;
							shape.PivotModifier.pivotSnapLocation = PivotSnapLocation.Custom;
						}

						// Try to snap the pivot if the user is holding control.
						if (Event.current.control)
						{
							SnapPivot(pivotSnapRange);
						}
						else
						{
							SnapPivot(0.0f);
						}
						RefreshTargets();

						EditorUtility.SetDirty(target);
					}
				}
			}
		}

		#if UNITY_5_4_OR_NEWER
		private void DrawPivotHandle(int controlID, Vector3 position, Quaternion rotation, float size, EventType eventType)
		#else
		private void DrawPivotHandle(int controlID, Vector3 position, Quaternion rotation, float size)
		#endif
		{
			QuickPolygon shape = null;
			for (int i = 0; i < targets.Length; i++)
			{
				shape = (QuickPolygon) targets[i];
			}
			if (shape.GetComponent<RectTransform> () == null)
			{
				Handles.color = pivotHandleBorderColor;
				Handles.DrawSolidDisc(currentHandleShape.transform.position, currentHandleShape.transform.forward, size);
				Handles.color = pivotHandleFillColor;
				Handles.DrawSolidDisc(currentHandleShape.transform.position, currentHandleShape.transform.forward, size * 0.8f);
			}
		}

		public void SnapPivot(float snapRadius)
		{
			// Find a location that is not futher away than the snap radius from the pivot point.
			foreach (PivotSnapLocation pivotSnapLocation in Enum.GetValues(typeof (PivotSnapLocation)))
			{
				if (pivotSnapLocation != PivotSnapLocation.Custom)
				{
					Vector2 snapLocation = PivotModifier.CalculatePivotSnapLocation(pivotSnapLocation);
					if (Vector2.Distance(currentHandleShape.PivotModifier.Pivot, snapLocation) <= snapRadius)
					{
						for (int i = 0; i < targets.Length; i++)
						{
							QuickPolygon shape = (QuickPolygon) targets[i];
							shape.PivotModifier.Pivot = snapLocation;
							shape.PivotModifier.pivotSnapLocation = pivotSnapLocation;
						}
					}
				}
			}
		}
		#endregion
	}

	public class AboutWindow : EditorWindow
	{
		private Texture2D Logo = null;

		private void OnEnable()
		{
			#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
			titleContent = new GUIContent("About");
			#else
			title = "About";
			#endif
			Logo = (Texture2D)Resources.Load("Images/Logo", typeof(Texture2D));
			position = new Rect(Screen.width / 2, Screen.height / 2, Logo.width, Logo.height + 5 + EditorGUIUtility.singleLineHeight * 4);
			minSize = new Vector2(Logo.width, Logo.height + 5 + EditorGUIUtility.singleLineHeight * 4);
			maxSize = minSize;

		}

		private void OnGUI()
		{
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
			GUI.DrawTexture(new Rect(0, 0, Logo.width, Logo.height), Logo);
			GUILayout.Space(Logo.height + 5);
			GUIStyle style = GUIStyle.none;
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = new Color32(86, 86 , 86 , 255);
			
			GUILayout.Label("Version: " + QuickPolygonEditor.version, style);
			GUILayout.Label("Copyright: Christopher McLaughlin 2017", style);
		}
	}
}