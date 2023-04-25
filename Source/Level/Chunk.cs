using Cubicle.Gearset;
using Cubicle.Rendering;
using Cubicle.Singletons;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public Vector3 Position;
        public Dictionary<Vector3, int> Blocks;

        private DynamicVertexBuffer _buffer;
        private GraphicsDevice _graphics;

        public static int SIZE = 16;
        public static int LAST = 15;

        public Chunk(GraphicsDevice graphics) {
            _graphics = graphics;

            // Roughly ~15% of all blocks are visible
            Blocks = new Dictionary<Vector3, int>(SIZE * SIZE * SIZE);
            VertexList = new List<VertexPositionTextureLight>(SIZE * SIZE * SIZE);
        }

        public int this[int x, int y, int z] {
            get => Blocks[new Vector3(x, y, z)];
            set => Blocks.Add(new Vector3(x, y, z), value);
        }
        public bool Empty() {
            return Blocks.Count <= 0;
        }

        public void Apply(GraphicsDevice graphics) {
            GS.BeginMark("Chunk.Apply", Microsoft.Xna.Framework.Color.Coral);
            if (_buffer == null) {
                GS.BeginMark("Chunk.Apply.AllocateVertextBuffer", Microsoft.Xna.Framework.Color.Silver);
                // FIXME: System for handling VBO(s).
                // FIXME: Handle chunk updates.
                _buffer = new DynamicVertexBuffer(graphics, typeof(VertexPositionTextureLight), 16 * 1000, BufferUsage.None);
                _buffer.SetData(Vertices);
                GS.EndMark("Chunk.Apply.AllocateVertextBuffer");
            }

            _graphics.SetVertexBuffer(_buffer);
            GS.EndMark("Chunk.Apply");
        }

        // TODO: Update this on modifications
        public Atlas Atlas;
        public Texture2D Texture {
            get {
                return Atlas.Texture;
            }
        }

        public static Vector3 ToRelative(Vector3 position) {
            return new Vector3((int)position.X >> 4, (int)position.Y >> 4, (int)position.Z >> 4);
        }
    }
}
