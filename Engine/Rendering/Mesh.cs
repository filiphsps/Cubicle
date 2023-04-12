using Silk.NET.OpenGL;

namespace Cubicle.NET.Engine.Rendering
{
    public class Mesh
    {
        private GL gl;

        public uint VAO { get; set; }
        public uint VBO { get; set; }
        public List<float>? Verticies { get; set; }

        public Mesh(GL gl)
        {
            this.gl = gl;
        }
        public Mesh(GL gl, float[] vertices)
        {
            this.gl = gl;
            LoadVertices(vertices);
        }

        public unsafe void LoadVertices(float[] vertices)
        {
            Verticies = new List<float>(vertices);

            VAO = gl.GenVertexArray();
            gl.BindVertexArray(VAO);

            VBO = gl.GenBuffer();

            fixed (float* buf = Verticies.ToArray())
            {
                gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
                gl.BufferData(
                    BufferTargetARB.ArrayBuffer,
                    (nuint)(Verticies.Count * sizeof(float)),
                    buf,
                    BufferUsageARB.StaticDraw);
                gl.EnableVertexAttribArray(0);
                gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, 0, (void*)0);
            }
        }

        public void Draw()
        {
            gl.BindVertexArray(VAO);
            gl.DrawArrays(PrimitiveType.Triangles, 0, (uint)Verticies!.Count);
        }
    }
}
