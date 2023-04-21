using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cubicle.Level;

namespace Cubicle.Components {
    public class Chunks {
        public Dictionary<Vector3, Chunk> LoadedChunks = new Dictionary<Vector3, Chunk>();
    }
}
