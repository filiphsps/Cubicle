using Silk.NET.Maths;

namespace Cubicle.NET.Engine.Rendering
{
    public class Camera
    {
        public Vector2D<int> Viewport { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
        public float FOV { get; set; }
        public Matrix4X4<int> Projection { get; private set; }
        public Matrix4X4<int> Worldview { get; private set; }

        public Camera()
        {
            Viewport = new Vector2D<int>(800, 600);
            Projection = Matrix4X4<int>.Identity;
            Worldview = Matrix4X4<int>.Identity;
        }
    }
}
