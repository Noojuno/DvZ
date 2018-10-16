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
            var meshData = new MeshData(2);

            for (int x = 0; x < Chunk.Size; x++)
            {
                for (int y = 0; y < Chunk.Size; y++)
                {
                    for (int z = 0; z < Chunk.Size; z++)
                    {
                        var offsetPosition = new Vector3Int(x, y, z) + (chunk.Position * Chunk.Size);
                        var block = chunk[x, y, z];

                        if (block.Definition.Render)
                        {
                            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                            {
                                var adjacentBlock = chunk.World.GetAdjacentBlock(offsetPosition, direction);
                                var o = offsetPosition.AdjustByDirection(direction);

                                //if (chunk.Position == Vector3Int.zero && adjacentBlock.Definition.Translucent) Debug.Log($"B: d {block.Definition} o {offsetPosition} p {x} {y} {z} direction {direction} A: d {adjacentBlock.Definition} o {o} {adjacentBlock.Definition.Translucent}");

                                if (!adjacentBlock.Definition.Render || !adjacentBlock.Definition.Translucent && adjacentBlock.Definition.Identifier != block.Definition.Identifier)
                                {
                                    int meshLayer = block.Definition.Translucent ? 1 : 0;

                                    meshData.AddQuad(new Vector3(x, y, z), meshLayer, direction, block.Definition.GetTexture(direction).Layer);
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