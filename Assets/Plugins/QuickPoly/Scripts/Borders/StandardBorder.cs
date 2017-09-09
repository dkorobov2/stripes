using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickPoly
{
	[Serializable]
	public class StandardBorder : Border
	{
        private const float MIN_OUTER_SCALE = 0.02f;
		public float outerScale = 0.02f;

        private const float MIN_INNER_SCALE = 0.0f;
        private const float MAX_INNER_SCALE = 1.0f;
        [Range(MIN_INNER_SCALE, MAX_INNER_SCALE)]
		public float innerScale = 0f;

		public Color color = new Color(218.0f / 255.0f, 140.0f / 255.0f, 0.0f / 255.0f, 1.0f);
		public bool KeepGeometry = true;
		public float UniformScale = 1;
		public Vector3[] OuterVerticles;
		public int[] OutVerID;
		public static Vector3 MakeNormal(Vector3 a, Vector3 b)
		{
			float dx = b.x - a.x;
			float dy = b.y - a.y;
			float z = (a.z + b.z)/2f;

			return new Vector3(dy, -dx, z);
		}

		public static Vector3 MakeNormal(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 nb = b - a;
			Vector3 nc = c - a;

			nb.Normalize();
			nc.Normalize();

			return -MakeNormal(nb, nc).normalized;
		}

		public static float RightAngleHelper(Vector3 a, Vector3 b, Vector3 c)
		{
			Vector3 nb = b - a;
			Vector3 nc = c - a;

			float angle = 0.5f*(180 - Vector3.Angle(nb, nc));
			return 1.0f/Mathf.Cos(Mathf.Deg2Rad*angle);
		}

		public override void ModifyData(MeshData data, Material material)
		{
			Vector3[] vertices = new Vector3[data.BorderVertices.Length*2];
			OuterVerticles = new Vector3[data.BorderVertices.Length];
			OutVerID = new int[data.BorderVertices.Length];
			Color[] colors = new Color[vertices.Length];
			int[] tris = new int[data.BorderVertices.Length*6];
			List<int> borders = new List<int>();
			bool facingForward = true;

			int len = data.BorderVertices.Length;

			for (int i = 0; i < len; i++)
			{
				int ia = i;
				int ib = i == data.BorderVertices.Length - 1 ? 0 : i + 1;
				int ic = i == 0 ? data.BorderVertices.Length - 1 : i - 1;

				Vector3 a = data.Vertices[data.BorderVertices[ia]];
				Vector3 b = data.Vertices[data.BorderVertices[ib]];
				Vector3 c = data.Vertices[data.BorderVertices[ic]];

				Vector3 normal = MakeNormal(a, b, c);
				float rightAngleMod = RightAngleHelper(a, b, c);

				float zOffset = 0f;
				if (outerScale < 0)
				{
					outerScale = 0;
				}
				Vector3 mid2Verticle = data.Vertices[data.BorderVertices[i]] - data.Vertices[0];
				vertices[i] = data.Vertices[data.BorderVertices[i]] + (normal * rightAngleMod * -(outerScale)) -
				              new Vector3(0f, 0f, zOffset);
				colors[i] = Color.black;
				// If InnerBorder is 1 we do not need to calculate inner border as it is within the middle vertext.
				if (innerScale < 1)
				{
					vertices[len + i] = data.Vertices[data.BorderVertices[i]] - (mid2Verticle*innerScale) -
					                    new Vector3(0f, 0f, zOffset);
					colors[len + i] = Color.black;
				}
			}

			if (innerScale >= 1)
			{
				// if Inner border is completly covering inner mesh we do not render a border. By adjusting inner mesh so it looks like 
				// border we save every single vertex that would need to be used in rendering border.
				data.Colors[0] = Color.black;
				for (int i = 0; i < len; i++)
				{
					data.Vertices[data.BorderVertices[i]] = vertices[i];
					OuterVerticles[i] = vertices[i];
					data.Colors[data.BorderVertices[i]] = colors[i];
				}
			}
			else
			{
				for (int i = 0; i < data.BorderVertices.Length; i++)
				{
					int trisId = i*6;

					int ia = len + i;
					int ib = i == data.BorderVertices.Length - 1 ? len : len + i + 1;

					ia += data.Vertices.Length;
					ib += data.Vertices.Length;

					int ic = ib - len;
					int id = ia - len;

					if (facingForward)
					{
						tris[trisId++] = ia;
						tris[trisId++] = ic;
						tris[trisId++] = ib;

						tris[trisId++] = ia;
						tris[trisId++] = id;
						tris[trisId++] = ic;
					}
					else
					{
						tris[trisId++] = ia;
						tris[trisId++] = ib;
						tris[trisId++] = ic;

						tris[trisId++] = ia;
						tris[trisId++] = ic;
						tris[trisId++] = id;
					}

					borders.Add(ia);
				}
				for (int i = 0; i < len; i++)
				{
					data.Vertices[data.BorderVertices[i]] = vertices[len + i];
					OutVerID[i] = 1 + len + i;
				}
				data.BorderVertices = borders.ToArray();
				data.Triangles = KeepGeometry ? data.Triangles.Concat(tris).ToArray() : tris.ToArray();
				data.Vertices = data.Vertices.Concat(vertices).ToArray();
				data.Colors = data.Colors.Concat(colors).ToArray();
			}

            if (material.HasProperty("_ReplaceColor"))
            {
                material.SetColor("_ReplaceColor", color);
            }
		}
    }
}