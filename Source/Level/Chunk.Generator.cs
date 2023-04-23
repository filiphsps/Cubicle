using Cubicle.Singletons;
using System;
using System.Linq;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public void Generate() {
            if (Position.Y != 0)
                return;

            for (var x = 0; x < SIZE; x++) {
                for (var y = 0; y < 6; y++) {
                    for (var z = 0; z < SIZE; z++) {
                        var pos = new Vector3(x, y, z);

                        if (y == 5 && (x == 0 || x == LAST) && (z == 0 || z == LAST)) {
                            Blocks.Add(pos + Vector3.UnitY, BlocksManager.GetBlock("border"));
                            continue;
                        }

                        if (y == 5)
                            Blocks.Add(pos, BlocksManager.GetBlock("grass"));
                        else if (y == 0)
                            Blocks.Add(pos, BlocksManager.GetBlock("border"));
                        else
                            Blocks.Add(pos, BlocksManager.GetBlock("dirt"));
                    }
                }
            }

            Atlas = TexturesManager.GetAtlas(Blocks.Values.Distinct().ToList());
            this.CalculateMesh();
        }

        public bool[] GetVisibleFaces(bool[] visibleFaces, Vector3 position) {
            // TODO: Cleanup
            // FIXME: Handle edges & top/bottom
            Array.Clear(visibleFaces, 0, 6);

            var x = position.X;
            var y = position.Y;
            var z = position.Z;

            if (z != LAST) {
                visibleFaces[0] = !Blocks.ContainsKey(new Vector3(x, y, z + 1));
            } else {
                visibleFaces[0] = true;
            }
            if (z != 0) {
                visibleFaces[1] = !Blocks.ContainsKey(new Vector3(x, y, z - 1));
            } else {
                visibleFaces[1] = true;
            }


            if (y != LAST) {
                visibleFaces[2] = !Blocks.ContainsKey(new Vector3(x, y + 1, z));
            } else {
                visibleFaces[2] = true;
            };
            if (y != 0) {
                visibleFaces[3] = !Blocks.ContainsKey(new Vector3(x, y - 1, z));
            } else {
                visibleFaces[3] = true;
            }


            if (x != LAST) {
                visibleFaces[4] = !Blocks.ContainsKey(new Vector3(x + 1, y, z));
            } else {
                visibleFaces[4] = true;
            };
            if (x != 0) {
                visibleFaces[5] = !Blocks.ContainsKey(new Vector3(x - 1, y, z));
            } else {
                visibleFaces[5] = true;
            }

            return visibleFaces;
        }
    }
}
