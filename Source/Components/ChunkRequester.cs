using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Components {
    public class ChunkRequester {
        public List<Vector3> RequestedChunks = new List<Vector3>(8 * 8 * 8);
        public readonly object RequestedChunksLock = new object();
    }
}
