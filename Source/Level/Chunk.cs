using Cubicle.Rendering;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Level {
    public sealed partial class Chunk : IDisposable {
        public Vector3 Position;
        public Dictionary<Vector3, Block> Blocks = new Dictionary<Vector3, Block>();

        public static int SIZE = 16;
        public static int HEIGHT = 128;
        public static int LAST = 15;

        public Chunk() {
            //Only about ~5% of all blocks are visible
            int total = (int)(0.05 * SIZE * SIZE * HEIGHT);

            VertexList = new List<VertexPositionTextureLight>(6 * total);
        }

        public void Dispose() { }
    }
}
