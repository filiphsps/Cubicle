using Cubicle.Rendering;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Components {
    public class Mesh {
        public VertexPositionTextureLight[] Vertices;
        public List<VertexPositionTextureLight> VertexList = new List<VertexPositionTextureLight>();
    }
}
