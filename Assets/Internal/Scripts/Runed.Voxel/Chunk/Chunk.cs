using System;
using UnityEngine;

namespace Runed.Voxel
{
    public class Chunk
    {
        // STATIC VARIABLES
        public static int Size = 16;

        // PUBLIC VARIABLES
        public World World;
        public Vector3Int Position;

        public bool Dirty = true;
        public bool Loaded = false;
        public bool Rendered = false;

        // PRIVATE VARIABLES
        public Block[,,] Blocks { get; set; }

        public Chunk(World world, Vector3Int position)
        {
            this.World = world;
            this.Position = position;
            this.World.AddChunk(this);
            this.Blocks = new Block[Chunk.Size, Chunk.Size, Chunk.Size];
        }

        public void RebuildAdjacentChunks()
        {
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (this.World.GetAdjacentChunk(this.Position, direction) != null)
                {
                    this.World.GetAdjacentChunk(this.Position, direction).Dirty = true;

                }
            }
        }

        /// <summary>
        /// Gets the block at the given index.
        /// </summary>
        /// <param name="x">The x coordinate of the block</param>
        /// <param name="y">The y coordinate of the block</param>
        /// <param name="z">The z coordinate of the block</param>
        /// <returns></returns>
        public Block this[int x, int y, int z]
        {
            get
            {
                if (x < 0 || x >= Chunk.Size || y < 0 || y >= Chunk.Size || z < 0 || z >= Chunk.Size)
                {
                    throw new ArgumentOutOfRangeException("Fix this to return the block in the world instance.");
                }

                return this.Blocks[x, y, z];
            }
            set
            {
                if (x < 0 || x >= Chunk.Size || y < 0 || y >= Chunk.Size || z < 0 || z >= Chunk.Size)
                {
                    throw new ArgumentOutOfRangeException("Fix this to return the block in the world instance.");
                }

                this.Blocks[x, y, z] = value;

                if (x == Chunk.Size - 1 || y == Chunk.Size - 1 || z == Chunk.Size - 1)
                {
                    this.RebuildAdjacentChunks();
                }

                this.Dirty = true;
            }
        }

        /// <summary>
        /// Gets the block at the given index.
        /// </summary>
        /// <param name="index">The index as a Vector3i.</param>
        /// <returns></returns>
        public Block this[Vector3 index] => this[(int)index.x, (int)index.y, (int)index.z];

        /// <summary>
        /// Gets the block at the given index.
        /// </summary>
        /// <param name="index">The index as a Vector3i.</param>
        /// <returns></returns>
        public Block this[Vector3Int index] => this[index.x, index.y, index.z];

    }
}