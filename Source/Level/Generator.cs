using Cubicle.Singletons;
using System;
using System.Numerics;

namespace Cubicle.Level {
    public static class Generator {
        private static Random _random;
        private static FastNoiseLite _noise;
        private static FastNoiseLite _terrain;

        enum Biome : byte {
            Fields
        }

        static Generator() {
            int seed = 1337;

            _random = new Random(seed);

            _noise = new FastNoiseLite(seed);
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            _noise.SetFrequency(0.001f);

            _terrain = new FastNoiseLite(seed);
            _terrain.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
            _terrain.SetFractalType(FastNoiseLite.FractalType.Ridged);
        }

        public static void Generate(Chunk chunk) {
            int[,] elevation_map = new int[Chunk.SIZE, Chunk.SIZE];
            Biome[,] biome_data = new Biome[Chunk.SIZE, Chunk.SIZE];

            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    elevation_map[x, z] = Height(chunk.Position, x, z, out Biome biome);
                    biome_data[x, z] = biome;
                }
            }

            var sy = (int)chunk.Position.Y << 4;
            for (int x = 0; x < Chunk.SIZE; x++) {
                for (int z = 0; z < Chunk.SIZE; z++) {
                    for (int y = 0; y < Chunk.SIZE; y++) {

                        if (sy + y >= elevation_map[x, z]) {
                            break;
                        }

                        int block = BlockType(elevation_map[x, z], sy + y, biome_data[x, z]);
                        chunk[x, y, z] = block;
                    }
                }
            }
        }

        private static int BlockType(int max_y, int y, Biome biome) {
            if (y == 0) {
                return BlocksManager.GetBlock("border");
            }

            switch (biome) {
                case Biome.Fields:
                    if (y == max_y - 1) {
                        return BlocksManager.GetBlock("grass");
                    }

                    return BlocksManager.GetBlock("dirt");
            }

            // Error?
            return 0;
        }

        private static int Height(Vector3 position, int x, int z, out Biome biome) {
            float xVal = x + ((int)position.X << 4);
            float zVal = z + ((int)position.Z << 4);

            //float noise = Math.Abs(_noise.GetNoise(xVal, zVal) + 0.5f);

            int height = (int)Math.Abs(6 * (_terrain.GetNoise(xVal, zVal) + 0.8f)) + 30;
            biome = Biome.Fields;

            return height;
        }
    }
}
