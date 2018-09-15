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

        // PRIVATE VARIABLES
        public Block[,,] Blocks { get; set; }

        public Chunk(World world, Vector3Int position)
        {
            this.World = world;
            this.World.AddChunk(this, this.Position);
            this.Blocks = new Block[Chunk.Size, Chunk.Size, Chunk.Size];
            this.Position = position;
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
                            if (y + 1 < Chunk.Size && !this[x, y + 1, z].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Up, Rect.zero );
                            }

                            if (y - 1 >= 0 && !this[x, y - 1, z].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Down, Rect.zero);
                            }

                            if (x + 1 < Chunk.Size && !this[x + 1, y, z].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Right, Rect.zero);
                            }

                            if (x - 1 >= 0 && !this[x - 1, y, z].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Left, Rect.zero);
                            }

                            if (z + 1 < Chunk.Size && !this[x, y, z + 1].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Forward, Rect.zero);
                            }

                            if (z - 1 >= 0 && !this[x, y, z - 1].Definition.Render)
                            {
                                meshData.AddQuad(new Vector3(x, y, z), BlockDirection.Back, Rect.zero);
                            }
                        }
                    }
                }
            }

            this.Dirty = false;

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