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

                        if (chunk[x, y, z].Definition.Render)
                        {
                            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                            {
                                var adjacentBlock = chunk.World.GetAdjacentBlock(offsetPosition, direction);

                                if (adjacentBlock.Definition == null ||
                                    adjacentBlock.Definition == BlockDefinition.Air ||
                                    !adjacentBlock.Definition.Render ||
                                    adjacentBlock.Definition.HasCustomModel ||
                                    adjacentBlock.Definition.Translucent &&
                                    adjacentBlock.Definition.Identifier != chunk[x, y, z].Definition.Identifier)
                                {
                                    meshData.AddQuad(new Vector3(x, y, z), 0, direction, 1);
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