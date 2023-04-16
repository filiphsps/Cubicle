using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Steamworks.Ugc;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Player : Rendering.Camera
    {
        public bool Pause = false;
        public float Height = 1.0f;
        public Vector3? Target = null;
        public float MinDistanceToTarget = 7.5f;
        public Util.Ray Ray;

        public Player(GL gl) : base(gl)
        {
            Position = new Vector3(8.0f, 5.0f, 8.0f);
            Ray = new Util.Ray(PerspectivePosition(), Direction);
        }

        public void Update(double delta)
        {
            Ray.Position = PerspectivePosition();
            Ray.Direction = Direction;
        }

        public override Vector3 PerspectivePosition()
        {
            return this.Position + new Vector3(0.0f, Height, 0.0f);
        }
    }
}
