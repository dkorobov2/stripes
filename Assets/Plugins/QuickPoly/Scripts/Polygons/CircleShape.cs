using UnityEngine;
using System;

namespace QuickPoly
{
    [Serializable]
    public class CircleShape : Shape
    {
        private const int MIN_RESOLUTION = 3;
        private const int MAX_RESOLUTION = 100;
        [Range(MIN_RESOLUTION, MAX_RESOLUTION)]
        public int resolution = 12;

        protected const float MIN_ROUNDING_DISTANCE = 0;
        protected const float MAX_ROUNDING_DISTANCE = 0.45f;
		[Range(MIN_ROUNDING_DISTANCE, MAX_ROUNDING_DISTANCE)]
		public float[] roundingsDistances;

        public CircleShape()
        {
            Name = "Circle";
            roundings = new int[resolution];
			roundingsDistances = new float[resolution];
	        maxRoudningDistance = MAX_ROUNDING_DISTANCE;
        }

        protected virtual Vector3 MakeVertex(int n)
        {
            float directinMlt = facingDirection == 0 ? 1f : -1;
            float angle = n * 2f * Mathf.PI / resolution;
            return new Vector3(Mathf.Sin(directinMlt * angle) / 2f, Mathf.Cos(angle) / 2f);
        }

        public override MeshData GetData()
        {
			int seg = resolution;
            bool rounded = false;
	        
				uniform = true;
                for (int i = 0; i < roundings.Length; i++)
				{
					if (i >= roundingsDistances.Length)
					{
						float[] nRoundings = new float[roundings.Length];
						for (int j = 0; j < nRoundings.Length; j++)
						{
							if (j < roundingsDistances.Length)
							{
								nRoundings[j] = roundingsDistances[j];
							}
							else
							{
								nRoundings[j] = roundingsDistances[0];
							}

						}
						roundingsDistances = nRoundings;
					}
					if (i >= 0 && i < roundingsDistances.Length)
						roundings[i] = Mathf.RoundToInt(1 + (roundingsDistances[i] * 40));
					
                    if (roundings[i] > 1)
                    {
	                    uniform = false;
                        rounded = true;
                        resolution += 2;
                        // if a corner have rounding we add 2 vertexes that will be bounds for rounding function
                    }
                }
			
            if (roundings.Length != seg)
            {
                roundings = new int[seg];
            }
            Vector3[] vertices = new Vector3[resolution + 1];
            int[] indices = new int[resolution * 3];
            int[] borderVertices = new int[resolution];
            resolution = seg;
            int[] roundableVertices = new int[resolution];

            vertices[0] = Vector3.zero; //Center vertex

            int index = 0;
            int a = 0;
            int curCorner = 1;
            if (roundings[0] > 1)
            {
                curCorner = 2;
            }
            for (int i = 1; i < vertices.Length; i++)
            {
                if (i == curCorner)
                {
                    vertices[i] = MakeVertex(a);
                    roundableVertices[a] = i;
                    a++;
                    if (a < roundings.Length)
                    {
                        if (roundings[a - 1] > 1 && roundings[a] > 1)
                        {
                            curCorner += 3;
                        }
                        else if ((roundings[a - 1] == 1 && roundings[a] == 1) || !rounded)
                        {
                            curCorner += 1;
                        }
                        else
                        {
                            curCorner += 2;
                        }
                    }
                }
                else
                {
                    vertices[i] = MakeVertex(a);
                }
                borderVertices[i - 1] = i;
                indices[index++] = 0;
                indices[index++] = i;
                indices[index++] = i == vertices.Length - 1 ? 1 : i + 1;
            }
            if (rounded)
            {
                //Fixing position of rounding vertexes
                for (int i = 0; i < roundableVertices.Length; i++)
                {
	                if (roundings[i] > 1)
	                {
						int prevI = (roundableVertices[i] - 1 < 0)? roundableVertices.Length - 1 : (roundableVertices[i] - 1);
						int nextI = (roundableVertices[i] + 1 >= vertices.Length)? 0 :(roundableVertices[i] + 1);
		                int prevR = roundableVertices[i - 1 >= 0 ? i - 1 : roundableVertices.Length - 1];
		                int nextR = roundableVertices[i + 1 < roundableVertices.Length ? i + 1 : 0];

						vertices[prevI] = Vector3.Lerp(vertices[roundableVertices[i]], vertices[prevR],roundingsDistances[i]);
			                vertices[nextI] = Vector3.Lerp(vertices[roundableVertices[i]],vertices[nextR], roundingsDistances[i]);
					}
                }
            }
            MeshData data = new MeshData(vertices, indices, borderVertices, roundableVertices, 0);
            for (int i = 0; i < roundableVertices.Length; i++)
            {
                if (roundings[i] > 1)
                    data.Bevel(i, roundings[i], 1f);
            }

            return data;
        }

	    public override float[] GetRoundingResolutions()
	    {
		    return roundingsDistances;
	    }

	    public override float GetRoundingResolution(int id)
	    {
		    return roundingsDistances[id];
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

		public override int GetShapeResolution()
        {
            return resolution;
        }

	    public override float ClampRoundingDistance(float value)
	    {
		    return Mathf.Min(Mathf.Max(value,MIN_ROUNDING_DISTANCE), MAX_ROUNDING_DISTANCE);
	    }
    }
}