using UnityEngine;

namespace QuickPoly
{
    [System.Serializable]
    public class FacingModifier
    {
        public FacingDirection facingDirection;

        public void ModifyData(MeshData data)
        {
            if (facingDirection != FacingDirection.Forward)
            {
                if (facingDirection == FacingDirection.Backward)
                {
                    data.Triangles = CalculateBackwardTriangles(data.Triangles);
                }
                else
                {
                    Vector3[] doubleVertices = new Vector3[data.Vertices.Length * 2];
                    System.Array.Copy(data.Vertices, doubleVertices, data.Vertices.Length);
                    System.Array.Copy(data.Vertices, 0, doubleVertices, data.Vertices.Length, data.Vertices.Length);
                    data.Vertices = doubleVertices;

                    Color[] doubleColors = new Color[data.Colors.Length * 2];
                    System.Array.Copy(data.Colors, doubleColors, data.Colors.Length);
                    System.Array.Copy(data.Colors, 0, doubleColors, data.Colors.Length, data.Colors.Length);
                    data.Colors = doubleColors;

                    Vector2[] doubleUV = new Vector2[data.UV.Length * 2];
                    System.Array.Copy(data.UV, doubleUV, data.UV.Length);
                    System.Array.Copy(data.UV, 0, doubleUV, data.UV.Length, data.UV.Length);
                    data.UV = doubleUV;

                    int[] doubleTriangles = new int[data.Triangles.Length * 2];
                    System.Array.Copy(data.Triangles, doubleTriangles, data.Triangles.Length);
                    for (int i = data.Triangles.Length; i < data.Triangles.Length * 2; i = i + 3)
                    {
                        doubleTriangles[i] = data.Triangles[i + 1 - data.Triangles.Length] + data.Vertices.Length / 2;
                        doubleTriangles[i + 1] = data.Triangles[i - data.Triangles.Length] + data.Vertices.Length / 2;
                        doubleTriangles[i + 2] = data.Triangles[i + 2 - data.Triangles.Length] + data.Vertices.Length / 2;
                    }
                    data.Triangles = doubleTriangles;
                }
            }
        }

        private int[] CalculateBackwardTriangles(int[] originalTriangles)
        {
            int[] backwardTriangles = new int[originalTriangles.Length];
            for (int i = 0; i < originalTriangles.Length; i = i + 3)
            {
                backwardTriangles[i] = originalTriangles[i + 1];
                backwardTriangles[i + 1] = originalTriangles[i];
                backwardTriangles[i + 2] = originalTriangles[i + 2];
            }
            return backwardTriangles;
        }
    }
}