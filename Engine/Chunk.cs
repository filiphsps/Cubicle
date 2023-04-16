using Cubicle.NET.Engine.Rendering;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using System.Numerics;

namespace Cubicle.NET.Engine
{
    public class Chunk
    {
        // FIXME: Migrate to actual block-class so we can combine meshes
        public Dictionary<Vector3, Block> Blocks = new Dictionary<Vector3, Block>();
        public Vector3 Position;

        public Chunk(GL gl, Vector3 position)
        {
            Position = position;
             
            for (var x = 0; x < 16; x++)
            {
                for (var y = 0; y < 4; y++)
                {
                    for (var z = 0; z < 16; z++)
                    {
                        var id = "Grass";

                        if (y < 3)
                            id = "Dirt";

                        var block = new Block(gl, id);
                        block.Position = new Vector3(x, y, z);
                        block.Chunk = this;
                        Blocks.Add(block.Position, block);
                    }
                }
            }

            Calculate();
        }

        private void Calculate()
        {
            foreach (var block in Blocks.Values)
            {
                // TODO: Do this properly.
                block.CheckEncased();
            }
        }

        public void Update(double delta)
        {
        }


        public void Draw(double delta, Rendering.Camera camera)
        {
            foreach (var block in Blocks.Values)
            {
                block.Draw(delta, camera);
            }
        }

        public void RemoveBlock(Vector3 pos)
        {
            Blocks.Remove(pos);
            Calculate();
        }

        public Block? GetBlock(float x, float y, float z)
        {
            try
            {
                return Blocks[new Vector3(x, y, z)];
            }
            catch
            {
                return null;
            }
        }
        public bool IsBlock(float x, float y, float z)
        {
            return Blocks.ContainsKey(new Vector3(x, y, z));
        }
    }
}
