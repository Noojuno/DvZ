﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Profiling;

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

                                if (!adjacentBlock.Definition.Render || !adjacentBlock.Definition.Translucent &&
                                    adjacentBlock.Definition.Identifier != block.Definition.Identifier)
                                {
                                    int meshLayer = block.Definition.Translucent ? 1 : 0;

                                    meshData.AddQuad(new Vector3(x, y, z), 0, direction,
                                        block.Definition.GetTexture(direction).Layer);
                                }
                            }
                        }
                    }
                }
            }

            chunk.Dirty = false;
            chunk.Rendered = true;

            return meshData;
        }


        public static MeshData CreateGreedyMesh(Chunk chunk)
        {
            var meshData = new MeshData();

            int size = Chunk.Size;

            var world = chunk.World;
            var offset = chunk.Position * Chunk.Size;

            Block block1;
            Block block2;

            for (bool backFace = true, b = false; b != backFace; backFace = backFace && b, b = !b)
            {
                for (int d = 0; d < 3; d++)
                {
                    int i, j, k, l, w, h, u = (d + 1) % 3, v = (d + 2) % 3;
                    int[] x = new int[3];
                    int[] q = new int[3];
                    int[] mask = new int[size * size];

                    Direction side = Direction.Left;

                    if (d == 0)
                    {
                        side = backFace ? Direction.Left : Direction.Right;
                    }
                    else if (d == 1)
                    {
                        side = backFace ? Direction.Down : Direction.Up;
                    }
                    else if (d == 2)
                    {
                        side = backFace ? Direction.Back : Direction.Forward;
                    }

                    q[d] = 1;
                    for (x[d] = -1; x[d] < size;)
                    {
                        // Compute the mask
                        int n = 0;
                        for (x[v] = 0; x[v] < size; ++x[v])
                        {
                            for (x[u] = 0; x[u] < size; ++x[u])
                            {
                                var block1Pos = new Vector3Int(x[0], x[1], x[2]) + offset;
                                var block2Pos = new Vector3Int(x[0] + q[0], x[1] + q[1], x[2] + q[2]) + offset;

                                block1 = world.GetBlock(block1Pos); // 0 <= x[d] ? WorldManager.Active.GetBlock(new Vecx[0], x[1], x[2]]) : Block.Null;
                                block2 = world.GetBlock(block2Pos);//x[d] < size - 1 ? chunk[x[0] + q[0], x[1] + q[1], x[2] + q[2]] : Block.Null;

                                var one = BlockManager.GetNumericalId(block1.Definition.Identifier);
                                var two = BlockManager.GetNumericalId(block2.Definition.Identifier);


                                if ((one != 0) == (two != 0))
                                {
                                    mask[n] = 0;
                                }
                                else if (one != 0)
                                {
                                    mask[n] = one;
                                }
                                else
                                {
                                    mask[n] = -two;
                                }

                                n++;
                            }
                        }

                        // Increment x[d]
                        ++x[d];

                        // Generate mesh for mask using lexicographic ordering
                        n = 0;
                        for (j = 0; j < size; ++j)
                        {
                            for (i = 0; i < size;)
                            {
                                var c = mask[n];
                                if (c != 0)
                                {
                                    // Compute width
                                    for (w = 1; i + w < size && mask[n + w] == c; ++w) ;

                                    // Compute height (this is slightly awkward
                                    var done = false;
                                    for (h = 1; j + h < size; ++h)
                                    {
                                        for (k = 0; k < w; ++k)
                                        {
                                            if (c != mask[n + k + h * size])
                                            {
                                                done = true;
                                                break;
                                            }
                                        }

                                        if (done) break;
                                    }

                                    // Add quad
                                    x[u] = i;
                                    x[v] = j;
                                    int[] du = new int[3];
                                    int[] dv = new int[3];

                                    dv[v] = h;
                                    du[u] = w;

                                    c = c <= 0 ? -c : c;

                                    AddFace(new Vector3(x[0], x[1], x[2]),
                                        new Vector3(x[0] + du[0], x[1] + du[1], x[2] + du[2]),
                                        new Vector3(x[0] + du[0] + dv[0], x[1] + du[1] + dv[1], x[2] + du[2] + dv[2]),
                                        new Vector3(x[0] + dv[0], x[1] + dv[1], x[2] + dv[2]), BlockManager.GetBlock(c), side, w, h, backFace, meshData);

                                    // Zero-out mask
                                    for (l = 0; l < h; ++l)
                                    {
                                        for (k = 0; k < w; ++k)
                                        {
                                            mask[n + k + l * size] = 0;
                                        }
                                    }

                                    // Increment counters and continue
                                    i += w;
                                    n += w;
                                }
                                else
                                {
                                    ++i;
                                    ++n;
                                }
                            }
                        }
                    }
                }
            }


            chunk.Dirty = false;
            chunk.Rendered = true;

            return meshData;
        }

        private static void AddFace(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, BlockDefinition blockDefinition, Direction side, int width, int height, bool back, MeshData meshData)
        {
            int index = meshData.Vertices.Count;
            int layer = blockDefinition.GetTexture(side).Layer;

            meshData.AddVertex(v1);
            meshData.AddVertex(v2);
            meshData.AddVertex(v3);
            meshData.AddVertex(v4);

            if (side == Direction.Forward || side == Direction.Back)
            {
                meshData.AddUV(new Vector3(0, 0, layer));
                meshData.AddUV(new Vector3(width, 0, layer));
                meshData.AddUV(new Vector3(width, height, layer));
                meshData.AddUV(new Vector3(0, height, layer));
            }
            else
            {
                meshData.AddUV(new Vector3(0, 0, layer));
                meshData.AddUV(new Vector3(0, width, layer));
                meshData.AddUV(new Vector3(height, width, layer));
                meshData.AddUV(new Vector3(height, 0, layer));
            }

            if (back)
            {
                meshData.AddTriangle(0, index);
                meshData.AddTriangle(0, index + 3);
                meshData.AddTriangle(0, index + 2);

                meshData.AddTriangle(0, index + 2);
                meshData.AddTriangle(0, index + 1);
                meshData.AddTriangle(0, index);
                return;
            }

            meshData.AddTriangle(0, index);
            meshData.AddTriangle(0, index + 1);
            meshData.AddTriangle(0, index + 2);

            meshData.AddTriangle(0, index + 2);
            meshData.AddTriangle(0, index + 3);
            meshData.AddTriangle(0, index);
        }
    }
}