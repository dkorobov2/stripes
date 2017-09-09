using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;


namespace QuickPoly
{
    // Serialization class for QuickPoly shapes. Used when generating xml presets.
    [System.Serializable]
    public class SerializationClass
    {
        public int ShapeMeshIndex;
        public int ShapeFillIndex;
        public int UseShapeBorder;
        public int PivotSnapLocation;
        public LightingType Lighting;

        public SquareShape SquareShape;
        public CircleShape CircleShape;
        public StarShape StarShape;
        public DiamondShape DiamondShape;
        public TriangleShape TriangleShape;
        public HexagonShape HexagonShape;
        public PentagonShape PentagonShape;


        public EmptyFill EmptyFill;
        public SingleColorFill SingleColorFill;
        public LinearGradientFill LinearGradientFill;
        public RadialGradientFill RadialGradientFill;
        public StandardBorder StandardBorder;

        public PivotModifier PivotModifier;
        public ProportionsModifier ProportionsModifier;
        public FacingModifier FacingModifier;
        public SortingLayerModifier SortingLayerModifier;

        public int[] Roundings;
        public bool UniformRounding;
		public bool IsUI;

        public static string Serialize(QuickPolygon shape)
        {
            var serializationClass = new SerializationClass
            {
                ShapeMeshIndex = (int)shape.ShapeMeshIndex,
                ShapeFillIndex = (int)shape.ShapeFillIndex,
                UseShapeBorder = (int)shape.UseShapeBorder,

                SquareShape = shape.SquareShape,
                CircleShape = shape.CircleShape,
                StarShape = shape.StarShape,
                DiamondShape = shape.DiamondShape,
                TriangleShape = shape.TriangleShape,
                HexagonShape = shape.HexagonShape,
                PentagonShape = shape.PentagonShape,

                EmptyFill = shape.EmptyFill,
                SingleColorFill = shape.SingleColorFill,
                LinearGradientFill = shape.LinearGradientFill,
                RadialGradientFill = shape.RadialGradientFill,
                StandardBorder = shape.StandardBorder,

                PivotModifier = shape.PivotModifier,
                ProportionsModifier = shape.ProportionsModifier,
                FacingModifier = shape.FacingModifier,
                SortingLayerModifier = shape.SortingLayerModifier,

				IsUI = shape.IsUI,
                Lighting = shape.Lighting
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializationClass));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, serializationClass);
                return textWriter.ToString();
            }
        }

        public static void Deserialize(QuickPolygon shape, string str, bool loadGeometry)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SerializationClass));

            TextReader reader = new StringReader(str);
            SerializationClass serializationClass = (SerializationClass)serializer.Deserialize(reader);
            reader.Close();
            if (loadGeometry)
            {
                shape.ShapeMeshIndex = (MeshType)serializationClass.ShapeMeshIndex;

                shape.SquareShape = serializationClass.SquareShape;
                shape.CircleShape = serializationClass.CircleShape;
                shape.StarShape = serializationClass.StarShape;
                shape.DiamondShape = serializationClass.DiamondShape;
                shape.TriangleShape = serializationClass.TriangleShape;
                shape.HexagonShape = serializationClass.HexagonShape;
                shape.PentagonShape = serializationClass.PentagonShape;
            }
            shape.ShapeFillIndex = (FillType)serializationClass.ShapeFillIndex;
            shape.UseShapeBorder = (BorderFillType)serializationClass.UseShapeBorder;

            shape.EmptyFill = serializationClass.EmptyFill;
            shape.SingleColorFill = serializationClass.SingleColorFill;
            shape.LinearGradientFill = serializationClass.LinearGradientFill;
            shape.RadialGradientFill = serializationClass.RadialGradientFill;
            shape.StandardBorder = serializationClass.StandardBorder;

            shape.PivotModifier = serializationClass.PivotModifier;
            shape.ProportionsModifier = serializationClass.ProportionsModifier;
            shape.FacingModifier = serializationClass.FacingModifier;
            shape.SortingLayerModifier = serializationClass.SortingLayerModifier;

			shape.IsUI = serializationClass.IsUI;
            shape.Lighting = serializationClass.Lighting;
        }
    }
}