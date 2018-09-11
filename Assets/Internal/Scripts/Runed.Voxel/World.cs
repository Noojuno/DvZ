using System;
using System.Numerics;
using System.Security.Principal;
using System.Collections.Generic;

namespace Runed.Voxel
{
    public class World
    {
        public int Seed = 0;
        public TerrainGenerator TerrainGenerator;
        public Chunk[,,] Chunks;

        public string worldName = "world";
        

        public World(int seed)
        {
            this.Seed = seed;

            this.TerrainGenerator = new TerrainGenerator(this.Seed);
            this.Chunks = new Chunk[100000, 100000, 100000];
        }

        public void AddChunk(Chunk chunk, Vector3 position)
        {
            this.Chunks[(int) position.X, (int) position.Y, (int) position.Z] = chunk;
        }

        public void Update()
        {

        }


        public Block GetBlock(int x, int y, int z)
        {
            return this.GetBlock(new Vector3(x, y, z));
        }

        public Block GetBlock(Vector3 worldPos)
        {
            int chunkX = (int)worldPos.X % 16;

            return this.Chunks[0, 0, 0][0, 0, 0];
        }

    }
}