using Cubicle.Rendering;
using Cubicle.Singletons;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public Vector3 Position;
        public Dictionary<Vector3, BlockReference> Blocks = new Dictionary<Vector3, BlockReference>();

        public static int SIZE = 16;
        public static int LAST = 15;

        public Chunk() {
            //Only about ~5% of all blocks are visible
            int total = (int)(0.05 * SIZE * SIZE * SIZE);

            VertexList = new List<VertexPositionTextureLight>(6 * total);
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
