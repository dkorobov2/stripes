using UnityEngine;
using System.Collections;

namespace QuickPoly
{
    public class UVModifier
    {
        public static void ModifyData(MeshData data)
        {
            data.UV = new Vector2[data.Vertices.Length];

            Bounds bounds = Helpers.CalculateBounds(data);

            float minX = bounds.center.x - bounds.extents.x;
            float minY = bounds.center.y - bounds.extents.y;

            // Set the UVs.
            for (int i = 0; i < data.Vertices.Length; i++)
            {
                // Check if the color isn't black. Color is black when the vertex belongs to the border.
                if (data.Colors[i] != Color.black)
                {
                    Vector3 fillVertex = data.Vertices[i];
                    data.UV[i] = new Vector2((fillVertex.x - minX) / bounds.size.x, (fillVertex.y - minY) / bounds.size.y);
                }
                else
                {
                    data.UV[i] = Vector3.zero;
                }
            }
        }
    }
}