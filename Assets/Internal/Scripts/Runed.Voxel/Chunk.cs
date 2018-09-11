using System;
using UnityEngine;
using Unity.Entities;

namespace Runed.Voxel
{
    public class Chunk
    {
        // STATIC VARIABLES
        public static int Size = 16;

        // PUBLIC VARIABLES
        public World World;
        public bool Dirty = true;

        // PRIVATE VARIABLES
        public Block[,,] Blocks { get; set; }

        public Chunk(World world)
        {
            this.World = world;
            this.Blocks = new Block[Chunk.Size, Chunk.Size, Chunk.Size];
        }

        public MeshData ToMeshData()
        {
            var meshData = new MeshData();

            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    for (int z = 0; z < Chunk.Size; z++)
                    {
                        if (this[x, y, z] != null && this[x, y, z].Definition.Render)
                        {
                            if (y + 1 < Chunk.Size && this[x, y + 1, z].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Up, Rect.zero );
                            }

                            if (y - 1 >= 0 && this[x, y - 1, z].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Down, Rect.zero);
                            }

                            if (x + 1 < Chunk.Size && this[x + 1, y, z].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Right, Rect.zero);
                            }

                            if (x - 1 >= 0 && this[x - 1, y, z].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Left, Rect.zero);
                            }

                            if (z + 1 < Chunk.Size && this[x, y, z + 1].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Forward, Rect.zero);
                            }

                            if (z - 1 >= 0 && this[x, y, z - 1].Definition == BlockManager.GetBlock("air"))
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Back, Rect.zero);
                            }
                        }
                    }
                }
            }

            return meshData;
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
            }
        }

        /// <summary>
        /// Gets the block at the given index.
        /// </summary>
        /// <param name="index">The index as a Vector3i.</param>
        /// <returns></returns>
        public Block this[Vector3 index] => this[(int)index.x, (int)index.y, (int)index.z];

    }
}