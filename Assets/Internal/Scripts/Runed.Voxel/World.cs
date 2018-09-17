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
        public readonly Dictionary<Vector3Int, Chunk> _chunks;

        public World(int seed)
        {
            this.Seed = seed;

            this.TerrainGenerator = new TerrainGenerator(this.Seed);
            this._chunks = new Dictionary<Vector3Int, Chunk>();
        }

        public void AddChunk(Chunk chunk)
        {
            this._chunks[chunk.Position] = chunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            return this.GetBlock(new Vector3Int(x, y, z));
        }

        public Block GetBlock(Vector3Int worldPos)
        {
            int chunkX = worldPos.x >> (int)Mathf.Sqrt(Chunk.Size);
            int chunkY = worldPos.y >> (int)Mathf.Sqrt(Chunk.Size);
            int chunkZ = worldPos.z >> (int)Mathf.Sqrt(Chunk.Size);

            Vector3Int chunkPos = new Vector3Int(chunkX, chunkY, chunkZ);


            if (!this.ChunkExistsAt(chunkPos)) return new Block(BlockManager.GetBlock("air"));

            int blockX = Mathf.Abs(worldPos.x - (chunkX * Chunk.Size));
            int blockY = Mathf.Abs(worldPos.y - (chunkY * Chunk.Size));
            int blockZ = Mathf.Abs(worldPos.z - (chunkZ * Chunk.Size));

            Vector3Int blockPos = new Vector3Int(blockX, blockY, blockZ);


            if (blockX >= Chunk.Size || blockY >= Chunk.Size || blockZ >= Chunk.Size) return new Block(BlockManager.GetBlock("air"));

            return this._chunks[chunkPos][blockPos];
        }

        public Chunk GetChunk(Vector3Int position)
        {
            if (this._chunks.ContainsKey(position))
            {
                return this._chunks[position];
            }

            return null;
        }

        public Chunk GetAdjacentChunk(Vector3Int position, Direction direction)
        {
            return this.GetChunk(position.AdjustByDirection(direction));
        }

        public Block GetAdjacentBlock(Vector3Int position, Direction direction)
        {
            return this.GetBlock(position.AdjustByDirection(direction));
        }

        public string DumpChunks()
        {
            var s = "";

            foreach (var chunk in this._chunks.Keys)
            {
                s += chunk.ToString() + ", ";
            }

            return s;
        }

        public bool ChunkExistsAt(Vector3Int position)
        {
            return this._chunks.ContainsKey(position);
        }

        public bool HasAdjacentChunk(Vector3Int position, Direction direction)
        {
            return false;
        }

        public void SetSeed(int seed)
        {
            this.Seed = seed;
            this.TerrainGenerator.SetSeed(seed);
        }

        public void Update()
        {

        }

    }
}