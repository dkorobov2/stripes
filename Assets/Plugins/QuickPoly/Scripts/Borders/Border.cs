using UnityEngine;
using System.Collections;
using System;

namespace QuickPoly
{
	[Serializable]
	public abstract class Border
	{
        public abstract void ModifyData(MeshData data, Material material);
	}
}