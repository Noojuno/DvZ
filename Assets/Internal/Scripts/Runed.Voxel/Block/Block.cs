using System;

namespace Runed.Voxel
{
    public class Block
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