using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class BlockDefinitionTestTwo : BlockDefinition
    {
        public override string Identifier => "blocktesttwo"; 
        public override string DisplayName => "Test Block Two";


        private TextureAsset tex;
        public BlockDefinitionTestTwo() : base()
        {
            this.tex = new TextureAsset("Textures/Blocks/hay_block_side", this.Identifier + "_0");
        }

        public override TextureAsset GetTexture(Direction direction)
        {
            if (direction == Direction.Right)
            {
                return this.tex;
            }

            return base.GetTexture(direction);
        }
    }
}
