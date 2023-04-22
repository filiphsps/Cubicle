using Cubicle.Singletons;
using System;
using System.Collections.Generic;
using System.Linq;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public void Generate() {
            if (Position.Y != 0)
                return;

            for (var x = 0; x < 16; x++) {
                for (var y = 0; y < 6; y++) {
                    for (var z = 0; z < 16; z++) {
                        var pos = new Vector3(x, y, z);

                        if (y == 5)
                            Blocks.Add(pos, new BlockReference("grass") { Position = pos });
                        else if (y == 0)
                            Blocks.Add(pos, new BlockReference("border") { Position = pos });
                        else
                            Blocks.Add(pos, new BlockReference("dirt") { Position = pos });
                    }
                }
            }

            Atlas = TexturesManager.GetAtlas(Blocks.Values.Select(x => x.Id).Distinct().ToList());
            this.CalculateMesh();
        }

        public bool[] GetVisibleFaces(bool[] visibleFaces, BlockReference block) {
            Array.Clear(visibleFaces, 0, 6);

            var x = block.Position.X;
            var y = block.Position.Y;
            var z = block.Position.Z;

            BlockReference? adjacent = null;

            if (z != LAST) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x, y, z + 1));
                visibleFaces[0] = adjacent is null;
            }
            else {
                visibleFaces[0] = true;
            }

            if (z != 0) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x, y, z - 1));
                visibleFaces[1] = adjacent is null;
            }
            else {
                visibleFaces[1] = true;
            }


            if (y != LAST) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x, y + 1, z));
                visibleFaces[2] = adjacent is null;
            }
            else {
                visibleFaces[2] = true;
            };

            if (x != 0) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x, y - 1, z));
                visibleFaces[3] = adjacent is null;
            }
            else {
                visibleFaces[3] = true;
            }


            if (x != LAST) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x + 1, y, z));
                visibleFaces[4] = adjacent is null;
            }
            else {
                visibleFaces[4] = true;
            };

            if (x != 0) {
                adjacent = Blocks.GetValueOrDefault(new Vector3(x - 1, y, z));
                visibleFaces[5] = adjacent is null;
            }
            else {
                visibleFaces[5] = true;
            }

            return visibleFaces;
        }
    }
}
