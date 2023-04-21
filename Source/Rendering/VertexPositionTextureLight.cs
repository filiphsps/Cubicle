using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cubicle.Rendering {
    public struct VertexPositionTextureLight : IVertexType {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;
        public Vector2 TextureCoordinate;

        static readonly VertexDeclaration vertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 7, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
        );

        public VertexDeclaration VertexDeclaration {
            get { return vertexDeclaration; }
        }

        public VertexPositionTextureLight(Vector3 position, Color color, Vector3 normal, Vector2 textureCoordinate) {
            Position = position;
            Color = color;
            Normal = normal;
            TextureCoordinate = textureCoordinate;
        }
    }
}
