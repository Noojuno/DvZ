using System;

namespace Runed.Voxel
{
    public struct Block
    {
        /* public Chunk Chunk { get; }
        public int Id { get; } */

        public BlockDefinition Definition { get; }

        public Block(BlockDefinition definition)
        {
            this.Definition = definition;
        }
    }
}