using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cubicle.Singletons {
    // FIXME: Move to util class
    // https://stackoverflow.com/a/14663233/3142553
    public class IdsArrayComparer : IEqualityComparer<int[]> {
        public bool Equals(int[] x, int[] y) {
            if (x.Length != y.Length) {
                return false;
            }
            for (int i = 0; i < x.Length; i++) {
                if (x[i] != y[i]) {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj) {
            int result = 17;
            for (int i = 0; i < obj.Length; i++) {
                unchecked {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }

    public struct Atlas {
        public Texture2D Texture;
        public int TexturesCount;
        public Dictionary<int, ushort> BlockIndices;
    }

    public static class TexturesManager {
        private static GraphicsDevice _graphics;
        private static Dictionary<String, Texture2D> _blockTextures;
        private static Dictionary<int[], Atlas> _chunkAtlas;

        static TexturesManager() {
            _blockTextures = new Dictionary<String, Texture2D>();
            _chunkAtlas = new Dictionary<int[], Atlas>(new IdsArrayComparer());
        }

        public static void LoadContent(GraphicsDevice graphics) {
            _graphics = graphics;

            var textures = Directory.GetFiles("Assets/Textures/Blocks", ".").ToArray();

            foreach (var texture in textures) {
                var key = texture.Split('\\')[1].Split('.')[0];

                FileStream fileStream = new FileStream(texture, FileMode.Open);
                _blockTextures.Add(key, Texture2D.FromStream(_graphics, fileStream));
                fileStream.Dispose();
            }
        }

        public static Atlas GetAtlas(List<int> blockIds) {
            blockIds.Sort();
            var ids = blockIds.ToArray();

            if (_chunkAtlas.ContainsKey(ids))
                return _chunkAtlas[ids];

            Console.WriteLine($"TextureAtlas(${_chunkAtlas.Count}): MISS - [{String.Join(";", ids)}]");

            // TODO: Dynamic size
            var texture = new Texture2D(_graphics, 8, ids.Length * 8);
            Color[] texture_data = new Color[texture.Width * texture.Height];

            var block_indices = new Dictionary<int, ushort>();

            // TODO: handle duplicate textures
            for (ushort i = 0; i < ids.Length; i++) {
                var id = ids[i];
                var name = BlocksManager.GetTextureName(id);

                var block_text = _blockTextures[name];
                Color[] color = new Color[block_text.Width * block_text.Height];
                block_text.GetData(color);

                for (int j = 0; j < color.Length; j++) {
                    texture_data[(i * color.Length) + j] = color[j];
                }

                block_indices.Add(id, i);
            }
            texture.SetData(texture_data);

            _chunkAtlas.Add(ids, new Atlas() with {
                Texture = texture,
                TexturesCount = ids.Length,
                BlockIndices = block_indices
            });
            return _chunkAtlas[ids];
        }
    }
}
