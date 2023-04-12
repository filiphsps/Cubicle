using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.Vulkan;

namespace Cubicle.NET.Engine.Rendering
{
    public class Camera
    {
        private GL gl;

        public Vector2D<uint> Viewport { get; set; }
        public float Near { get; set; }
        public float Far { get; set; }
        public float FOV { get; set; }
        public Matrix4 Projection { get; private set; }
        public Matrix4 Worldview;

        public Camera(GL gl)
        {
            this.gl = gl;

            Viewport = new Vector2D<uint>(800, 600);
            Projection = Matrix4.Identity();
            Worldview = Matrix4.Identity();
        }

        public void SetSize(int w, int h, float n, float f, float fov)
        {
            Viewport = new Vector2D<uint>((uint)w, (uint)h);
            Near = n;
            Far = f;
            FOV = fov;

            float fovRads = 1.0f / MathF.Tan(fov * MathF.PI / 360.0f);
            float aspect = ((float)h) / ((float)w);
            float distance = n - f;

            Projection.M[0] = fovRads * aspect;
            Projection.M[1] = 0.0f;
            Projection.M[2] = 0.0f;
            Projection.M[3] = 0.0f;

            Projection.M[4] = 0.0f;
            Projection.M[5] = fovRads;
            Projection.M[6] = 0.0f;
            Projection.M[7] = 0.0f;

            Projection.M[8] = 0.0f;
            Projection.M[9] = 0.0f;
            Projection.M[10] = (n + f) / distance;
            Projection.M[11] = (2 * n * f) / distance;

            Projection.M[12] = 0.0f;
            Projection.M[13] = 0.0f;
            Projection.M[14] = -1.0f;
            Projection.M[15] = 0.0f;
        }

        public void UseViewport()
        {
            gl.Viewport(0, 0, Viewport.X, Viewport.Y);
        }

        public Matrix4 Matrix()
        {
            return Projection * Worldview;
        }
    }
}
