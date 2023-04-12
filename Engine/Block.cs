using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.OpenGL;
using Silk.NET.SDL;

namespace Cubicle.NET.Engine
{
    public class Block : Entity
    {

        private float[] vertices =
        {
            // Front face - 2 triangles
            -0.0f,  1.0f, -0.0f,
                1.0f,  1.0f, -0.0f,
            -0.0f, -0.0f, -0.0f,
            -0.0f, -0.0f, -0.0f,
                1.0f,  1.0f, -0.0f,
                1.0f, -0.0f, -0.0f,

            // Top Face
            -0.0f,  1.0f,  1.0f,
                1.0f,  1.0f,  1.0f,
            -0.0f,  1.0f, -0.0f,
            -0.0f,  1.0f, -0.0f,
                1.0f,  1.0f,  1.0f,
                1.0f,  1.0f, -0.0f,
            
            // Right Face
                1.0f,  1.0f, -0.0f,
                1.0f,  1.0f,  1.0f,
                1.0f, -0.0f, -0.0f,
                1.0f, -0.0f, -0.0f,
                1.0f,  1.0f,  1.0f,
                1.0f, -0.0f,  1.0f,
            
            // Bottom Face
            -0.0f, -0.0f,  1.0f,
                1.0f, -0.0f,  1.0f,
            -0.0f, -0.0f, -0.0f,
            -0.0f, -0.0f, -0.0f,
                1.0f, -0.0f,  1.0f,
                1.0f, -0.0f, -0.0f,
            
            // Back Face
                1.0f,  1.0f,  1.0f,
            -0.0f,  1.0f,  1.0f,
                1.0f, -0.0f,  1.0f,
                1.0f, -0.0f,  1.0f,
            -0.0f,  1.0f,  1.0f,
            -0.0f, -0.0f,  1.0f,
            
            // Left Face
            -0.0f,  1.0f,  1.0f,
            -0.0f,  1.0f, -0.0f,
            -0.0f, -0.0f,  1.0f,
            -0.0f, -0.0f,  1.0f,
            -0.0f,  1.0f, -0.0f,
            -0.0f, -0.0f, -0.0f,
        };

        public Block(GL gl) : base()
        {
            Mesh = new Mesh(gl, vertices);
            Shader = new Rendering.Shader(gl, "block.vert", "block.frag");
        }

        public override void Draw(Camera camera)
        {
            Matrix4 mat = camera.Matrix() * LocalToWorld();

            Shader.Use();
            Shader.SetMatrix(mat);

            Mesh.Draw();
        }
    }
}
