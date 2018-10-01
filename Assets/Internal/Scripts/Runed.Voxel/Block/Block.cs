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
            if (definition == null)
            {
                throw new NullReferenceException();
            }

            this.Definition = definition;
        }
    }
}