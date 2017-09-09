using UnityEngine;
using System.Collections.Generic;
using System;
using QuickPoly.MSG;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QuickPoly
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[Serializable]
	public class QuickPolygon : MonoBehaviour
	{
		#region Variables
		public MeshType ShapeMeshIndex;
		public FillType ShapeFillIndex = FillType.Unicolor;
		public ColliderType ShapeColliderIndex = ColliderType.None;
		
		public BorderFillType UseShapeBorder;
		
		public Shape Shape
		{
			get
			{
				switch ((int) ShapeMeshIndex)
				{
				case 0:
					return SquareShape;
				case 1:
					return CircleShape;
				case 2:
					return StarShape;
				case 3:
					return DiamondShape;
				case 4:
					return TriangleShape;
				case 5:
					return PentagonShape;
				case 6:
					return HexagonShape;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}
		
		public Fill Fill
		{
			get
			{
				switch ((int) ShapeFillIndex)
				{
				case 0:
					return EmptyFill;
				case 1:
					return SingleColorFill;
				case 2:
					return LinearGradientFill;
				case 3:
					return RadialGradientFill;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}
		
		public Border Border
		{
			get { return UseShapeBorder == BorderFillType.Unicolor ? StandardBorder : null; }
		}
		
		public MeshData LastData;
		public string CurrentPresetName = "";
		public bool AllowLoadPresetGeometry;

		public bool IsUI;
		
		public SquareShape SquareShape;
		public CircleShape CircleShape;
		public StarShape StarShape;
		public DiamondShape DiamondShape;
		public TriangleShape TriangleShape;
		public PentagonShape PentagonShape;
		public HexagonShape HexagonShape;
		
		public EmptyFill EmptyFill;
		public SingleColorFill SingleColorFill;
		public LinearGradientFill LinearGradientFill;
		public RadialGradientFill RadialGradientFill;
		
		public StandardBorder StandardBorder;
		
		public ProportionsModifier ProportionsModifier;
		public FacingModifier FacingModifier;
		public SortingLayerModifier SortingLayerModifier;
		public PivotModifier PivotModifier;
		
		private static HashSet<int> DuplicationCache = new HashSet<int>();
		
		public Color MainColor
		{
			get
			{
				Fill fill = Fill;
				if (fill == null || fill is EmptyFill)
				{
					return Color.white;
				}
				if (fill is SingleColorFill)
				{
					return ((SingleColorFill) fill).color;
				}
				if (fill is LinearGradientFill)
				{
					return ((LinearGradientFill) fill).colorA;
				}
				return ((RadialGradientFill) fill).colorA;
			}
		}
		
		public Bounds Bounds
		{
			get
			{
				return GetComponent<MeshFilter>().sharedMesh.bounds;
			}
		}
		
		public Vector2 Pivot
		{
			get
			{
				return PivotModifier.Pivot;
			}
			set
			{
				PivotModifier.Pivot = value;
			}
		}
		
		private Mesh mesh;
		public Mesh Mesh
		{
			get
			{
				if (mesh == null)
				{
					mesh = new Mesh();
				}
				return mesh;
			}
		}
		
		private Material material;
		public Material Material
		{
			get
			{
				if (material == null)
				{
					
					material = new Material(Resources.Load("QuickPolyDefault", typeof(Material)) as Material);
				}
				return material;
			}
		}
		
		public bool IsCircle()
		{
			if (Shape is HexagonShape || Shape is PentagonShape || Shape is SquareShape || Shape is StarShape ||
			    Shape is TriangleShape)
			{
				return false;
			}
			if (Shape is CircleShape) return true;
			else return false;
		}
		
		#endregion
		#region OnFunctions
		private void Awake()
		{
			var id = GetInstanceID();
			if (!DuplicationCache.Contains(id))
			{
				GetComponent<MeshFilter>().sharedMesh = Mesh;
				GetComponent<MeshRenderer>().sharedMaterial = Material;
				DuplicationCache.Add(id);
				RecalculateMesh();
			}
		}
		
		#region UI
		void Start() 
		{
			UpdateTransformType();
		}

		public void UpdateTransformType()
		{
			if (IsUI)
			{
				if (GetComponent<RectTransform>() == null)
				{
					ConvertTransformToRectTransform();
				}
			}
			else
			{
				if (GetComponent<RectTransform>() != null)
				{
					ConvertRectTransformToTransform();
				}
			}
		}

		private bool initializeRectTransform;
		public void ConvertTransformToRectTransform()
		{
			IsUI = true;
			initializeRectTransform = true;
			RectTransform tfm = gameObject.AddComponent<RectTransform>();
			if (tfm.parent != null && tfm.parent.GetComponent<RectTransform>() != null)
			{
			}
			else
			{
				Camera cam = Camera.main;
				if (cam == null)
				{
					cam = GameObject.FindObjectOfType<Camera>();
					if (cam == null) 
					{
					GameObject camObject = new GameObject("Camera");
					cam = camObject.AddComponent<Camera>();
					#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
					cam.gameObject.AddComponent<FlareLayer>();
					#else
					cam.gameObject.AddComponent("FlareLayer");
					#endif
					cam.gameObject.AddComponent<GUILayer>();
					cam.gameObject.AddComponent<AudioListener>();
					cam.orthographic = true;
					}
				}
				Canvas canvas = GameObject.FindObjectOfType<Canvas>();
				if (canvas == null)
				{
					GameObject canvasObject = new GameObject("Canvas");
					canvas = canvasObject.AddComponent<Canvas>();
					canvas.gameObject.AddComponent<CanvasScaler>();
					canvas.gameObject.AddComponent<GraphicRaycaster>();
					canvas.renderMode = RenderMode.ScreenSpaceCamera;
					canvas.planeDistance = 10;
				}
				canvas.worldCamera = cam;
				tfm.SetParent(canvas.transform);
				tfm.localScale = new Vector3(1, 1, 1);
			}
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Selection.activeObject = tfm.gameObject;
			}
			Component[] componentArray;
			componentArray = GetComponents(typeof(Component));
			for (int i = 0; i < componentArray.Length; i++) {
				UnityEditorInternal.ComponentUtility.MoveComponentUp(GetComponent<MeshFilter>());
				UnityEditorInternal.ComponentUtility.MoveComponentUp(GetComponent<MeshRenderer>());
			}
			#endif
		}

		public void ConvertRectTransformToTransform()
		{
			IsUI = false;
			QuickPolygon copy = Create("", ShapeMeshIndex, false);
			copy.transform.name = name;
			copy.transform.position = transform.position;
			copy.transform.rotation = transform.rotation;
			copy.transform.localScale = transform.localScale;
			List<Component> componentList = new List<Component>(GetComponents<Component>());
			for (int i = 0; i < componentList.Count; i++) {
				if (componentList[i].GetComponent<Transform>() && componentList[i].GetComponent<MeshFilter>() && componentList[i].GetComponent<MeshRenderer>() && componentList[i].GetComponent<QuickPolygon>()) {
					componentList.Remove(GetComponent<Transform>());
					componentList.Remove(GetComponent<MeshFilter>());
					componentList.Remove(GetComponent<MeshRenderer>());
					componentList.Remove(GetComponent<QuickPolygon>());
				}
				if (componentList.Count > 0)
				{
					copy.gameObject.AddComponent(componentList[0].GetType());
				}
			}
			SerializationClass.Deserialize(copy, SerializationClass.Serialize(this), true);
			Helpers.Destroy(this.gameObject);
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Selection.activeObject = copy.gameObject;
			}
			#endif
			RecalculateMesh();
		}

		void OnRectTransformDimensionsChange()
		{
			RectTransform tfm = GetComponent<RectTransform>();
			if (!initializeRectTransform) {
				ProportionsModifier.width = tfm.rect.width / 90.6f;
				ProportionsModifier.height = tfm.rect.height / 90.6f;
				RecalculateMesh();
			} 
			else 
			{
				initializeRectTransform = false;
			}
		}
		#endregion
		
		void OnValidate()
		{
			ProportionsModifier.OnValidate();
		}
		
		private void OnDestroy()
		{
			DuplicationCache.Remove(GetInstanceID());
			CleanupMemory();
		}
		
		private void CleanupMemory()
		{
			Helpers.Destroy(material, false);
			Helpers.Destroy(Mesh, false);
		}
		
		public void Reset()
		{
			GetComponent<MeshRenderer>().sharedMaterial = Material;
			GetComponent<MeshFilter>().sharedMesh = Mesh;
			
			SquareShape = new SquareShape();
			CircleShape = new CircleShape();
			StarShape = new StarShape();
			DiamondShape = new DiamondShape();
			TriangleShape = new TriangleShape();
			HexagonShape = new HexagonShape();
			PentagonShape = new PentagonShape();
			
			
			FacingModifier = new FacingModifier();
			StandardBorder = new StandardBorder();
			
			EmptyFill = new EmptyFill();
			SingleColorFill = new SingleColorFill();
			LinearGradientFill = new LinearGradientFill();
			RadialGradientFill = new RadialGradientFill();
			
			StandardBorder = new StandardBorder();
			
			ProportionsModifier = new ProportionsModifier();
			SortingLayerModifier = new SortingLayerModifier();
			PivotModifier = new PivotModifier();
		}
		#endregion

		public void RecalculateMesh()
		{
			if (Shape == null)
			{
				return;
			}
			
			Shape.facingDirection = 0;
			LastData = Shape.GetData();
			ProportionsModifier.ModifyData(LastData, GetComponent<RectTransform>());
			if (Fill != null)
			{
				Fill.ModifyData(LastData, Material);
			}
			else
			{
				EmptyFill.ModifyData(LastData, Material);
			}
			
			if (Border != null)
			{
				((StandardBorder) Border).KeepGeometry = ShapeFillIndex != FillType.None;
				Border.ModifyData(LastData, Material);
			}
			
			// Don't show the fill mesh if the fill is empty.
			if (ShapeFillIndex == FillType.None && Border == null)
			{
				LastData.Triangles = new int[0];
			}
			if (!IsUI)
			{
				PivotModifier.ModifyData(LastData, transform);
			}
			UVModifier.ModifyData(LastData);
			FacingModifier.ModifyData(LastData);
			SortingLayerModifier.ModifyData(LastData, GetComponent<MeshRenderer>());
			
			Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
			
			mesh.Clear();
			mesh.vertices = LastData.Vertices;
			mesh.triangles = LastData.Triangles;
			mesh.colors = LastData.Colors;
			mesh.uv = LastData.UV;
			mesh.name = Shape.Name;
			mesh.RecalculateNormals();
			
			if (ShapeColliderIndex != ColliderType.None && (Fill == null || Fill is EmptyFill) && Border == null)
			{
				Debug.LogWarning(Warnings.COLLIDER_POLYGON_FILL_AND_BORDER_NOT_EXIST, this);
				ShapeColliderIndex = ColliderType.None;
			}
			ColliderManager.ReloadCollider(this, ShapeColliderIndex, LastData, ((StandardBorder)Border));
			Lighting = lighting;
			if (GetComponent<RectTransform>() != null)
			{
				RectTransform tfm = GetComponent<RectTransform>();
				Vector3 size = new Vector3(ProportionsModifier.width, ProportionsModifier.height);
				tfm.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x * 90.6f);
				tfm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y * 90.6f);
			}
		}
		
		#region API
		#region Set
		#region ChangeShape
		public void SetIsUI(bool value, bool recalculate = false)
		{
			IsUI = value;
			if (recalculate)
			{
				UpdateTransformType();
			}
		}
		public void SetMeshType(int meshType, bool recalculate = false)
		{
			SetMeshType((MeshType)meshType, recalculate);
		}
		public void SetMeshType(MeshType meshType, bool recalculate = false)
		{
			ShapeMeshIndex = meshType;
			if (recalculate) { RecalculateMesh(); }
		}
		public void SetResolution(int value, bool recalculate = false)
		{
			Shape.SetShapeResolution(value);
			if (recalculate)
			{
				RecalculateMesh();
			}
		}

		public void SetResolution(int value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetResolution(value, true);
			}
			else
			{
				Animations.PlayChangeResolution(this, value, interpolateType, duration);
			}
		}
		
		public void SetUniformRoundingResolution(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetUniformRoundingResolution(value, true);
			}
			else
			{
				Animations.PlayChangeUniformRoundingResolution(this, Shape.ClampRoundingDistance(value), interpolateType, duration);
			}
		}
		
		public void SetUniformRoundingResolution(float value, bool recalculate = false)
		{
			for (int i = 0; i < Shape.GetShapeResolution(); i++)
			{
				Shape.SetRoundingResolution(i, value);
			}
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		
		public void SetRoundingResolution(int cornerID, float value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetRoundingResolution(cornerID, value, true);
			}
			else
			{
				Animations.PlayChangeRoundingResolutionOnCornerAnimation(this, cornerID, Shape.ClampRoundingDistance(value), interpolateType, duration);
			}
		}
		
		public void SetRoundingResolution(int cornerID, float value, bool recalculate = false)
		{
			Shape.SetRoundingResolution(cornerID, value);
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		
		public void SetStarRoundingDents(bool value, bool recalculate = false)
		{
			if (Shape is StarShape)
			{
				((StarShape)Shape).roundType = value;
				if (recalculate)
				{
					RecalculateMesh();
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_STAR_MESH);
			}
		}
		public void SetStarInnerRadius(float value, bool recalculate = false)
		{
			if (Shape is StarShape)
			{
				((StarShape)Shape).SetInnerRadius(value);
				if (recalculate)
				{
					RecalculateMesh();
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_STAR_MESH);
			}
		}
		public void SetStarInnerRadius(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (Shape is StarShape)
			{
				if (duration <= 0)
				{
					SetStarInnerRadius(value, true);
				}
				else
				{
					Animations.PlayChangeStarInnerRadius(this, value, interpolateType, duration);
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_STAR_MESH);
			}
		}
		
		public void SetDiamondTopWidth(float value, bool recalculate = false)
		{
			if (Shape is DiamondShape)
			{
				((DiamondShape)Shape).SetTopWidth(value);
				if (recalculate)
				{
					RecalculateMesh();
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
			}
		}
		
		public void SetDiamondTopWidth(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (Shape is DiamondShape)
			{
				if (duration <= 0)
				{
					SetDiamondTopWidth(value, true);
				}
				else
				{
					Animations.PlayChangeDiamondTopWidth(this, value, interpolateType, duration);
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
			}
		}
		public void SetDiamondMiddleHeight(float value, bool recalculate = false)
		{
			if (Shape is DiamondShape)
			{
				((DiamondShape)Shape).SetMiddleHeight(value);
				if (recalculate)
				{
					RecalculateMesh();
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
			}
		}
		
		public void SetDiamondMiddleHeight(float value,InterpolateType interpolateType, float duration = 0)
		{
			if (Shape is DiamondShape)
			{
				if (duration <= 0)
				{
					SetDiamondMiddleHeight(value, true);
				}
				else
				{
					Animations.PlayChangeDiamondMiddleHeight(this,value,interpolateType,duration);
				}
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
			}
		}
		
		#endregion
		#region ChangeFill
		public void SetFillEmpty(bool recalculate = false)
		{
			ShapeFillIndex = FillType.None;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetFillUnicolor(Color color, bool recalculate = false)
		{
			ShapeFillIndex = FillType.Unicolor;
			SingleColorFill.color = color;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetFillLinearGradient(Color colorA, Color colorB, float percentage = 0.5f, float angle = 0.0f, bool recalculate = false)
		{
			ShapeFillIndex = FillType.LinearGradient;
			LinearGradientFill.colorA = colorA;
			LinearGradientFill.colorB = colorB;
			LinearGradientFill.percentage = percentage;
			LinearGradientFill.angle = angle;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetFillRadialGradient(Color colorA, Color colorB, float percentage = 0.5f, bool recalculate = true)
		{
			ShapeFillIndex = FillType.RadialGradient;
			RadialGradientFill.colorA = colorA;
			RadialGradientFill.colorB = colorB;
			RadialGradientFill.percentage = percentage;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		#endregion
		#region ChangeBorder
		public void SetBorderEmpty(bool recalculate = false)
		{
			UseShapeBorder = BorderFillType.None; 
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetBorderUnicolor(Color color, float outerScale = 0.01f, float innerScale = 0f, bool recalculate = false)
		{
			StandardBorder = new StandardBorder();
			UseShapeBorder = BorderFillType.Unicolor;
			StandardBorder.color = color;
			StandardBorder.outerScale = Mathf.Max(0,Mathf.Min(4,outerScale));
			StandardBorder.innerScale = Mathf.Max(0, Mathf.Min(1, innerScale));
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetBorderScale(float outerScale, float innerScale, InterpolateType interpolateType, float duration = 0)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			if (duration <= 0)
			{
				SetBorderScale(outerScale, innerScale, true);
			}
			else
			{
				Animations.PlayChangeBorderSizeAnimation(this, outerScale, innerScale, interpolateType, duration);
			}
		}
		public void SetBorderScale(float outerScale, float innerScale, bool recalculate = false)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			StandardBorder.outerScale = Mathf.Max(0, outerScale);
			StandardBorder.innerScale = Mathf.Max(0, Mathf.Min(1, innerScale));
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetBorderOuterScale(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			if (duration <= 0)
			{
				SetBorderOuterScale(value, true);
			}
			else
			{
				Animations.PlayChangeOuterBorderAnimation(this, value, interpolateType, duration);
			}
		}
		public void SetBorderOuterScale(float value, bool recalculate = false)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST,this);
				return;
			}
			StandardBorder.outerScale = Mathf.Max(0,value);
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetBorderInnerScale(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			if (duration <= 0)
			{
				SetBorderInnerScale(value, true);
			}
			else
			{
				Animations.PlayChangeInnerBorderAnimation(this, value, interpolateType, duration);
			}
		}
		public void SetBorderInnerScale(float value, bool recalculate = false)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			StandardBorder.innerScale = Mathf.Max(0, Mathf.Min(1, value));
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetBorderColor(Color color, bool recalculate = false)
		{
			if (UseShapeBorder == BorderFillType.None)
			{
				Debug.LogWarning(Warnings.BORDER_NOT_EXIST, this);
				return;
			}
			StandardBorder.color = color;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		#endregion
		#region ChangeScale
		public void SetUniformScale(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetUniformScale(value, true);
			}
			else
			{
				Animations.PlayChangeUniformScaleAnimation(this, value, interpolateType, duration);
			}
		}
		public void SetUniformScale(float value, bool recalculate = false)
		{
			ProportionsModifier.isUniform = true;
			ProportionsModifier.width= value;
			ProportionsModifier.height = value;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetScale(float width, float height, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetScale(width,height,true);
			}
			else
			{
				Animations.PlayChangeScaleAnimation(this,width,height, interpolateType, duration);
			}
		}
		public void SetScale(float width, float height,bool recalculate = false)
		{
			if (width != height) ProportionsModifier.isUniform = false;
			ProportionsModifier.width = width;
			ProportionsModifier.height = height;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetWidth(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetWidth(value, true);
			}
			else
			{
				Animations.PlayChangeScaleAnimation(this, value, GetHeight(), interpolateType, duration);
			}
		}
		public void SetWidth(float value, bool recalculate = false)
		{
			ProportionsModifier.isUniform = false;
			ProportionsModifier.width = value;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetHeight(float value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetHeight(value, true);
			}
			else
			{
				Animations.PlayChangeScaleAnimation(this, GetHeight(), value, interpolateType, duration);
			}
		}
		public void SetHeight(float value, bool recalculate = false)
		{
			ProportionsModifier.isUniform = false;
			ProportionsModifier.height = value;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		#endregion
		#region Rest
		public LightingType lighting;
		public LightingType Lighting
		{
			get
			{
				return lighting;
			}
			set
			{
				lighting = value;
				switch (lighting)
				{
				case LightingType.Unity3D:
					Material.shader = Shader.Find("QuickPoly/Standard");
					break;
				case LightingType.None:
				default:
					Material.shader = Shader.Find("QuickPoly/Standard Unlit");
					break;
				}
				if (Material.HasProperty("_LightingEnabled"))
				{
					Material.SetFloat("_LightingEnabled", lighting == LightingType.Unity3D ? 1f : 0f);
				}
			}
		}

		#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
		public string SortingLayer
		{
			get
			{
				return SortingLayerModifier.sortingLayerName;
			}
			set
			{
				SortingLayerModifier.sortingLayerName = value;
				GetComponent<Renderer>().sortingLayerName = value;
			}
		}
		public void SetSortingLayer(string value)
		{
			SortingLayer = value;
		}
		#else
		public int SortingLayer
		{
			get
			{
				return SortingLayerModifier.sortingLayerID;
			}
			set
			{
				SortingLayerModifier.sortingLayerID = value;
				GetComponent<Renderer>().sortingLayerID = value;
			}
		}
		public void SetSortingLayer(int value)
		{
			SortingLayer = value;
		}
		#endif
		public int SortingOrder
		{
			get
			{
				return SortingLayerModifier.sortingOrder;
			}
			set
			{
				SortingLayerModifier.sortingOrder = value;
				GetComponent<Renderer>().sortingOrder = value;
			}
		}
		public void SetSortingOrder(int value)
		{
			SortingOrder = value;
		}
		public void SetLighting(LightingType value)
		{
			Lighting = value;
		}
		public void SetCollider(ColliderType colliderIndex, bool recalculate = false)
		{
			ShapeColliderIndex = colliderIndex;
			if (recalculate)
			{
				RecalculateMesh();
			}
		}
		public void SetPosition(Vector3 value)
		{
			this.transform.position = value;
		}
		public void SetPosition(Vector3 value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetPosition(value);
			}
			else
			{
				Animations.PlayChangePositionAnimation(this,value,interpolateType,duration);
			}
		}
		public void SetRotation(Vector3 value)
		{
			this.transform.rotation = Quaternion.Euler(value);
		}
		public void SetRotation(Quaternion value)
		{
			this.transform.rotation = value;
		}
		
		public void SetRotation(Vector3 value, InterpolateType interpolateType, float duration = 0)
		{
			if (duration <= 0)
			{
				SetRotation(value);
			}
			else
			{
				Animations.PlayChangeRotationAnimation(this, value, interpolateType, duration);
			}
		}
		
		public void SetPivot(Vector2 value)
		{
			PivotModifier.pivotSnapLocation = PivotSnapLocation.Custom;
			PivotModifier.Pivot = value;
			RecalculateMesh();
		}
		
		public void SetPivot(PivotSnapLocation value)
		{
			PivotModifier.pivotSnapLocation = value;
			RecalculateMesh();
		}
		public void SetFacingDirection(FacingDirection value)
		{
			FacingModifier.facingDirection = value;
			RecalculateMesh();
		}
		
		#endregion
		#endregion
		
		#region Get
		#region GetShape
		public bool GetIsUI()
		{
			return IsUI;
		}
		public MeshType GetMeshType()
		{
			return ShapeMeshIndex;
		}
		public int GetResolution()
		{
			return Shape.GetShapeResolution();
		}
		public float GetRoundingResolution(int cornerID)
		{
			return Shape.GetRoundingResolution(cornerID);
		}
		public float GetUniformRoundingResolution()
		{
			return GetRoundingResolution(0);
		}
		public bool GetStarRoundingDents()
		{
			if (Shape is StarShape)
			{
				return ((StarShape)Shape).roundType;
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_STAR_MESH);
				return false;
			}
		}
		
		public float GetStarInnerRadius()
		{
			if (Shape is StarShape)
			{
				return ((StarShape)Shape).GetInnerRadius();
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_STAR_MESH);
				return 0;
			}
		}
		
		public float GetDiamondTopWidth()
		{
			if (Shape is DiamondShape)
			{
				return ((DiamondShape)Shape).GetDiamondTopWidth();
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
				return 0;
			}
		}
		public float GetDiamondMiddleHeight()
		{
			if (Shape is DiamondShape)
			{
				return ((DiamondShape)Shape).GetMiddleHeight();
			}
			else
			{
				Debug.LogWarning(Warnings.SHAPEMESH_IS_NOT_DIAMOND_MESH);
				return 0;
			}
		}
		#endregion
		#region GetFill
		
		public FillType GetFillType()
		{
			return ShapeFillIndex;
		}
		public Color GetFillColorA()
		{
			switch (ShapeFillIndex)
			{
			case FillType.None:
				return new Color();
			case FillType.LinearGradient:
				return LinearGradientFill.colorA;
			case FillType.RadialGradient:
				return RadialGradientFill.colorA;
			case FillType.Unicolor:
				return SingleColorFill.color;
			default:
				return new Color();
			}
		}
		public Color GetFillColorB()
		{
			switch (ShapeFillIndex)
			{
			case FillType.None:
				return new Color();
			case FillType.LinearGradient:
				return LinearGradientFill.colorB;
			case FillType.RadialGradient:
				return RadialGradientFill.colorB;
			case FillType.Unicolor:
				return SingleColorFill.color;
			default:
				return new Color();
			}
		}
		public float GetFillPercentage()
		{
			switch (ShapeFillIndex)
			{
			case FillType.None:
				return 0;
			case FillType.LinearGradient:
				return LinearGradientFill.percentage;
			case FillType.RadialGradient:
				return RadialGradientFill.percentage;
			case FillType.Unicolor:
				return 1;
			default:
				return 0;
			}
		}
		public float GetFillAngle()
		{
			switch (ShapeFillIndex)
			{
			case FillType.None:
				return 0;
			case FillType.LinearGradient:
				return LinearGradientFill.angle;
			case FillType.RadialGradient:
				return 0;
			case FillType.Unicolor:
				return 0;
			default:
				return 0;
			}
		}
		#endregion
		#region GetBorder
		public BorderFillType GetBorderType() { return UseShapeBorder;}
		
		public bool HaveBorder()
		{
			return GetBorderType() != BorderFillType.None;
		}
		public float GetBorderOuterScale()
		{
			if(Border != null)
				return StandardBorder.outerScale;
			return 0;
		}
		public float GetBorderInnerScale()
		{
			if (Border != null)
				return StandardBorder.innerScale;
			return 0;
		}
		
		public Color GetBorderColor() { return GetBorderType() == BorderFillType.Unicolor ? StandardBorder.color : new Color(); }
		#endregion
		#region GetScale
		public float GetUniformScale()
		{
			return ProportionsModifier.UniformScale; 
		}
		public float GetWidth()
		{
			return ProportionsModifier.width;
		}
		public float GetHeight()
		{
			return ProportionsModifier.height; 
		}
		#endregion
		#region Rest
		#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
		public string GetSortingLayer()
		{
			return SortingLayer;
		}
		#else
		public int GetSortingLayer()
		{
			return SortingLayer;
		}
		#endif
		public int GetSortingOrder()
		{
			return SortingOrder;
		}
		public LightingType GetLighting()
		{
			return Lighting;
		}
		public ColliderType GetColliderType()
		{
			return ShapeColliderIndex;
		}
		public Vector3 GetCenter()
		{
			return transform.position - ToPivotVector();
		}
		
		public Vector3 ToPivotVector()
		{
			return PivotModifier.ToPivotVector(this.transform, PivotModifier.GetPositionDelta());
		}
		public Vector3 GetPosition()
		{
			return transform.position;
		}
		public Vector3 GetRotation()
		{
			return transform.rotation.eulerAngles;
		}
		public Vector2 GetPivot()
		{
			return PivotModifier.Pivot;
		}
		public PivotSnapLocation GetPivotSnapLocation()
		{
			return PivotModifier.pivotSnapLocation;
		}
		public FacingDirection GetFacingDirection()
		{
			return FacingModifier.facingDirection;
		}
		#endregion
		#endregion
		
		#region CreateShape
		public static QuickPolygon CreateFromMostUsedPreset(string name, MeshType meshIndex, bool isUI)
		{
			return CreateFromMostUsedPreset(name, meshIndex, Vector3.zero, Quaternion.identity, isUI);
		}
		
		public static QuickPolygon CreateFromMostUsedPreset(string name, MeshType meshIndex, Vector3 position, Vector3 rotation, bool isUI)
		{
			return CreateFromMostUsedPreset(name, meshIndex, position, Quaternion.Euler(rotation), isUI);
		}
		public static QuickPolygon CreateFromMostUsedPreset(string name, MeshType meshIndex, Vector3 position, Quaternion rotation, bool isUI)
		{
			PresetsRecord mostUsedPreset = PresetsManager.GetMostUsedPresetRecord();
			QuickPolygon mesh = Create(name, meshIndex, position, rotation, isUI);
			if (mostUsedPreset != null && PresetsManager.IsPresetExists(mostUsedPreset))
			{
				PresetsManager.LoadPresetFor(mostUsedPreset, mesh);
			}
			else
			{
				Debug.LogWarning(MSG.Warnings.PRESET_MOST_USED_NOT_FOUND);
			}
			mesh.RecalculateMesh();
			return mesh;
		}
		public static QuickPolygon CreateFromLastPreset(string name, MeshType meshIndex, bool isUI)
		{
			return CreateFromLastPreset(name, meshIndex, Vector3.zero, Quaternion.identity, isUI);
		}
		
		public static QuickPolygon CreateFromLastPreset(string name, MeshType meshIndex, Vector3 position, Vector3 rotation, bool isUI)
		{
			return CreateFromLastPreset(name, meshIndex, position, Quaternion.Euler(rotation), isUI);
			
		}
		public static QuickPolygon CreateFromLastPreset(string name, MeshType meshIndex, Vector3 position, Quaternion rotation, bool isUI)
		{
			PresetsRecord lastPreset = PresetsManager.GetLastUsedPreset();
			QuickPolygon mesh = Create(name, meshIndex, position, rotation, isUI);
			if (lastPreset != null && PresetsManager.IsPresetExists(lastPreset))
			{
				PresetsManager.LoadPresetFor(lastPreset, mesh);
			}
			else
			{
				Debug.LogWarning(MSG.Warnings.PRESET_LAST_NOT_FOUND);
			}
			mesh.RecalculateMesh();
			return mesh;
		}
		public static QuickPolygon Create(string name, MeshType meshIndex, bool isUI)
		{
			return Create(name, meshIndex, Vector3.zero, Quaternion.identity, isUI);
		}
		
		public static QuickPolygon Create(string name, MeshType meshIndex, Vector3 position, Vector3 rotation, bool isUI)
		{
			return Create(name, meshIndex, position, Quaternion.Euler(rotation), isUI);
		}
		public static QuickPolygon Create(string name, MeshType meshIndex, Vector3 position, Quaternion rotation, bool isUI)
		{
			GameObject polygon = new GameObject();
			if (!string.IsNullOrEmpty(name))
			{
				polygon.name = name;
			}
			else
			{
				polygon.name = meshIndex.ToString();
			}
			polygon.transform.position = position;
			polygon.transform.rotation = rotation;
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				Undo.RegisterCreatedObjectUndo(polygon, "Shape Created");
				Selection.activeObject = polygon;
			}
			#endif
			QuickPolygon newPolygon = polygon.AddComponent<QuickPolygon>();
			newPolygon.ShapeMeshIndex = meshIndex;
			newPolygon.IsUI = isUI;
			newPolygon.Reset();
			newPolygon.RecalculateMesh();
			return newPolygon;
		}
		#endregion
		
		#region Presets
		public static QuickPolygon LoadPreset(QuickPolygon polygon, string name, bool loadMesh = false, bool recalculate = true)
		{
			if (PresetsManager.IsPresetExists(name))
			{
				PresetsManager.LoadPresetFor(name, polygon, loadMesh);
			}
			else
			{
				Debug.LogWarning(MSG.Warnings.PRESET_NOT_FOUND);
			}
			return polygon;
		}
		public static void SavePreset(QuickPolygon polygon, string name)
		{
			polygon.SaveAsPreset(name);
		}
		public void SaveAsPreset()
		{
			if (this.CurrentPresetName == "" || this.CurrentPresetName == "None")
			{
				Debug.LogWarning(Warnings.PRESET_SAVE_EMPTY_NAME);
				return;
			}
			SaveAsPreset(this.CurrentPresetName);
		}
		public void SaveAsPreset(string name)
		{
			CurrentPresetName = name;
			PresetsManager.SavePreset(CurrentPresetName, this);
		}
		#endregion
		#endregion
		
	}
	#region Enums
	public enum MeshType
	{
		Square = 0,
		Circle = 1,
		Star = 2,
		Diamond = 3,
		Triangle = 4,
		Pentagon = 5,
		Hexagon = 6
	}
	
	public enum FillType
	{
		None = 0,
		Unicolor,
		LinearGradient,
		RadialGradient,
	}
	
	public enum BorderFillType : int
	{
		None = 0,
		Unicolor = 1
	}
	
	public enum ColliderType : int
	{
		None = 0,
		Box = 1,
		FillPolygon = 2,
		BorderPolygon = 3,
		FillAndBorderPolygon = 4
	}
	
	public enum LightingType
	{
		None,
		Unity3D
	}
	public enum InterpolateType
	{
		Linear,
		Cosinus,
		EaseInOutQuint,
		EaseOutBack,
		EaseOutElastic
	}
	
	public enum FacingDirection
	{
		Forward = 0,
		Backward,
		DoubleSided
	}
	#endregion
}