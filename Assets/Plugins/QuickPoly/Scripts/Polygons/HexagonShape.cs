using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class HexagonShape : CircleShape
    {
        public HexagonShape()
        {
            Name = "Hexagon";
			resolution = 6;
			roundings = new int[resolution];
			roundingsDistances = new float[resolution];
			maxRoudningDistance = MAX_ROUNDING_DISTANCE;
        }

        public override void SetShapeResolution(int value)
        {
            Debug.LogWarning(MSG.Warnings.SHAPEMESH_RESOLUTION_UNCHANGEABLE);
        }

        void OnEnable()
        {
            if (roundings == null || roundings.Length == 0)
            {
                roundings = new int[6];
            }
        }
    }
}