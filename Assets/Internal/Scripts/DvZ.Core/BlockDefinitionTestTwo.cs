using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class BlockDefinitionTestTwo : BlockDefinition
    {
        public override string Identifier => "blocktesttwo"; 
        public override string DisplayName => "Test Block Two";

        public override Texture2D Texture => Resources.Load<Texture2D>("Textures/lapis_ore");
    }
}
