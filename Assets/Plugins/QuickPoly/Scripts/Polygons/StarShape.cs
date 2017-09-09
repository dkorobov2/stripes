using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class StarShape : Shape
    {
        private const int MIN_RESOLUTION = 4;
        private const int MAX_RESOLUTION = 30;
        [Range(MIN_RESOLUTION, MAX_RESOLUTION)]
        public int resolution = 5;

        private const float MIN_INNER_RADIUS = 0.1f;
        private const float MAX_INNER_RADIUS = 1.0f;
        [Range(MIN_INNER_RADIUS, MAX_INNER_RADIUS)]
        public float innerRadius = 0.5f;

        public int innerRounding = 1;

        private const float MIN_ROUNDING_DISTANCE = 0f;
        private const float MAX_ROUNDING_DISTANCE = 0.99f;

		[Range(MIN_ROUNDING_DISTANCE, MAX_ROUNDING_DISTANCE)]
		public float[] roundingsDistances;
        public bool roundType;

        public StarShape()
        {
            Name = "Star";
            roundings = new int[resolution];
			roundingsDistances = new float[resolution];
			maxRoudningDistance = MAX_ROUNDING_DISTANCE;
		}

        private void OnEnable()
		{
			if (roundings == null || roundings.Length == 0)
			{
				roundings = new int[resolution];
			}
			if (roundingsDistances == null || roundingsDistances.Length == 0)
			{
				roundingsDistances = new float[resolution];
			}
        }

        private Vector3 MakeVertex(int n)
        {
            float directionMlt = facingDirection == 0 ? 1f : -1f;

            bool shifted = n % 2 == 1;

            float addition = 2f * Mathf.PI / resolution;
            float angle = (n / 2) * 1f * addition;
            if (shifted)
            {
                angle += addition / 2;
            }

            float x = Mathf.Sin(angle * directionMlt);
            float y = Mathf.Cos(angle);

            if (shifted)
            {
                return new Vector3(x / 2 * innerRadius, y / 2 * innerRadius, 0);
            }
            else
            {
                return new Vector3(x / 2, y / 2, 0);
            }
        }

        public override MeshData GetData()
        {
            if (roundings.Length != resolution)
            {
                int[] arr = new int[resolution];
                for (int i = 0; i < resolution && i < roundings.Length; i++)
                {
                    arr[i] = roundings[i];
                }
                roundings = arr;
            }

			if (roundingsDistances.Length != resolution)
			{
				float[] arr = new float[resolution];
				for (int i = 0; i < resolution && i < roundingsDistances.Length; i++)
				{
					arr[i] = roundingsDistances[i];
				}
				roundingsDistances = arr;
			}
            bool rounded = false;
	        uniform = true;
            // This is why using rounding for at least one corner adds redundant vertices.
            int segments = resolution * 2;
                for (int i = 0; i < roundings.Length; i++)
				{
					roundings[i] = Mathf.RoundToInt(1 + (roundingsDistances[i] * 20));
                    if (roundings[i] > 1)
                    {
	                    uniform = false;
                        rounded = true;
                        segments += 2;
                    }
                }

            Vector3[] vertices = new Vector3[segments + 1];
            int[] indices = new int[segments * 3];
            int[] borderVertices = new int[segments];
            int[] roundableVertices = new int[resolution];

            vertices[0] = Vector3.zero;

            int index = 0;
            int a = 0;
            int furtherCorner = 1;
            int closerCorner = furtherCorner + 1;
            if (roundings[0] > 1)
            {
                if (roundType)
                {
                    furtherCorner = 1;
                }
                else
                {
                    furtherCorner = 2;
                }
                closerCorner = furtherCorner + 2;
            }
            for (int i = 1; i < vertices.Length; i++)
            {
                if (i == closerCorner)
                {
                    vertices[i] = MakeVertex(2 * a - 1);
                }
                else if (i == furtherCorner && a < roundings.Length)
                {
                    vertices[i] = MakeVertex(2 * a);

                    closerCorner = i + (roundings[a] > 1 ? 2 : 1);
                    if (roundType)
                    {
                        roundableVertices[a] = closerCorner;
                    }
                    else
                    {
                        roundableVertices[a] = i;
                    }

                    a++;
                    int nex = a < roundings.Length ? a : 0;

                    if (roundings[a - 1] > 1 && roundings[nex] > 1)
                    {
                        furtherCorner += 4;
                    }
                    else if (roundings[a - 1] == 1 && roundings[nex] == 1)
                    {
                        furtherCorner += 2;
                    }
                    else
                    {
                        if (roundType)
                        {
                            furtherCorner += roundings[a - 1] > 1 ? 4 : 2;
                        }
                        else
                        {
                            furtherCorner += 3;
                        }
                    }
                }
                else
                {
                    vertices[i] = Vector3.zero; // will be overwiten below, based on previous and next vertex position
                }
                borderVertices[i - 1] = i;

                indices[index++] = 0;
                indices[index++] = i;
                indices[index++] = i == vertices.Length - 1 ? 1 : i + 1;
            }
            if (rounded)
            {
                for (int i = 0; i < roundableVertices.Length; i++)
                {

                    if (roundings[i] > 1)
                    {
                        int tipID = roundableVertices[i];
                        int prevI = tipID == 1 ? vertices.Length - 1 : tipID - 1;
                        int nextI = tipID + 1;

                        int prevR = tipID == 1 ? vertices.Length - 2 : tipID == 2 ? vertices.Length - 1 : tipID - 2;
                        int nextR = tipID == vertices.Length - 2 ? 1 : tipID + 2;

						vertices[prevI] = Vector3.Lerp(vertices[prevR], vertices[tipID], 1-roundingsDistances[i]);
						vertices[nextI] = Vector3.Lerp(vertices[nextR], vertices[tipID], 1 - roundingsDistances[i]);
                    }
                }
            }

            var data = new MeshData(vertices, indices, borderVertices, roundableVertices, 0);
            if (rounded)
            {
                for (int i = 0; i < resolution; i++)
                {
                    if (roundings[i] > 1)
                        data.Bevel(i, roundings[i], 1);
                }
            }
            return data;
        }


		public override void SetShapeResolution(int value)
        {
            if (value < MIN_RESOLUTION)
            {
                resolution = MIN_RESOLUTION;
            }
            else if (value > MAX_RESOLUTION)
            {
                resolution = MAX_RESOLUTION;
            }
            else
            {
                resolution = value;
            }
        }

		public void SetInnerRadius(float value)
		{
			innerRadius = Mathf.Max(MIN_INNER_RADIUS, Mathf.Min(MAX_INNER_RADIUS, value));
		}

		public override void SetRoundingResolution(int id, float value)
		{
			if (value <= MIN_ROUNDING_DISTANCE)
			{
				roundingsDistances[id] = MIN_ROUNDING_DISTANCE;
			}
			else if (value > MAX_ROUNDING_DISTANCE)
			{
				roundingsDistances[id] = MAX_ROUNDING_DISTANCE;
			}
			else
			{
				roundingsDistances[id] = value;
			}
		}

		public float GetInnerRadius()
		{
			return innerRadius;
		}

		public override int GetShapeResolution()
		{
			return resolution;
		}
		
		public override float GetRoundingResolution(int id)
		{
			return roundingsDistances[id];
		}

	    public override float[] GetRoundingResolutions()
	    {
		    return roundingsDistances;
	    }
		public override float ClampRoundingDistance(float value)
		{
			return Mathf.Min(Mathf.Max(value, MIN_ROUNDING_DISTANCE), MAX_ROUNDING_DISTANCE);
		}
    }
}