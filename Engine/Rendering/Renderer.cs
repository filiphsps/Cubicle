using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Cubicle.NET.Engine.Rendering
{
    public class Renderer : Manager
    {
        private GL gl;

        private Chunk chunk;

        public static Camera Camera;
        public static Shader BlockShader;

        public static Dictionary<String, Texture> Textures = new Dictionary<String, Texture>();
        public static Dictionary<String, Model> Models = new Dictionary<String, Model>();

        public unsafe Renderer(GL gl)
        {
            this.gl = gl;
            BlockShader = new Rendering.Shader(gl, "Res/Shaders/block.vert", "Res/Shaders/block.frag");

            Camera = new Camera(gl);
            chunk = new Chunk(gl, new Vector3(0, 0, 0));
            
        }

        private bool wireframe = false;
        public void ToggleWireframe()
        {
            wireframe = !wireframe;
            if (wireframe)
                gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Line);
            else
                gl.PolygonMode(TriangleFace.FrontAndBack, PolygonMode.Fill);

        }

        public override void Update(double delta)
        {
            //player.Update(delta);
        }

        public unsafe void Render(double delta)
        {
            gl.Enable(EnableCap.DepthTest);
            gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            chunk.Draw(delta, Camera);
        }
    }
}
