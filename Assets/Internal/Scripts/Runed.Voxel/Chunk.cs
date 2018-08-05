using System;
using Runed.Utilities;

namespace Runed.Voxel
{
    public sealed class Chunk
    {
        public static int Size = 16;

        private Block[,,] _blocks;

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

                return this._blocks[x, y, z];
            }
            set
            {
                if (x < 0 || x >= Chunk.Size || y < 0 || y >= Chunk.Size || z < 0 || z >= Chunk.Size)
                {
                    throw new ArgumentOutOfRangeException("Fix this to return the block in the world instance.");
                }

                this._blocks[x, y, z] = value;
            }
        }

        /// <summary>
        /// Gets the block at the given index.
        /// </summary>
        /// <param name="index">The index as a Vector3i.</param>
        /// <returns></returns>
        public Block this[Vector3i index] => this[(int)index.X, (int)index.Y, (int)index.Z];

    }
}