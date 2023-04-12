using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace Cubicle.NET.Engine.Rendering
{
    public class Renderer : Manager
    {
        private GL gl;

        public static Player player;
        private Chunk chunk;
        private Camera camera;

        public unsafe Renderer(GL gl)
        {
            this.gl = gl;


            camera = new Camera(gl);

            float[] player_vertices = { };

            player = new Player(gl, "scene.vert", "scene.frag", player_vertices);
            chunk = new Chunk(gl);
            
        }

        public override void Update(double delta)
        {
            player.Update(delta);
        }

        public unsafe void Render(double delta)
        {
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            camera.Worldview = player.WorldToCamera();
            camera.SetSize(800, 600, 0.01f, 100.0f, 65.0f);
            camera.UseViewport();

            player.Draw(camera);
            chunk.Draw(delta, camera);
        }
    }
}
