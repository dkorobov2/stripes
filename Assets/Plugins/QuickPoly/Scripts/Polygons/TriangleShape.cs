using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class TriangleShape : CircleShape
    {
        public TriangleShape()
        {
            Name = "Triangle";
            resolution = 3;
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
                roundings = new int[3];
            }
        }
    }
}