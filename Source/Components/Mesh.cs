using Cubicle.Rendering;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Components {
    public class Mesh {
        public Mesh() {
            // TODO: Remove this
            Vertices = new VertexPositionTextureLight[6];
            Vertices[0].Position = new Vector3(-1, -1, 1);
            Vertices[1].Position = new Vector3(-1, 1, 1);
            Vertices[2].Position = new Vector3(1, -1, 1);
            Vertices[3].Position = Vertices[1].Position;
            Vertices[4].Position = new Vector3(1, 1, 1);
            Vertices[5].Position = Vertices[2].Position;
        }

        public VertexPositionTextureLight[] Vertices;
        public List<VertexPositionTextureLight> VertexList = new List<VertexPositionTextureLight>();
    }
}
