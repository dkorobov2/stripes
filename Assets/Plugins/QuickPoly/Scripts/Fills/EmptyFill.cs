using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public class EmptyFill : Fill
	{
		public override void ModifyData(MeshData data, Material material)
		{
            FillWithWhite(data);
		}
	}
}