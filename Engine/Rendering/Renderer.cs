using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using Silk.NET.Maths;
using System.Drawing;
using System;
using Silk.NET.Input;

namespace Cubicle.NET.Engine.Rendering
{
    public class Renderer : Manager
    {
        private GL gl;

        public static Player player;
        private Entity entity;
        private Camera camera;

        public unsafe Renderer(GL gl)
        {
            this.gl = gl;


            camera = new Camera(gl);

            float[] vertices =
            {
                // Front face - 2 triangles
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,

                // Top Face
                -1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f,  1.0f, -1.0f,
            
                // Right Face
                 1.0f,  1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f, -1.0f,
                 1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
            
                // Bottom Face
                -1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f, -1.0f,
            
                // Back Face
                 1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                 1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,
            
                // Left Face
                -1.0f,  1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f, -1.0f,  1.0f,
                -1.0f,  1.0f, -1.0f,
                -1.0f, -1.0f, -1.0f,
            };

            player = new Player();

            entity = new Entity(gl, "test.vert", "test.frag", vertices);
            entity.Position = new Vector3D<float>(0, 0.5f, -5.0f);
        }

        public override void Update(double delta)
        {
        }

        public unsafe void Render(double delta)
        {
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            gl.ClearColor(0.2f, 0.2f, 0.8f, 1.0f);

            camera.Worldview = player.WorldToCamera();
            camera.SetSize(800, 600, 0.01f, 100.0f, 65.0f);
            camera.UseViewport();

            player.Update(delta);

            entity.Draw(camera);
        }
    }
}
