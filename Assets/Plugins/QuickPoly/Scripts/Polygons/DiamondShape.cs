using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public class DiamondShape : Shape
	{
		private const float MIN_TOP_WIDTH = 0.1f;
		private const float MAX_TOP_WIDTH = 0.90f;
		[Range(MIN_TOP_WIDTH, MAX_TOP_WIDTH)]
		public float topWidth = 0.75f;
		private const float MIN_MIDDLE_HEIGHT = 0f;
		private const float MAX_MIDDLE_HEIGHT = 1.0f;
		[Range(MIN_MIDDLE_HEIGHT, MAX_MIDDLE_HEIGHT)]
		public float middleHeight = 0.7f;
		
		private const float MIN_ROUNDING_DISTANCE = 0.01f;
		private const float MAX_ROUNDING_DISTANCE = 0.40f;
		
		[Range(MIN_ROUNDING_DISTANCE, MAX_ROUNDING_DISTANCE)]
		public float[] roundingsDistances;
		
		public DiamondShape()
		{
			Name = "Diamond";
			roundings = new int[5];
			roundingsDistances = new float[5];
			maxRoudningDistance = MAX_ROUNDING_DISTANCE;
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
		
		public override MeshData GetData()
		{
			if (roundings.Length != 5)
			{
				roundings = new int[5];
			}
			if (roundingsDistances.Length != 5)
			{
				roundingsDistances = new float[5];
			}
			uniform = true;
			bool rounded = false;
			float directionMlt = facingDirection == 0 ? 1f : -1f;
			float row = 0.5f * (2*middleHeight-1f);
			float hatX = topWidth * 0.5f * directionMlt;
			float shoulderX = 0.5f * directionMlt;
			
			Vector3[] vertices = new[]
			{
				new Vector3(0, 0, 0),
				new Vector3(hatX, 0.5f, 0),
				new Vector3(-hatX, 0.5f, 0),
				new Vector3(-shoulderX, row, 0),
				new Vector3(0, -0.5f, 0),
				new Vector3(shoulderX, row, 0),
			};
			
			int[] indices = new[]
			{
				5, 4, 0,
				4, 3, 0,
				3, 2, 0,
				2, 1, 0,
				1, 5, 0,
			};
			int[] borderVertices = new[] { 5, 4, 3, 2, 1 };
			int[] roundableVertices = new int[] { 1, 2, 3, 4, 5 };
			for (int i = 0; i < roundableVertices.Length; i++)
			{
				float m1, m2;
				if (i == 0)
				{
					m1 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[roundableVertices.Length - 1]]);
					m2 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[i + 1]]);
				} else if (i == roundableVertices.Length - 1)
				{
					m1 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[0]]);
					m2 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[i-1]]);
				}
				else
				{
					m1 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[i - 1]]);
					m2 = Vector3.Distance(vertices[roundableVertices[i]], vertices[roundableVertices[i + 1]]);
				}
				roundingsDistances[i] = Mathf.Min(roundingsDistances[i], Mathf.Min(m1, m2)/2.2f);
				
				roundings[i] = Mathf.RoundToInt(1 + (roundingsDistances[i] * 40));
			}
			MeshData data = new MeshData(vertices, indices, borderVertices, roundableVertices, 0);
			for (int i = 0; i < roundings.Length; i++)
			{
				if (roundings[i] > 1)
				{
					rounded = true;
					uniform = false;
				}
			}
			if (rounded)
			{
				for (int i = 0; i < roundableVertices.Length; i++)
				{
					if (roundings[i] > 1)
					{
						data.PrepareBevel(i, roundingsDistances[i]);
					}
				}
				for (int i = 0; i < roundableVertices.Length; i++)
				{
					if (roundings[i] > 1)
					{
						data.Bevel(i, roundings[i], 1);
					}
				}
			}
			return data;
		}
		
		public float GetDiamondTopWidth()
		{
			return topWidth;
		}
		public float GetMiddleHeight()
		{
			return middleHeight;
		}
		
		public void SetTopWidth(float value)
		{
			topWidth = Mathf.Max(MIN_TOP_WIDTH, Mathf.Min(MAX_TOP_WIDTH, value));
		}
		public void SetMiddleHeight(float value)
		{
			middleHeight = Mathf.Max(MIN_MIDDLE_HEIGHT, Mathf.Min(MAX_MIDDLE_HEIGHT, value));
		}
		
		public override void SetShapeResolution(int value)
		{
			Debug.LogWarning(MSG.Warnings.SHAPEMESH_RESOLUTION_UNCHANGEABLE);
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
		public override float[] GetRoundingResolutions()
		{
			return roundingsDistances;
		}
		
		public override float GetRoundingResolution(int id)
		{
			return roundingsDistances[id];
		}
		
		public override int GetShapeResolution()
		{
			return 5;
		}
		public override float ClampRoundingDistance(float value)
		{
			return Mathf.Min(Mathf.Max(value, MIN_ROUNDING_DISTANCE), MAX_ROUNDING_DISTANCE);
		}
	}
}