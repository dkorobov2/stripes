using System;
using System.Linq;
using UnityEngine;

namespace QuickPoly
{
    public class ColliderManager
    {
        public static void ReloadCollider(QuickPolygon shape, ColliderType colliderType, MeshData data, StandardBorder border)
        {
            switch (colliderType)
            {
                case ColliderType.None:
                    {
                        RemoveColliders(shape);
                        break;
                    }
                case ColliderType.Box:
                    {
                        BoxCollider2D col = shape.GetComponent<BoxCollider2D>();
                        if (col == null)
                        {
                            Collider2D prevCol = shape.GetComponent<Collider2D>();
                            if (prevCol != null)
                            {
                                Helpers.Destroy(prevCol);
                            }
                            col = shape.gameObject.AddComponent<BoxCollider2D>();
                        }
                        Bounds boundingBox = shape.Bounds;
						#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_OR_NEWER
						col.offset = new Vector2(boundingBox.center.x, boundingBox.center.y);
						#else
						col.center = new Vector2(boundingBox.center.x, boundingBox.center.y);
						#endif
                        col.size = new Vector2(boundingBox.size.x, boundingBox.size.y);
                        break;
                    }
                case ColliderType.FillPolygon:
                    {
						PolygonCollider2D col = shape.GetComponent<PolygonCollider2D>();
                        if (col == null)
                        {
                            Collider2D prevCol = shape.GetComponent<Collider2D>();
                            if (prevCol != null)
                            {
                                Helpers.Destroy(prevCol);
                            }
                            col = shape.gameObject.AddComponent<PolygonCollider2D>();
                        }

                        Vector2[] points = new Vector2[data.BorderVertices.Length];
                        for (int i = 0; i < data.BorderVertices.Length; i++)
                        {
                            Vector3 v = data.Vertices[data.BorderVertices[i]];
                            points[i] = new Vector2(v.x, v.y);
                        }
                        col.points = points;
                        break;
                    }
                case ColliderType.BorderPolygon:
                    {
                        if (border == null)
                        {
                            Debug.LogWarning(MSG.Warnings.COLLIDER_POLYGON_BORDER_NOT_EXIST, shape);
                            shape.ShapeColliderIndex  = ColliderType.None;
							ReloadCollider(shape, shape.ShapeColliderIndex, data, border);
                            break;
                        }
						PolygonCollider2D col = shape.GetComponent<PolygonCollider2D>();
                        if (col == null)
                        {
                            Collider2D prevCol = shape.GetComponent<Collider2D>();
                            if (prevCol != null)
                            {
                                Helpers.Destroy(prevCol);
                            }
                            col = shape.gameObject.AddComponent<PolygonCollider2D>();
                        }
                        Vector2[] points = new Vector2[border.OutVerID.Length];
                        for (int i = 0; i < border.OutVerID.Length; i++)
                        {
                            Vector3 v = data.Vertices[border.OutVerID[i]];
                            points[i] = new Vector2(v.x, v.y);
                        }
                        col.points = points;

                        break;
                    }
                case ColliderType.FillAndBorderPolygon:
                    {
                        if (border == null)
                        {
                            Debug.LogWarning(MSG.Warnings.COLLIDER_POLYGON_BORDER_NOT_EXIST, shape);
                            shape.ShapeColliderIndex = ColliderType.None;
							ReloadCollider(shape, shape.ShapeColliderIndex, data, border);
                            break;
                        }
						PolygonCollider2D col = shape.GetComponent<PolygonCollider2D>();
                        if (col == null)
                        {
                            Collider2D prevCol = shape.GetComponent<Collider2D>();
                            if (prevCol != null)
                            {
                                Helpers.Destroy(prevCol);
                            }
                            col = shape.gameObject.AddComponent<PolygonCollider2D>();
                        }
                        Vector2[] points = new Vector2[data.BorderVertices.Length + 2 + border.OutVerID.Length];
                        for (int i = 0; i < data.BorderVertices.Length; i++)
                        {
                            Vector3 v = data.Vertices[data.BorderVertices[i]];
                            points[i] = new Vector2(v.x, v.y);
                        }
                        points[data.BorderVertices.Length] = points[0];
                        for (int i = 0; i < border.OutVerID.Length; i++)
                        {
                            Vector3 v = data.Vertices[border.OutVerID[i]];
                            points[i + data.BorderVertices.Length + 1] = new Vector2(v.x, v.y);
                        }
                        points[data.BorderVertices.Length + border.OutVerID.Length + 1] = points[data.BorderVertices.Length + 1];
                        col.points = points;
                        break;
                    }
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static void RemoveColliders(QuickPolygon shape)
        {
            shape.ShapeColliderIndex = ColliderType.None;
            Collider2D[] col = shape.GetComponents<Collider2D>();
			if (col != null && col.Any())
            {
                col.ToList().ForEach(c => Helpers.Destroy(c));
            }
        }
    }
}