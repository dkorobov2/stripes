using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class PentagonShape : CircleShape
    {
        public PentagonShape()
        {
            Name = "Pentagon";
			resolution = 5;
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
				roundings = new int[5];
			}
			if (roundingsDistances == null || roundingsDistances.Length == 0)
			{
				roundingsDistances = new float[5];
			}
        }
    }
}