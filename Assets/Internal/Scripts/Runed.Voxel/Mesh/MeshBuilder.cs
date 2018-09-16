using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public class MeshBuilder
    {
        public static MeshData BuildChunk(Chunk chunk)
        {
            var meshData = new MeshData();

            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    for (int z = 0; z < Chunk.Size; z++)
                    {
                        var offsetPosition = new Vector3Int(x, y, z) + (chunk.Position * Chunk.Size);

                        if (chunk[x, y, z] != null && chunk[x, y, z].Definition.Render)
                        {
                            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                            {
                                if (chunk.World.GetAdjacentBlock(offsetPosition, direction) == null ||
                                    !chunk.World.GetAdjacentBlock(offsetPosition, direction).Definition.Render)
                                {
                                    meshData.AddQuad(new Vector3(x, y, z), direction, new Rect(0, 0, 1, 1));
                                }
                            }
                        }
                    }
                }
            }

            chunk.Dirty = false;

            return meshData;
        }
    }
}