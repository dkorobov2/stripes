using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class PivotModifier
    {
        // Normalized pivot position.
        public float x = 0.5f;
        public float y = 0.5f;

        // Snap location.
        public PivotSnapLocation pivotSnapLocation = PivotSnapLocation.MiddleCenter;

        // Transform position delta for the current pivot position. Should be (0, 0) when the pivot is (0.5, 0.5).
        // Position delta serialized is only used during scene load to remember the delta between sessions.
        [SerializeField]
        public Vector2 positionDeltaSerialized;
        private Vector2? positionDeltaRuntime;

        // Constant vector to convert pivot from [0, 1] to [-0.5, 0.5]
        private static readonly Vector2 conversionVector = new Vector2(-0.5f, -0.5f);

        // Normalized pivot in vector.
        public Vector2 Pivot
        {
            get { return new Vector2(x, y); }
            set
            {
				x = value.x;
				y = value.y;
            }
        }

        public void ModifyData(MeshData data, Transform transform)
        {
            Vector2 positionDelta = GetPositionDelta();
            if (pivotSnapLocation != PivotSnapLocation.Custom)
            {
                Pivot = CalculatePivotSnapLocation(pivotSnapLocation);
            }

            // Reset transform's position.
			transform.position -= ToPivotVector(transform, positionDelta);

            Bounds bounds = Helpers.CalculateBounds(data, true);
            Vector3 displacement = Vector2.Scale(Pivot + conversionVector, bounds.size);

            // Displace mesh vertices.
            for (int i = 0; i < data.Vertices.Length; i++)
            {
                data.Vertices[i] -= displacement;
            }

			transform.position += transform.right * displacement.x * transform.localScale.x + transform.up * displacement.y * transform.localScale.y;
			
			positionDeltaSerialized = displacement;
            positionDeltaRuntime = positionDeltaSerialized;
        }

	    public Vector2 GetPositionDelta()
	    {
		    return positionDeltaRuntime.HasValue ? positionDeltaRuntime.Value : positionDeltaSerialized;
	    }

	    public static Vector3 ToPivotVector(Transform transform, Vector2 positionDelta)
	    {
		    return transform.right * positionDelta.x * transform.localScale.x + transform.up * positionDelta.y * transform.localScale.y;
	    }

	    public static Vector2 CalculatePivotSnapLocation(PivotSnapLocation pivotSnapLocation)
        {
            switch (pivotSnapLocation)
            {
                case PivotSnapLocation.UpperLeft: return new Vector2(0.0f, 1.0f);
                case PivotSnapLocation.MiddleLeft: return new Vector2(0.0f, 0.5f);
                case PivotSnapLocation.LowerLeft: return new Vector2(0.0f, 0.0f);
                case PivotSnapLocation.UpperCenter: return new Vector2(0.5f, 1.0f);
                case PivotSnapLocation.LowerCenter: return new Vector2(0.5f, 0.0f);
                case PivotSnapLocation.UpperRight: return new Vector2(1.0f, 1.0f);
                case PivotSnapLocation.MiddleRight: return new Vector2(1.0f, 0.5f);
                case PivotSnapLocation.LowerRight: return new Vector2(1.0f, 0.0f);
                default: return new Vector2(0.5f, 0.5f);
            }
        }

        public static Vector2[] CalculateAllPivotSnapLocations()
        {
            return new Vector2[] 
            {
                CalculatePivotSnapLocation(PivotSnapLocation.UpperLeft),
                CalculatePivotSnapLocation(PivotSnapLocation.MiddleLeft),
                CalculatePivotSnapLocation(PivotSnapLocation.LowerLeft),
                CalculatePivotSnapLocation(PivotSnapLocation.UpperCenter),
                CalculatePivotSnapLocation(PivotSnapLocation.MiddleCenter),
                CalculatePivotSnapLocation(PivotSnapLocation.LowerCenter),
                CalculatePivotSnapLocation(PivotSnapLocation.UpperRight),
                CalculatePivotSnapLocation(PivotSnapLocation.MiddleRight),
                CalculatePivotSnapLocation(PivotSnapLocation.LowerRight)
            };
        }

        public static Vector2 CalculatePivotNormalized(Vector2 pivot, Bounds bounds)
        {
            Vector2 pivotNormalized = pivot - (Vector2)bounds.center;
            pivotNormalized.x = pivotNormalized.x / bounds.size.x;
            pivotNormalized.y = pivotNormalized.y / bounds.size.y;
            pivotNormalized -= conversionVector;
            return pivotNormalized;
        }
    }

    public enum PivotSnapLocation
    {
        UpperLeft,
        MiddleLeft,
        LowerLeft,
        UpperCenter,
        MiddleCenter,
        LowerCenter,
        UpperRight,
        MiddleRight,
        LowerRight,
        Custom
    }
}