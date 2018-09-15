using FastNoiseC;
using UnityEngine;

namespace Runed.Voxel
{
    public class TerrainGenerator
    {
        private int _seed = 100;
        private FastNoise _noise;

        public TerrainGenerator(int seed)
        {
            this._seed = seed;

            this._noise = new FastNoise(seed);
            this._noise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
            this._noise.SetInterp(FastNoise.Interp.Quintic);
            this._noise.SetFractalType(FastNoise.FractalType.FBM);
            this._noise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Natural);
            this._noise.SetCellularJitter(0.45f);
            this._noise.SetCellularReturnType(FastNoise.CellularReturnType.CellValue);
            this._noise.SetFrequency(1f);
            this._noise.SetFractalGain(1f);
            this._noise.SetFractalLacunarity(1f);
            this._noise.SetFractalOctaves(5);
        }

        public void Generate(Chunk chunk)
        {
            /* for (int z = 0; z < Chunk.Size; z++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    for (int x = 0; x < Chunk.Size; x++)
                    {
                        if (Mathf.Sqrt((float)(x - Chunk.Size / 2) * (x - Chunk.Size / 2) + (y - Chunk.Size / 2) * (y - Chunk.Size / 2) + (z - Chunk.Size / 2) * (z - Chunk.Size / 2)) <= Chunk.Size / 2)
                        {
                            chunk[x, y, z] = new Block(BlockManager.GetBlock(1));
                        }
                        else
                        {
                            chunk[x, y, z] = new Block(BlockManager.GetBlock("air"));
                        }
                    }
                }
            }*/

            int index = 0;

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    // Pseudo function to process data in noise set

                    var s = Mathf.Min(x * z, 15);

                    Debug.Log(s);

                    for (int y = 0; y < Chunk.Size; y++)
                    {
                        chunk[x, y, z] = new Block(BlockManager.GetBlock("air"));
                    }

                    for (int y = 0; y < s; y++)
                    {
                        chunk[x, y, z] = new Block(BlockManager.GetBlock(1));
                    }

                }
            }

        }
    }
}
