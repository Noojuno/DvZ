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
            int chunkX = Mathf.FloorToInt(worldPos.x / Chunk.Size);
            int chunkY = Mathf.FloorToInt(worldPos.y / Chunk.Size);
            int chunkZ = Mathf.FloorToInt(worldPos.z / Chunk.Size);

            Vector3Int chunkPos = new Vector3Int(chunkX, chunkY, chunkZ);

            int blockX = Mathf.Abs(worldPos.x - (chunkX * Chunk.Size));
            int blockY = Mathf.Abs(worldPos.y - (chunkY * Chunk.Size));
            int blockZ = Mathf.Abs(worldPos.z - (chunkZ * Chunk.Size));

            Vector3Int blockPos = new Vector3Int(blockX, blockY, blockZ);

            return this._chunks[chunkPos][blockPos];
        }


        public void Update()
        {

        }

    }
}