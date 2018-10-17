using System;

namespace Runed.Voxel
{
    public struct Block
    {
        public static Block Null = new Block(BlockDefinition.Air);

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

        public bool Equals(Block block)
        {
            return this.Definition.Identifier == block.Definition.Identifier;
        }
    }
}