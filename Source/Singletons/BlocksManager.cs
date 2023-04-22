using Cubicle.Level;
using System;
using System.Collections.Generic;

namespace Cubicle.Singletons {
    public static class BlocksManager {
        // TODO: uint?
        private static Dictionary<int, Block> BlockMap;
        private static List<String> BlockNameMap;

        static BlocksManager() {
            BlockMap = new Dictionary<int, Block>();
            BlockNameMap = new List<String>();
        }

        public static void LoadContent() {
            Add(new Block() { Name = "dirt", TextureNames = new String[] { "dirt" } });
            Add(new Block() { Name = "grass", TextureNames = new String[] { "grass_side", "grass_top", "dirt" } });
            Add(new Block() { Name = "border", TextureNames = new String[] { "border" } });
        }
        private static void Add(Block block) {
            BlockNameMap.Add(block.Name);
            int id = BlockNameMap.IndexOf(block.Name);
            BlockMap.Add(id, block);
        }

        public static int GetBlock(String name) {
            return BlockNameMap.IndexOf(name);
        }
        public static String[] GetTextureNames(int id) {
            return BlockMap[id].TextureNames;
        }
    }
}
