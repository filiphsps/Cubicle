using Cubicle.Models;
using Cubicle.Rendering;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public VertexPositionTextureLight[] Vertices;
        public List<VertexPositionTextureLight> VertexList;
        public int VertexCount;

        public void CalculateMesh() {
            bool[] visibleFaces = new bool[6];

            foreach (var block in Blocks.Values) {
                visibleFaces = GetVisibleFaces(visibleFaces, block);

                for (int face = 0; face < 6; face++) {
                    if (visibleFaces[face]) {
                        AddFaceMesh(block, face);
                    }
                }
            }

            Vertices = VertexList.ToArray();
            VertexCount = VertexList.Count;
            VertexList.Clear();
        }
        void AddFaceMesh(Block block, int face) {
            // TODO: multiface
            AddData(VertexList, face, block, Position);
        }

        static void AddData(List<VertexPositionTextureLight> vertices, int face, Block block, Vector3 chunkPosition) {
            var position = block.Position;
            var world_pos = new Vector3(chunkPosition.X * 16, chunkPosition.Y, chunkPosition.Z * 16) + position;

            for (int i = 0; i < 6; i++) {
                VertexPositionTextureLight vertex = Cube.Faces[face][i];
                vertex.Position += position;
                vertex.Normal = world_pos;

                var x = chunkPosition.X;
                if (x < 0)
                    x *= -1;
                var z = chunkPosition.Z;
                if (z < 0)
                    z *= -1;

                var r = 25 + (int)((255 - 25) * (x + z) / 8);
                var g = 25 + (int)((100 - 25) * (x + z) / 8);
                var b = 25 + (int)((255 - 25) * (x + z) / 8);

                g += 25 + (int)((155 - 25) * (position.X + position.Z) / 32);
                b += 25 + (int)((155 - 25) * (position.Y) / 16);

                vertex.Color = new Color(r, g, b, 1);


                if (position.X == 15 && position.Z == 15)
                    vertex.Color = Color.Blue;
                else if (position.X == 15 && position.Z == 0)
                    vertex.Color = Color.Red;
                else if (position.X == 0 && position.Z == 15)
                    vertex.Color = Color.Yellow;
                else if (position.X == 0 && position.Z == 0)
                    vertex.Color = Color.Purple;

                var texture = 0; // TODO
                var textureCount = 1; // TODO
                if (vertex.TextureCoordinate.Y == 0) {
                    vertex.TextureCoordinate.Y = (float)texture / textureCount;
                }
                else {
                    vertex.TextureCoordinate.Y = (float)(texture + 1) / textureCount;
                }
                vertices.Add(vertex);
            }
        }
    }
}
