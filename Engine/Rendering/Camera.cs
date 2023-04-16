using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;
using System.Numerics;

namespace Cubicle.NET.Engine.Rendering
{
    public class Camera
    {
        private GL gl;

        public Vector3 Position = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 Front = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 Up = Vector3.UnitY;
        public Vector3 Direction = Vector3.Zero;
        public float Yaw = -90f;
        public float Pitch = 0f;
        public float Zoom = 65f;
        public float Speed = 5f;

        public Camera(GL gl)
        {
            this.gl = gl;
        }

        public virtual Vector3 PerspectivePosition()
        {
            return Position;
        }

        public void UseViewport()
        {
            gl.Viewport(0, 0, 800, 600);
        }
    }
}
