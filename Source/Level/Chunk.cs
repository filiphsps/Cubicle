using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Level {
    public class Chunk {
        public Vector3 Position;
        public List<Block> Blocks = new List<Block>();
    }
}
