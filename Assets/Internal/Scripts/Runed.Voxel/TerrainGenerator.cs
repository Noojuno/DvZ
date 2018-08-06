using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //this._noise.SetInterp(FastNoiseSIMD.Interp.Quintic);
            this._noise.SetFractalType(FastNoiseSIMD.FractalType.FBM);
            this._noise.SetCellularDistanceFunction(FastNoiseSIMD.CellularDistanceFunction.Natural);
            this._noise.SetCellularJitter(0.45f);
            this._noise.SetCellularReturnType(FastNoiseSIMD.CellularReturnType.CellValue);
            this._noise.SetFrequency(1f);
            this._noise.SetFractalGain(1f);
            this._noise.SetFractalLacunarity(1f);
            this._noise.SetFractalOctaves(5);
        }

        public void Generate(int x, int y, int z)
        {
            this.Generate(new Vector3(x, y, z));
        }

        public void Generate(Vector3 position)
        {

        }
    }
}
