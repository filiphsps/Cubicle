using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Steamworks.Ugc;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Player : Entity
    {
        public Player() : base()
        {
        }

        public Player(GL gl, string vertex, string fragment, float[] meshVertices, uint[] indices) : base(gl, vertex, fragment, meshVertices, indices)
        {
        }

        public override void Update(double delta)
        {
        }
    }
}
