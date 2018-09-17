using Runed.Voxel;

namespace DvZ.Core
{
    public class BlockDefinitionTest : BlockDefinition
    {
        public override string Identifier => "glass";
        public override string DisplayName => "Test Block";

        public override bool Translucent => true;
    }
}
