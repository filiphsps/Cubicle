using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using Silk.NET.Maths;

namespace Cubicle.NET.Engine.Rendering
{
    public class Renderer : Manager
    {
        private GL gl;

        private Shader shader;

        public unsafe Renderer(GL gl)
        {
            this.gl = gl;

            shader = new Shader(gl, "test.vert", "test.frag");
        }

        public override void Update()
        {
        }

        public unsafe void Render()
        {
            gl.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            //shader.SetUniform("uBlue", (float)Math.Sin(DateTime.Now.Millisecond / 100f * Math.PI));

            //gl.DrawElements(PrimitiveType.Triangles, (uint)Indices.Length, DrawElementsType.UnsignedInt, null);
        }
    }
}
