using UnityEngine;
using System.Collections;
using System;

namespace QuickPoly 
{
	[Serializable]
	public abstract class Shape
	{
		public abstract MeshData GetData();

        public const int MIN_ROUNDING_VALUE = 1;
		public const int MAX_ROUNDING_VALUE = 50;
		[Range(MIN_ROUNDING_VALUE, MAX_ROUNDING_VALUE)]
		public int[] roundings;

		public bool uniform;
        public string Name = "Shape";
		public float maxRoudningDistance = 1;

		public int facingDirection;

		public abstract void SetShapeResolution(int value);
		public abstract int GetShapeResolution();
		public abstract void SetRoundingResolution(int id,float value);
		public abstract float ClampRoundingDistance(float value);
		public abstract float GetRoundingResolution(int id);
		public abstract float[] GetRoundingResolutions();
	}
}