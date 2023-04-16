using System.Linq;
using System.Numerics;

namespace Cubicle.NET.Engine.Rendering
{
    public struct Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Tangent;
        public Vector2 TexCoords;
        public Vector3 Bitangent;

        public const int MAX_BONE_INFLUENCE = 8;
        public int[] BoneIds;
        public float[] Weights;
    }
}
