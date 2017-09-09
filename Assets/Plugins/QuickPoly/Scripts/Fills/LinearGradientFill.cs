using UnityEngine;
using System;

namespace QuickPoly
{
	[Serializable]
	public class LinearGradientFill : Fill
	{
        public Color colorA = Color.white;
        public Color colorB = Color.black;
        [Range(0.0f, 360.0f)]
		public float angle;
        [Range(0.0f, 1.0f)]
		public float percentage = 0.5f;

        public override void ModifyData(MeshData data, Material material)
		{
            FillWithWhite(data);

            // Calculate colors based on percentage.
            Color fromColor;
            Color toColor;

            if (percentage < 0.5f)
            {
                toColor = Color.Lerp(colorB, colorA, -2.0f * percentage + 1.0f);
                fromColor = colorA;
            }
            else
            {
                fromColor = Color.Lerp(colorA, colorB, 2.0f * percentage - 1.0f);
                toColor = colorB;
            }

            if (material.HasProperty("_GradientColorA"))
            {
                material.SetColor("_GradientColorA", fromColor);
            }
            if (material.HasProperty("_GradientColorB"))
            {
                material.SetColor("_GradientColorB", toColor);
            }
            if (material.HasProperty("_GradientType"))
            {
                material.SetFloat("_GradientType", 0.0f);
            }

            // Calculate correct vectors from the angle.
            Vector2 from = new Vector2(-0.5f, 0.0f);
            Vector2 to = new Vector2(0.5f, 0.0f);

            // Rotate.
            from = Quaternion.Euler(0, 0, angle) * from;
            to = Quaternion.Euler(0, 0, angle) * to;

            // Normalize to uv coordinates.
            Vector2 normalizationVector = new Vector2(0.5f, 0.5f);
            from += normalizationVector;
            to += normalizationVector;

            if (material.HasProperty("_GradientCoords"))
            {
                material.SetVector("_GradientCoords", new Vector4(from.x, from.y, to.x, to.y));
            }
		}
	}
}