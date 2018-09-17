using FastNoise.SIMD;
using UnityEngine;

namespace Runed.Voxel
{
    public class TerrainGenerator
    {
        private int _seed = 100;
        private FastNoiseSIMD _noise;

        public TerrainGenerator(int seed)
        {
            this._seed = seed;

            this._noise = new FastNoiseSIMD(seed);
            this._noise.SetNoiseType(FastNoiseSIMD.NoiseType.SimplexFractal);
            //this._noise.SetInterp(FastNoise.Interp.Quintic);
            this._noise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
            this._noise.SetCellularDistanceFunction(FastNoiseSIMD.CellularDistanceFunction.Natural);
            this._noise.SetCellularJitter(0.45f);
            this._noise.SetCellularReturnType(FastNoiseSIMD.CellularReturnType.CellValue);
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
            C:\Users\jonat\Projects\DvZ\Assets\Internal\Scripts\Runed.Voxel\Mesh\MeshBuilder.cs            }
                        else
                        {
                            chunk[x, y, z] = new Block(BlockManager.GetBlock("air"));
                        }
                    }
                }
            }*/

            int index = 0;
            var noise = this._noise.GetNoiseSet(chunk.Position.x, chunk.Position.y, chunk.Position.z, Chunk.Size,
                Chunk.Size, Chunk.Size);

            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    // Pseudo function to process data in noise set
                    int g = 0;

                    //Debug.Log(s);

                    for (int y = 0; y < Chunk.Size; y++)
                    {
                        chunk[x, y, z] = new Block(BlockManager.GetBlock("air"));
                    }


                    if (chunk.Position.y % 2 != 0)
                    {
                        for (int y = 15; y > Mathf.Min(Mathf.Min(x, z), Chunk.Size); y--)
                        {
                            chunk[x, y, z] = new Block(BlockManager.GetBlock("blocktesttwo"));
                        }
                    }
                    else
                    {
                        for (int y = 0; y < Mathf.Min(Mathf.Max(x, z), Chunk.Size); y++)
                        {
                            chunk[x, y, z] = new Block(BlockManager.GetBlock("blocktesttwo"));
                        }
                    }


                }
            }
        }
    }
}