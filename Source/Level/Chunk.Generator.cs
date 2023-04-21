﻿using System;
using System.Collections.Generic;
using Vector3 = System.Numerics.Vector3;

namespace Cubicle.Level {
    public sealed partial class Chunk {
        public void Generate() {
            var size = Position.Y == 0 ? 16 : 1;
            for (var x = 0; x < size; x++) {
                for (var y = 0; y < 1; y++) {
                    for (var z = 0; z < size; z++) {
                        Blocks.Add(new Vector3(x, y, z), new Block() { Position = new Vector3(x, y, z) });
                    }
                }
            }

            this.CalculateMesh();
        }

        public bool[] GetVisibleFaces(bool[] visibleFaces, Block block) {
            Array.Clear(visibleFaces, 0, 6);

            var x = block.Position.X;
            var y = block.Position.Y;
            var z = block.Position.Z;

            Block? adjacentBlock = null;

            if (z != LAST) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x, y, z + 1));
                visibleFaces[0] = adjacentBlock is null;
            }
            else {
                visibleFaces[0] = true;
            }

            if (z != 0) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x, y, z - 1));
                visibleFaces[1] = adjacentBlock is null;
            }
            else {
                visibleFaces[1] = true;
            }


            if (y != LAST) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x, y + 1, z));
                visibleFaces[2] = adjacentBlock is null;
            }
            else {
                visibleFaces[2] = true;
            };

            if (x != 0) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x, y - 1, z));
                visibleFaces[3] = adjacentBlock is null;
            }
            else {
                visibleFaces[3] = true;
            }


            if (x != LAST) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x + 1, y, z));
                visibleFaces[4] = adjacentBlock is null;
            }
            else {
                visibleFaces[4] = true;
            };

            if (x != 0) {
                adjacentBlock = Blocks.GetValueOrDefault(new Vector3(x - 1, y, z));
                visibleFaces[5] = adjacentBlock is null;
            }
            else {
                visibleFaces[5] = true;
            }

            return visibleFaces;
        }
    }
}
