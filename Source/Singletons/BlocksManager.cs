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
            Add(new Block() { Name = "dirt", TextureName = "dirt" });
            Add(new Block() { Name = "grass", TextureName = "grass_top" });
        }

        private static void Add(Block block) {
            BlockNameMap.Add(block.Name);
            int id = BlockNameMap.IndexOf(block.Name);

            BlockMap.Add(id, block);
        }

        public static int GetBlock(String name) {
            return BlockNameMap.IndexOf(name);
        }
        public static String GetTextureName(int id) {
            return BlockMap[id].TextureName;
        }
    }
}
