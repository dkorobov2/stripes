using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class SquareShape : CircleShape
    {
        public SquareShape()
        {
            Name = "Square";
			resolution = 4;
			roundings = new int[resolution];
			roundingsDistances = new float[resolution];
			maxRoudningDistance = MAX_ROUNDING_DISTANCE;
        }

        public override void SetShapeResolution(int value)
        {
            Debug.LogWarning(MSG.Warnings.SHAPEMESH_RESOLUTION_UNCHANGEABLE);
        }

        private void OnEnable()
		{
			if (roundings == null || roundings.Length == 0)
			{
				roundings = new int[4];
			}
			if (roundingsDistances == null || roundingsDistances.Length == 0 || roundingsDistances.Length> 4)
			{
				roundingsDistances = new float[4];
			}
        }

        protected override Vector3 MakeVertex(int n)
        {
            float directinMlt = facingDirection == 0 ? 1f : -1;
            float angle = n * 2f * Mathf.PI / resolution + Mathf.PI / 4f;
            float s = Mathf.Sqrt(2f);
            return new Vector3(Mathf.Sin(directinMlt * angle) / s, Mathf.Cos(angle) / s);
        }
    }
}