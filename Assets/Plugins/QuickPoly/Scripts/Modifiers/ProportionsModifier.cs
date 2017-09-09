using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public class ProportionsModifier
	{
		public float width = 1;
		public float height = 1;
		public bool isUniform = false;
		private Vector3 scale;

		public float UniformScale
		{
			get { return (width + height)/2; }
		}

		public void ModifyData(MeshData data, RectTransform rectTransform)
		{
			if (rectTransform != null) 
			{
				scale = new Vector3(width * 90.6f, height * 90.6f);
			}
			else
			{
				scale = new Vector3(width, height);
			}

			for (int i = 0; i < data.Vertices.Length; i++)
			{
				data.Vertices[i].Scale(scale);
			}
		}

        public void OnValidate()
        {
            width = Mathf.Max(0.001f, width);
            height = Mathf.Max(0.001f, height);
        }
	}
}