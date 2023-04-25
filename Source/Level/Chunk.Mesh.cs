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
        public int VertexCount = 0;

        public void CalculateMesh() {
            if (Blocks.Count <= 0)
                return;

            bool[] visibleFaces = new bool[6];

            foreach (var entry in Blocks) {
                var block = entry.Value;
                var position = entry.Key;

                var textures = Atlas.BlockIndices[block];
                visibleFaces = GetVisibleFaces(visibleFaces, position);

                for (int face = 0; face < 6; face++) {
                    if (visibleFaces[face]) {
                        AddFaceMesh(position, face, textures[face]);
                    }
                }
            }

            Vertices = VertexList.ToArray();
            VertexCount = VertexList.Count;
            VertexList.Clear();
        }
        void AddFaceMesh(Vector3 position, int face, ushort? texture) {
            AddData(VertexList, face, position, texture);
        }

        void AddData(List<VertexPositionTextureLight> vertices, int face, Vector3 position, ushort? texture) {
            var world_pos = new Vector3(Position.X * 16, Position.Y * 16, Position.Z * 16) + position;

            for (int i = 0; i < 6; i++) {
                VertexPositionTextureLight vertex = Cube.Faces[face][i];
                vertex.Position += position;
                vertex.Normal = world_pos;
                vertex.Color = Color.White;

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
