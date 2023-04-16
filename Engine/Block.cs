using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Assimp;
using Silk.NET.OpenGL;
using Silk.NET.SDL;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Block : Entity
    {
        GL gl;

        static Model? GenericModel;

        public Block(GL gl) : base()
        {
            this.gl = gl;

            if (GenericModel == null)
                GenericModel = new Rendering.Model(gl, "Res/Models/Cube.obj");

            // TODO: Cache these properly
            Texture = new Rendering.Texture(gl, "Res/Blocks/Grass/Texture.png");
            Model = GenericModel;
        }

        public override void Draw(double delta, Rendering.Camera camera)
        {
            Texture.Bind();
            Rendering.Renderer.BlockShader.Use();
            Rendering.Renderer.BlockShader.SetUniform("uTexture0", 0);

            var model = LocalToWorld();
            var view = Matrix4x4.CreateLookAt(camera.Position, camera.Position + camera.Front, camera.Up);
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

                gl.DrawArrays(Silk.NET.OpenGL.PrimitiveType.Triangles, 0, (uint)mesh.Vertices.Length);
            }
        }

        public override Matrix4x4 LocalToWorld()
        {
            return
                Matrix4x4.CreateTranslation(Position * 0.5f + new Vector3(0.25f, 0, 0.25f)) *
                Matrix4x4.CreateRotationY(Euler.Y) *
                Matrix4x4.CreateRotationX(Euler.X) *
                Matrix4x4.CreateRotationZ(Euler.Z) *
                Matrix4x4.CreateScale(Scale * 2);
        }
    }
}
