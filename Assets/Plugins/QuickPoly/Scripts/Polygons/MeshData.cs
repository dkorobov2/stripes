using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickPoly
{
	[Serializable]
	public class MeshData
	{
		public Vector3[] Vertices;
		public Color[] Colors;
		public int[] Triangles;
		public int[] BorderVertices;
		public int[] RoundableVertices;
		public int RoundingVertex;
        public Vector2[] UV;
        public Vector3[] normals;

		public MeshData(Vector3[] verts, int[] triangles, int[] borderVertices, int[] roundableVertices, int roundingVertex)
		{
			Vertices = verts;
			Triangles = triangles;
			BorderVertices = borderVertices;
			RoundableVertices = roundableVertices;
			RoundingVertex = roundingVertex;
		}

		private Vector3 BevelMoveSquare(float percent, float offset, Vector3 a, Vector3 b, Vector3 c)
		{
			float roundPercent = Mathf.Sin(percent*Mathf.PI);

			Vector3 nb = Vector3.Lerp(a, b, offset);
			Vector3 nc = Vector3.Lerp(a, c, offset);

			Vector3 l1 = Vector3.Lerp(nb, nc, percent);
			Vector3 norm = StandardBorder.MakeNormal(c, b);
			float scale = Vector3.Distance(a, Vector3.Lerp(b, c, 0.5f))*0.87f;
			Vector3 l3 = l1 + norm*roundPercent*scale;

			return l3;
		}

		private Vector3 BevelMove(float percent, float offset, Vector3 a, Vector3 b, Vector3 c)
		{
			float roundPercent = Mathf.Sin(percent*Mathf.PI);

			Vector3 nb = Vector3.Lerp(a, b, offset);
			Vector3 nc = Vector3.Lerp(a, c, offset);

			Vector3 l1 = Vector3.Lerp(nb, nc, percent);

			float aDistance = Vector3.Distance(Vertices[RoundingVertex], a);
			float l1Distance = Vector3.Distance(Vertices[RoundingVertex], l1);
			float distDif = aDistance - l1Distance;

			Vector3 vect = (l1 - Vertices[RoundingVertex]).normalized;
			Vector3 l3 = l1 + vect*(distDif*roundPercent*0.5f);

			return l3;
		}

		private void RemoveTrisWithIds(int a, int b)
		{
			for (int i = 0; i < Triangles.Length - 1; i += 3)
			{
				int na = Triangles[i];
				int nb = Triangles[i + 1];
				int nc = Triangles[i + 2];

				int[] arrA = new[] {a, b}.OrderBy(x => x).ToArray();
				int[] arrB = new[] {na, nb, nc}.OrderBy(x => x).ToArray();

				bool same = true;
				for (int x = 0; x < arrA.Length; x++)
				{
					if (arrA[x] == arrB[x]) continue;
					same = false;
					break;
				}

				if (same)
				{
					Debug.Log(i);
					var l = Triangles.ToList();
					l.RemoveRange(i, 3);
					Triangles = l.ToArray();
				}
				;
			}
		}

		public void BevelSquare(int roundableId, int n, float offset)
		{
			if (n <= 1) return;

			int borderId;
			for (borderId = 0; borderId < BorderVertices.Length; borderId++)
				if (BorderVertices[borderId] == RoundableVertices[roundableId]) break;

			int aId = BorderVertices[borderId];
			int bId = BorderVertices[borderId == BorderVertices.Length - 1 ? 0 : borderId + 1];
			int cId = BorderVertices[borderId - 1 == -1 ? BorderVertices.Length - 1 : borderId - 1];

			List<Vector3> verticesTemp = Vertices.ToList();
			List<int> tris = Triangles.ToList();
			List<int> borders = BorderVertices.ToList();
			int[] verts = new int[n];
			int nextVertIndex = Vertices.Length;
			verts[0] = aId;
			for (int i = 1; i < n; i++)
			{
				verticesTemp.Add(Vector3.zero);
				verts[i] = nextVertIndex++;

				tris.Add(verts[i]);
				tris.Add(verts[i - 1]);
				tris.Add(cId);

				borders.Insert(borderId, verts[i]);
			}
			Vertices = verticesTemp.ToArray();
			Triangles = tris.ToArray();
			BorderVertices = borders.ToArray();

			for (int i = 0; i < n; i++)
			{
				float percent = 1f*(i + 1)/(n + 1);
				Vertices[verts[i]] = BevelMoveSquare(percent, offset, Vertices[aId], Vertices[bId], Vertices[cId]);
			}
		}

		public void Bevel(int roundableId, int n, float offset)
		{
			if (n <= 1) return;

			int borderId = -1;
			for (var i = 0; i < BorderVertices.Length; i++)
			{
				if (BorderVertices[i] == RoundableVertices[roundableId])
				{
					borderId = i;
					break;
				}
			}

			if (borderId < 0)
			{
				return;
			}

			int aId = BorderVertices[borderId];
			int bId = BorderVertices[borderId == BorderVertices.Length - 1 ? 0 : borderId + 1];
			int cId = BorderVertices[borderId - 1 == -1 ? BorderVertices.Length - 1 : borderId - 1];
			int rId = RoundingVertex;

			List<Vector3> verticesTemp = Vertices.ToList();
			List<int> tris = Triangles.ToList();
			List<int> borders = BorderVertices.ToList();
			int[] verts = new int[n];
			int nextVertIndex = Vertices.Length;
			verts[n - 1] = aId;

			for (int i = 0; i < n - 1; i++)
			{
				verticesTemp.Add(Vector3.zero);
				verts[i] = nextVertIndex++;
			}

			for (int i = 0; i < n - 1; i++)
			{
				tris.Add(verts[i + 1]);
				tris.Add(verts[i]);
				tris.Add(rId);

				borders.Insert(borderId + 1, verts[i]);
			}
			Vertices = verticesTemp.ToArray();
			Triangles = tris.ToArray();
			BorderVertices = borders.ToArray();

			for (int i = 0; i < n; i++)
			{
				float percent = 1f*(i + 1)/(n + 1);
				Vertices[verts[i]] = BevelMove(percent, offset, Vertices[aId], Vertices[bId], Vertices[cId]);
			}

			Reconnect(aId, bId, rId, verts[0]);
		}

		private bool Reconnect(int a, int b, int c, int aTo)
		{
			for (int i = 0; i < Triangles.Length; i += 3)
			{
				if (TryReconnect(i, i + 1, i + 2, a, b, c, aTo)) return true;
				if (TryReconnect(i, i + 2, i + 1, a, b, c, aTo)) return true;

				if (TryReconnect(i + 1, i, i + 2, a, b, c, aTo)) return true;
				if (TryReconnect(i + 1, i + 2, i, a, b, c, aTo)) return true;

				if (TryReconnect(i + 2, i + 1, i, a, b, c, aTo)) return true;
				if (TryReconnect(i + 2, i, i + 1, a, b, c, aTo)) return true;
			}

			return false;
		}

		private bool TryReconnect(int indexA, int indexB, int indexC, int matchA, int matchB, int matchC, int aTo)
		{
			if (Triangles[indexA] == matchA && Triangles[indexB] == matchB && Triangles[indexC] == matchC)
			{
				Triangles[indexA] = aTo;
				return true;
			}
			return false;
		}

		public void PrepareBevel(int roundableId, float distance)
		{
			int borderId = -1;
			for (int i = 0; i < BorderVertices.Length; i++)
			{
				if (BorderVertices[i] == RoundableVertices[roundableId])
				{
					borderId = i;
					break;
				}
			}
			if (borderId < 0)
			{
				return;
			}
			int aId = BorderVertices[borderId];
			int bId = BorderVertices[borderId == BorderVertices.Length - 1 ? 0 : borderId + 1];
			int cId = BorderVertices[borderId - 1 == -1 ? BorderVertices.Length - 1 : borderId - 1];
			int rId = RoundingVertex;

			int bRId = RoundableVertices[roundableId == RoundableVertices.Length - 1 ? 0 : roundableId + 1];
			int cRId = RoundableVertices[roundableId == 0 ? RoundableVertices.Length - 1 : roundableId - 1];

			int vc = Vertices.Length;
			int vb = vc + 1;

			Vector3 v1 = Vertices[cRId] - Vertices[aId];
			Vector3 v2 = Vertices[bRId] - Vertices[aId];

			float maxDist = Mathf.Max(v1.magnitude, v2.magnitude);

			Vector3[] vertices = new[]
			{
				Vertices[aId] + v1.normalized*maxDist*distance,
				Vertices[aId] + v2.normalized*maxDist*distance,
			};

			int[] indices = new[]
			{
				aId, vc, rId,
				vb, aId, rId
			};

			Vertices = Vertices.Concat(vertices).ToArray();
			Triangles = Triangles.Concat(indices).ToArray();

			List<int> bl = BorderVertices.ToList();
			bl.Insert(borderId + 1, vc);
			bl.Insert(borderId, vb);
			BorderVertices = bl.ToArray();

			Reconnect(aId, bId, rId, vc);
			Reconnect(aId, cId, rId, vb);
		}
	}
}