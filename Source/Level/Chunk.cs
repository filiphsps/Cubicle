using Cubicle.Components;
using Cubicle.Rendering;
using Cubicle.Singletons;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public Vector3 Position;
        public Dictionary<Vector3, BlockReference> Blocks = new Dictionary<Vector3, BlockReference>();

        private DynamicVertexBuffer _buffer;
        private GraphicsDevice _graphics;

        public static int SIZE = 16;
        public static int LAST = 15;

        public Chunk(GraphicsDevice graphics) {
            _graphics = graphics;

            // Roughly ~15% of all blocks are visible
            int total = 6 * (int)(0.15 * SIZE * SIZE * SIZE);

            VertexList = new List<VertexPositionTextureLight>(total);

            _buffer = new DynamicVertexBuffer(graphics, typeof(VertexPositionTextureLight), total + 3000, BufferUsage.None);
        }

        public void Apply(GraphicsDevice graphics) {
            _graphics.SetVertexBuffer(_buffer);
        }

        // TODO: Update this on modifications
        public Atlas Atlas;
        public Texture2D Texture {
            get {
                return Atlas.Texture;
            }
        }
    }
}
