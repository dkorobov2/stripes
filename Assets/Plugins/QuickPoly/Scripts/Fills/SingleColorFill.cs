using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public class SingleColorFill : Fill
	{
		public Color color = Color.white;

        public override void ModifyData(MeshData data, Material material)
		{
            FillWithWhite(data);
            if (material.HasProperty("_GradientColorA"))
            {
                material.SetColor("_GradientColorA", color);
            }
            if (material.HasProperty("_GradientColorB"))
            {
                material.SetColor("_GradientColorB", color);
            }
		}
	}
}