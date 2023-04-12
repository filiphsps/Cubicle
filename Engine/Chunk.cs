using Cubicle.NET.Engine.Rendering;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Cubicle.NET.Engine
{
    public class Chunk
    {
        // FIXME: Migrate to actual block-class so we can combine meshes
        private List<Block> blocks = new List<Block>();

        public Chunk(GL gl)
        {

            for (float x = 0; x < 16; x++)
            {
                for (float y = 0; y < 2; y++)
                {
                    for (float z = 0; z < 16; z++)
                    {
                        var block = new Block(gl);
                        block.Position = new Vector3D<float>(x, y, z);
                        blocks.Add(block);
                    }
                }
            }
        }

        public void Update(double delta)
        {
            foreach (var block in blocks)
            {
                block.Update(delta);
            }
        }


        public void Draw(double delta, Camera camera)
        {
            foreach (var block in blocks)
            {
                block.Draw(camera);
            }
        }
    }
}
