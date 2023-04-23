using Cubicle.Models;
using Cubicle.Rendering;
using Cubicle.Singletons;
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
                var textures = Atlas.BlockIndices[block.Id];
                visibleFaces = GetVisibleFaces(visibleFaces, block);

                for (int face = 0; face < 6; face++) {
                    if (visibleFaces[face]) {
                        AddFaceMesh(block, face, textures[face]);
                    }
                }
            }

            Vertices = VertexList.ToArray();
            VertexCount = VertexList.Count;
            VertexList.Clear();

            _buffer.SetData(Vertices);
        }
        void AddFaceMesh(BlockReference block, int face, ushort? texture) {
            AddData(VertexList, face, block, texture);
        }

        void AddData(List<VertexPositionTextureLight> vertices, int face, BlockReference block, ushort? texture) {
            var position = block.Position;
            var world_pos = new Vector3(Position.X * 16, Position.Y, Position.Z * 16) + position;

            for (int i = 0; i < 6; i++) {
                VertexPositionTextureLight vertex = Cube.Faces[face][i];
                vertex.Position += position;
                vertex.Normal = world_pos;
                vertex.Color = Color.White;

                if (position.X == 15 && position.Z == 15)
                    vertex.Color = Color.Blue;
                else if (position.X == 15 && position.Z == 0)
                    vertex.Color = Color.Red;
                else if (position.X == 0 && position.Z == 15)
                    vertex.Color = Color.Yellow;
                else if (position.X == 0 && position.Z == 0)
                    vertex.Color = Color.Purple;

                var textureCount = Atlas.TexturesCount;
                if (vertex.TextureCoordinate.Y == 0) {
                    vertex.TextureCoordinate.Y = (float)texture / textureCount;
                } else {
                    vertex.TextureCoordinate.Y = (float)(texture + 1) / textureCount;
                }
                vertices.Add(vertex);
            }
        }
    }
}
