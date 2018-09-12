using System;
using System.Security.Principal;
using System.Collections.Generic;
using UnityEngine;

namespace Runed.Voxel
{
    public class World
    {
        public int Seed = 0;
        public string Name = "world";
        
        // CONFIG
        //TODO: WorldConfig class?
        public static int MaxX = 100;
        public static int MaxY = 100;
        public static int MaxZ = 100;

        public static int MinX = -100;
        public static int MinY = -100;
        public static int MinZ = -100;

        public TerrainGenerator TerrainGenerator;
        private readonly Dictionary<Vector3Int, Chunk> _chunks;

        public World(int seed)
        {
            this.Seed = seed;

            this.TerrainGenerator = new TerrainGenerator(this.Seed);
            this._chunks = new Dictionary<Vector3Int, Chunk>();
        }

        public void AddChunk(Chunk chunk, Vector3Int position)
        {
            this._chunks[position] = chunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return this.GetBlock(new Vector3Int(x, y, z));
        }

        public Block GetBlock(Vector3Int worldPos)
        {
            int chunkX = (int)worldPos.x % 16;

            return this._chunks[worldPos][0, 0, 0];
        }

        public void LoadChunk(Vector3Int position)
        {

        }

        public void Update()
        {

        }

    }
}