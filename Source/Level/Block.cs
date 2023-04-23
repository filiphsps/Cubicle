using System;
using System.Numerics;

namespace Cubicle.Level {
    public class Block {
        public String Name;

        // "front", "back", "top", "bottom", "right", "left"
        public String[] TextureNames;

        public static Vector3 ToRelative(Vector3 position) {
            return new Vector3((int)position.X % 16, (int)position.Y % 16, (int)position.Z % 16);
        }
    }
}
