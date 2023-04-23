using Cubicle.Level;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Components {
    public class Chunks {
        public Dictionary<Vector3, Chunk> LoadedChunks = new Dictionary<Vector3, Chunk>();
    }
}
