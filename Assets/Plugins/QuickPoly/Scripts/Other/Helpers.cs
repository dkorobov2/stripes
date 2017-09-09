using UnityEngine;
using System.Collections.Generic;

namespace QuickPoly
{
    public class Helpers
    {
        #if UNITY_EDITOR
        // Objects set to destroy in the editor mode are added to the queue.
        public static Queue<UnityEngine.Object> destroyQueue = new Queue<UnityEngine.Object>();
        #endif

        // Destroy helper. In the editor Destroy Immediate should be used at the end of the inspector update.
        public static void Destroy(UnityEngine.Object obj, bool useQueue = true)
        {
            #if UNITY_EDITOR
            if (Application.isPlaying)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                if (useQueue)
                {
                    destroyQueue.Enqueue(obj);
                }
                else
                {
                    GameObject.DestroyImmediate(obj);
                }                
            }
            #else
		    GameObject.Destroy(obj);
            #endif
        }

        private const float maxLabelWidth = 75;

        public static float CalculateLabelWidth(int fieldsPerRow, float rowWidth)
        {
            return Mathf.Min(rowWidth / (fieldsPerRow * 2.0f), maxLabelWidth);
        }

        public static float CalculateFieldWidth(int fieldsPerRow, float rowWidth)
        {
            return (rowWidth / fieldsPerRow) - CalculateLabelWidth(fieldsPerRow, rowWidth);
        }

        public static Bounds CalculateBounds(MeshData data, bool includeBorder = false)
        {
            // Find bounds of the shape's fill.
            float minX = Mathf.Infinity;
            float maxX = -Mathf.Infinity;
            float minY = Mathf.Infinity;
            float maxY = -Mathf.Infinity;

            for (int i = 0; i < data.Vertices.Length; i++)
            {
                // Check if the color isn't black. Color is black when the vertex belongs to the border.
                if (data.Colors[i] != Color.black || includeBorder)
                {
                    Vector3 fillVertex = data.Vertices[i];
                    if (fillVertex.x < minX) minX = fillVertex.x;
                    if (fillVertex.x > maxX) maxX = fillVertex.x;
                    if (fillVertex.y < minY) minY = fillVertex.y;
                    if (fillVertex.y > maxY) maxY = fillVertex.y;
                }
            }

            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.0f);
            Vector3 center = new Vector3(minX + size.x / 2.0f, minY + size.y / 2.0f, 0.0f);

            return new Bounds(center, size);
        }
    }
}