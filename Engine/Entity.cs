using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Shader = Cubicle.NET.Engine.Rendering.Shader;

namespace Cubicle.NET.Engine
{
    
    public class Entity
    {
        public Mesh Mesh { get; private set; }
        public Shader Shader { get; private set; }

        public Vector3D<float> Position;
        public Vector3D<float> Velocity;

        public Vector3D<float> Euler;
        public Vector3D<float> Scale;

        public Entity()
        {
            Position = Vector3D<float>.Zero;
            Velocity = Vector3D<float>.Zero;
            Euler = Vector3D<float>.Zero;
            Scale = Vector3D<float>.One;
        }

        public Entity(GL gl, string vertex, string fragment, float[] meshVertices)
        {
            Mesh = new Mesh(gl, meshVertices);
            Shader = new Shader(gl, vertex, fragment);

            Position = Vector3D<float>.Zero;
            Velocity = Vector3D<float>.Zero;
            Euler = Vector3D<float>.Zero;
            Scale = Vector3D<float>.One;
        }

        public virtual void Update(double delta)
        {
        }

        public virtual void Draw(Camera camera)
        {
            Matrix4 mat = camera.Matrix() * LocalToWorld();

            Shader.Use();
            Shader.SetMatrix(mat);

            Mesh.Draw();
        }

        public Matrix4 LocalToWorld()
        {
            return
                Matrix4.Translate(Position) *
                Matrix4.RotateY(Euler.Y) *
                Matrix4.RotateX(Euler.X) *
                Matrix4.RotateZ(Euler.Z) *
                Matrix4.Scale(Scale);
        }

        public Matrix4 WorldToLocal()
        {
            return
                Matrix4.Scale(Scale / 1.0f) *
                Matrix4.RotateZ(-Euler.Z) *
                Matrix4.RotateX(-Euler.X) *
                Matrix4.RotateY(-Euler.Y) *
                Matrix4.Translate(-Position);
        }
    }
}
