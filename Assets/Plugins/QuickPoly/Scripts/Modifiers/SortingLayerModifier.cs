using UnityEngine;

namespace QuickPoly
{
	[System.Serializable]
	public class SortingLayerModifier
	{
		public string sortingLayerName;
		public int sortingLayerID;
		public int sortingOrder;
		
		public void ModifyData(MeshData data, Renderer renderer)
		{
			#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
			renderer.sortingLayerName = sortingLayerName;
			#else
			renderer.sortingLayerID = sortingLayerID;
			#endif
			renderer.sortingOrder = sortingOrder;
		}
	}
}