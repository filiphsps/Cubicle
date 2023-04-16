using Cubicle.NET.Engine.Rendering;
using Microsoft.VisualBasic;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Chunk
    {
        // FIXME: Migrate to actual block-class so we can combine meshes
        private List<Block> blocks = new List<Block>();
        public Vector3D<int> Position;

        public Chunk(GL gl, Vector3D<int> position)
        {
            Position = position;

            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 1; y++)
                {
                    for (var z = 0; z < 4; z++)
                    {
                        var block = new Block(gl);
                        block.Position = new Vector3(x, y, z);
                        blocks.Add(block);
                    }
                }
            }
        }

        public void Update(double delta)
        {
        }


        public void Draw(double delta, Camera camera)
        {
            foreach (var block in blocks)
            {
                block.Draw(delta, camera);
            }
        }

        private void GenerateMesh()
        {
            
        }
    }
}
