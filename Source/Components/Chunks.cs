using Cubicle.Level;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Cubicle.Components {
    public class Chunks {
        public Dictionary<Vector3, Chunk> LoadedChunks = new Dictionary<Vector3, Chunk>();
    }
}
