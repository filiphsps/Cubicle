using Cubicle.Gearset;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
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
        public Dictionary<int, ushort[]> BlockIndices;
    }

    public static class TexturesManager {
        private static GraphicsDevice _graphics;
        public static ConcurrentDictionary<String, Texture2D> BlockTextures;
        public static ConcurrentDictionary<int[], Atlas> ChunkAtlas;

        static TexturesManager() {
            BlockTextures = new ConcurrentDictionary<String, Texture2D>();
            ChunkAtlas = new ConcurrentDictionary<int[], Atlas>(new IdsArrayComparer());
        }

        public static void LoadContent(GraphicsDevice graphics) {
            _graphics = graphics;

            var textures = Directory.GetFiles("Assets/Textures/Blocks", ".").ToArray();

            foreach (var texture in textures) {
                var key = texture.Split('\\')[1].Split('.')[0];

                FileStream stream = new FileStream(texture, FileMode.Open);
                // FIXME: Error handling
                // TODO: Move to FromFile
                BlockTextures.TryAdd(key, Texture2D.FromStream(_graphics, stream));
                stream.Dispose();
            }

            // FIXME: Remove this when GetAtlas supports multithreading,
            //  or we have a good work-around. Texture2D is a blocker.
            GetAtlas(new List<int>() {
                BlocksManager.GetBlock("grass"),
                BlocksManager.GetBlock("dirt"),
                BlocksManager.GetBlock("border")
            });
            GetAtlas(new List<int>() {
                BlocksManager.GetBlock("grass"),
                BlocksManager.GetBlock("dirt")
            });
            GetAtlas(new List<int>() {
                BlocksManager.GetBlock("dirt"),
                BlocksManager.GetBlock("border")
            });
            GetAtlas(new List<int>() {
                BlocksManager.GetBlock("dirt"),
            });
        }

        public static Atlas GetAtlas(List<int> blockIds) {
            blockIds.Sort();
            var ids = blockIds.ToArray();

            if (ids.Length <= 0)
                throw new Exception("blockIds can't be empty!");

            if (ChunkAtlas.ContainsKey(ids))
                return ChunkAtlas[ids];

            GS.Log("TexturesManager", $"GetAtlas: MISS - [{String.Join(";", ids)}]");

            // Figure out which textures to add to the atlas
            // Some blocks may have more than one texture (multiface)
            var textures_to_add = new List<String>();
            for (ushort i = 0; i < ids.Length; i++) {
                var names = BlocksManager.GetTextureNames(ids[i]);
                foreach (var name in names) {
                    if (textures_to_add.Contains(name))
                        continue;

                    textures_to_add.Add(name);
                }
            }

            // TODO: Multiple widths & heights
            var texture = new Texture2D(_graphics, 8, textures_to_add.Count * 8);
            Color[] texture_data = new Color[texture.Width * texture.Height];

            // Draw texture atlas
            var texture_indices = new Dictionary<String, ushort>();
            for (ushort i = 0; i < textures_to_add.Count; i++) {
                var name = textures_to_add[i];
                var block_texture = BlockTextures[name];

                Color[] color = new Color[block_texture.Width * block_texture.Height];
                block_texture.GetData(color);
                for (int j = 0; j < color.Length; j++) {
                    texture_data[(i * color.Length) + j] = color[j];
                }

                texture_indices.Add(name, i);
            }
            texture.SetData(texture_data);

            // Create the final array
            var block_indices = new Dictionary<int, ushort[]>();
            for (ushort i = 0; i < ids.Length; i++) {
                var id = ids[i];
                var names = BlocksManager.GetTextureNames(id);

                var f = names.Select(x => texture_indices[x]).ToList();
                switch (f.Count) {
                    case 6:
                        block_indices.Add(id, new ushort[6] { f[0], f[1], f[2], f[3], f[4], f[5] });
                        break;
                    case 3:
                        block_indices.Add(id, new ushort[6] { f[0], f[0], f[1], f[2], f[0], f[0] });
                        break;
                    case 1:
                        block_indices.Add(id, new ushort[6] { f[0], f[0], f[0], f[0], f[0], f[0] });
                        break;
                    default:
                        throw new Exception("");
                }
            }

            // FIXME: Error handling
            ChunkAtlas.TryAdd(ids, new Atlas() with {
                Texture = texture,
                TexturesCount = texture_indices.Count,
                BlockIndices = block_indices
            });
            return ChunkAtlas[ids];
        }
    }
}
