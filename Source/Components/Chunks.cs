using Cubicle.Level;
using System.Collections.Concurrent;
using System.Numerics;

namespace Cubicle.Components {
    public class Chunks {
        public ConcurrentDictionary<Vector3, Chunk> LoadedChunks = new ConcurrentDictionary<Vector3, Chunk>();
    }
}
