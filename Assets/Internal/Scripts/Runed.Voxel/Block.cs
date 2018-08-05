namespace Runed.Voxel
{
    public struct Block
    {
        public Chunk Chunk { get; }
        public BlockDefinition Definition { get; }

        public Block(BlockDefinition definition)
        {
            this.Definition = definition;
            this.Chunk = null;
        }
    }
}