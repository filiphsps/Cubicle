using Cubicle.NET.Engine.Rendering;
using Cubicle.NET.Util;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Numerics;
using Shader = Cubicle.NET.Engine.Rendering.Shader;

namespace Cubicle.NET.Engine
{
    
    public class Entity
    {
        public Model Model;
        public Rendering.Texture Texture;
        public Shader Shader;

        public Vector3 Position;
        public Vector3 Velocity;

        public Vector3 Euler;
        public Vector3 Scale;

        public Entity()
        {
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            Euler = Vector3.Zero;
            Scale = Vector3.One;
        }

        public Entity(GL gl, string vertex, string fragment, float[] meshVertices, uint[] indices)
        {
            //Mesh = new Mesh(gl, meshVertices, indices);
            Shader = new Shader(gl, vertex, fragment);

            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            Euler = Vector3.Zero;
            Scale = Vector3.One;
        }

        public virtual void Update(double delta)
        {
        }

        public virtual void Draw(double delta, Camera camera)
        {
        }
    }
}
