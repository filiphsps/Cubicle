using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using Silk.NET.Windowing;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

namespace Cubicle.NET.Engine
{
    public class Block : Entity
    {
        GL gl;

        public Block(GL gl) : base()
        {
            this.gl = gl;

            Shader = new Rendering.Shader(gl, "Res/Shaders/block.vert", "Res/Shaders/block.frag");
            Texture = new Rendering.Texture(gl, "Res/Models/texture.png");
            Model = new Rendering.Model(gl, "Res/Models/Cube.obj");
        }

        public override void Draw(double delta, Rendering.Camera camera)
        {
            Texture.Bind();
            Shader.Use();
            Shader.SetUniform("uTexture0", 0);

            var model = LocalToWorld();
            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + camera.Front, camera.Up);
            //It's super important for the width / height calculation to regard each value as a float, otherwise
            //it creates rounding errors that result in viewport distortion
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.Zoom), (float)800 / (float)600, 0.1f, 100.0f);

            foreach (var mesh in Model.Meshes)
            {
                mesh.Bind();
                Shader.Use();
                Texture.Bind();
                Shader.SetUniform("uTexture0", 0);
                Shader.SetUniform("uModel", model);
                Shader.SetUniform("uView", view);
                Shader.SetUniform("uProjection", projection);

                gl.DrawArrays(Silk.NET.OpenGL.PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
            }
        }

        public Matrix4x4 LocalToWorld()
        {
            return
                Matrix4x4.CreateTranslation(Position / 2) *
                Matrix4x4.CreateRotationY(Euler.Y) *
                Matrix4x4.CreateRotationX(Euler.X) *
                Matrix4x4.CreateRotationZ(Euler.Z) *
                Matrix4x4.CreateScale(Scale / 2);
        }
    }
}
