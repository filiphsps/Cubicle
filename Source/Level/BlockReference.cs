using Cubicle.Singletons;
using System;
using System.Numerics;

namespace Cubicle.Level {
    public class BlockReference {
        public int Id;
        public Vector3 Position;

        public BlockReference(String name) {
            Id = BlocksManager.GetBlock(name);
        }
    }
}
