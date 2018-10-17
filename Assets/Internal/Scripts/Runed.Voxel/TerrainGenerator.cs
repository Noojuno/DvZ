using UnityEngine;
using System;
using FastNoiseC;
using Random = UnityEngine.Random;

namespace Runed.Voxel
{
    public class TerrainGenerator
    {
        private int _seed = 100;
        private FastNoiseC.FastNoise _noise;

        protected int interpBitStep;
        protected float interpScale;
        protected int interpSize;
        protected int interpSizeSq;
        protected float maxHeight = float.MaxValue;
        protected float minHeight = float.MinValue;
        public float terrainScale = 20f;
        public float canyonMaxHeight = 10f;
        public float canyonGradient = 2f;

        public TerrainGenerator(int seed)
        {
            this._seed = seed;

            this._noise = new FastNoiseC.FastNoise(seed);
            this._noise.SetNoiseType(FastNoiseC.FastNoise.NoiseType.Cellular);
            this._noise.SetInterp(FastNoiseC.FastNoise.Interp.Quintic);
            this._noise.SetFractalType(FastNoiseC.FastNoise.FractalType.FBM);
            this._noise.SetCellularDistanceFunction(FastNoiseC.FastNoise.CellularDistanceFunction.Natural);
            this._noise.SetCellularJitter(0.45f);
            this._noise.SetCellularReturnType(FastNoiseC.FastNoise.CellularReturnType.CellValue);
            this._noise.SetFrequency(1f);
            this._noise.SetFractalGain(1f);
            this._noise.SetFractalLacunarity(1f);
            this._noise.SetFractalOctaves(5);

            this.SetInterpBitStep(1);

            minHeight = -terrainScale;
            maxHeight = terrainScale;
        }

        public void SetSeed(int seed)
        {
            this._seed = seed;
            this._noise.SetSeed(this._seed);
        }

        protected void SetNoiseArraySize(int size)
        {
            //System.Array.Resize(ref _noise, size);
        }

        public void Generate(Chunk chunk)
        {
            float[] interpLookup = new float[interpSize * interpSize * interpSize];

            int xOffset = chunk.Position.x << 4;
            int yOffset = chunk.Position.y << 4;
            int zOffset = chunk.Position.z << 4;
            int index = 0;

            for (int x = 0; x < interpSize; x++)
            {
                float xf = (x << interpBitStep) + xOffset;

                for (int y = 0; y < interpSize; y++)
                {
                    float yf = (y << interpBitStep) + yOffset;

                    for (int z = 0; z < interpSize; z++)
                    {
                        float zf = (z << interpBitStep) + zOffset;

                        float voxel = -yf;
                        voxel += (float) this._noise.GetNoise(xf, yf, zf) * terrainScale;

                        interpLookup[index++] = voxel;
                    }
                }
            }

            index = 0;

            var air = new Block(BlockDefinition.Air);
            var test = new Block(BlockManager.GetBlock("test"));
            var testtwo = new Block(BlockManager.GetBlock("blocktesttwo"));

            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    for (int z = 0; z < Chunk.Size; z++)
                    {
                        if (y % 4 != 0)
                        {
                            if (x % 2 == 0)
                            {
                                chunk[x, y, z, false] = testtwo;
                            }
                            else
                            {
                                chunk[x, y, z, false] = test;
                            }
                        }
                        else
                        {
                            chunk[x, y, z, false] = air;
                        }
                    }
                }
            }

            chunk.Dirty = true;
            chunk.Loaded = true;
        }

        protected float[] GetInterpNoise(int noiseArrayIndex, Vector3Int chunkPos)
        {
            int offsetShift = Chunk.Size - interpBitStep;

            return new[] {0f}; //this._noise.GetNoise(chunkPos.x << offsetShift,
            //chunkPos.y << offsetShift, chunkPos.z << offsetShift, interpSize, interpSize, interpSize,
            //1 << interpBitStep);
        }

        protected void SetInterpBitStep(int interpBitStep)
        {
            this.interpBitStep = interpBitStep;
            this.interpSize = (Chunk.Size >> interpBitStep) + 1;
            this.interpSizeSq = this.interpSize * this.interpSize;
            this.interpScale = 1f / (1 << interpBitStep);
        }

        protected float VoxelInterpLookup(int localX, int localY, int localZ, float[] interpLookup)
        {
            var xs = (localX + 0.5f) * this.interpScale;
            var ys = (localY + 0.5f) * this.interpScale;
            var zs = (localZ + 0.5f) * this.interpScale;

            var x0 = FastFloor(xs);
            var y0 = FastFloor(ys);
            var z0 = FastFloor(zs);

            xs = xs - x0;
            ys = ys - y0;
            zs = zs - z0;

            var lookupIndex = this.InterpLookupIndex(x0, y0, z0);

            return Lerp(Lerp(
                Lerp(interpLookup[lookupIndex], interpLookup[lookupIndex + this.interpSizeSq], xs),
                Lerp(interpLookup[lookupIndex + this.interpSize],
                    interpLookup[lookupIndex + this.interpSizeSq + this.interpSize], xs),
                ys), Lerp(
                Lerp(interpLookup[++lookupIndex], interpLookup[lookupIndex + this.interpSizeSq], xs),
                Lerp(interpLookup[lookupIndex + this.interpSize],
                    interpLookup[lookupIndex + this.interpSizeSq + this.interpSize], xs),
                ys), zs);
        }

        protected int InterpLookupIndex(int interpX, int interpY, int interpZ)
        {
            return interpZ + interpY * this.interpSize + interpX * this.interpSizeSq;
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        private static int FastFloor(float f)
        {
            return f >= 0.0f ? (int) f : (int) f - 1;
        }
    }
}