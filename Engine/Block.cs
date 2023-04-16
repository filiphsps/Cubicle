using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Block : Entity
    {
        GL gl;

        public Chunk Chunk;
        public bool Encased;

        public Block(GL gl, String id = "Grass") : base()
        {
            this.gl = gl;
            var model = "Res/Models/Cube.obj";
            var texture = $"Res/Blocks/{id}/Texture.png";


            if (!Rendering.Renderer.Models.ContainsKey(model))
            {
                Model = new Rendering.Model(gl, model);
                Rendering.Renderer.Models.Add(model, Model);
            }
            else
            {
                Model = Rendering.Renderer.Models[model];
            }

            if (!Rendering.Renderer.Textures.ContainsKey(texture))
            {
                Texture = new Rendering.Texture(gl, texture);
                Rendering.Renderer.Textures.Add(texture, Texture);
            }
            else
            {
                Texture = Rendering.Renderer.Textures[texture];
            }
        }

        public override void Draw(double delta, Rendering.Camera camera)
        {
            // Don't draw encased blocks
            if (Encased)
                return;

            Texture.Bind();
            Rendering.Renderer.BlockShader.Use();
            Rendering.Renderer.BlockShader.SetUniform("uTexture0", 0);

            var model = LocalToWorld();
            var view = Matrix4x4.CreateLookAt(camera.PerspectivePosition(), camera.PerspectivePosition() + camera.Front, camera.Up);
            //It's super important for the width / height calculation to regard each value as a float, otherwise
            //it creates rounding errors that result in viewport distortion
            var near = 0.1f;
            var far = 100f;
            var projection = Matrix4x4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(camera.Zoom), (float)800 / (float)600, near, far);

            foreach (var mesh in Model.Meshes)
            {
                mesh.Bind();
                Rendering.Renderer.BlockShader.Use();
                Texture.Bind();
                Rendering.Renderer.BlockShader.SetUniform("uTexture0", 0);
                Rendering.Renderer.BlockShader.SetUniform("uModel", model);
                Rendering.Renderer.BlockShader.SetUniform("uView", view);
                Rendering.Renderer.BlockShader.SetUniform("uProjection", projection);
                Rendering.Renderer.BlockShader.SetUniform("uHighlight", new Vector4(0, 0, 0, 0));

                if (Cubicle.Player.Target == Position)
                    Rendering.Renderer.BlockShader.SetUniform("uHighlight", new Vector4(0.15f, 0.15f, 0.15f, 1f));

                gl.DrawArrays(Silk.NET.OpenGL.PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
            }
        }

        public void CheckEncased()
        {
            bool res = true;
            var x = Position.X;
            var y = Position.Y;
            var z = Position.Z;

            // Above
            if (!Chunk.IsBlock(x, y + 1, z))
                res = false;
            // Bellow
            else if (!Chunk.IsBlock(x, y - 1, z))
                res = false;
            // Front
            else if (!Chunk.IsBlock(x, y, z + 1))
                res = false;
            // Behind
            else if (!Chunk.IsBlock(x, y, z - 1))
                res = false;
            // Right
            else if (!Chunk.IsBlock(x + 1, y, z))
                res = false;
            // Left
            else if (!Chunk.IsBlock(x - 1, y, z))
                res = false;

            Encased = res;
        }

        public override Matrix4x4 LocalToWorld()
        {
            return
                Matrix4x4.CreateTranslation((Chunk.Position + Position) * 0.5f + new Vector3(0.25f, 0.25f, 0.25f)) *
                Matrix4x4.CreateRotationY(Euler.Y) *
                Matrix4x4.CreateRotationX(Euler.X) *
                Matrix4x4.CreateRotationZ(Euler.Z) *
                Matrix4x4.CreateScale(Scale * 2);
        }
    }
}
