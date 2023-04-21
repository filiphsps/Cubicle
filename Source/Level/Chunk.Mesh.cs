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
            //bool[] visibleFaces = new bool[6];

            foreach (var block in Blocks) {
                //visibleFaces = GetVisibleFaces(visibleFaces, block.Position);

                for (int face = 0; face < 6; face++) {
                    //if (visibleFaces[face]) {
                        AddFaceMesh(block, face);
                    //}
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

            for (int i = 0; i < 6; i++) {
                VertexPositionTextureLight vertex = Cube.Faces[face][i];
                vertex.Position += position;
                vertex.Color = Color.Green;
                vertex.Color.B -= (byte)((position.X + position.Z) * 8);
                vertex.Color.R -= (byte)((chunkPosition.X + chunkPosition.Z) * 32);

                if (position.X == 15 && position.Z == 15)
                    vertex.Color = Color.Blue;
                else if(position.X == 15 && position.Z == 0)
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
