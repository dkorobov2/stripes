using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public abstract class Fill
	{
        public abstract void ModifyData(MeshData data, Material material);
        protected void FillWithWhite(MeshData data)
        {
            data.Colors = new Color[data.Vertices.Length];
            for (int i = 0; i < data.Colors.Length; i++)
            {
                data.Colors[i] = Color.white;
            }
        }
	}
}