using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public class BlockDefinition
    {
        public static BlockDefinition Air = new BlockDefinitionAir();

        public virtual string Identifier { get; }
        public virtual string DisplayName { get; }

        public virtual bool Solid { get; }
        public virtual bool Translucent { get; } = false;
        public virtual bool Render { get; } = true;

        public virtual Texture2D Texture { get; }

        public virtual bool HasCustomModel { get; }

        private readonly TextureAsset texture;

        public BlockDefinition()
        {
            this.texture = new TextureAsset($"Textures/Blocks/{this.Identifier}", $"{this.Identifier}");
        }

        public virtual TextureAsset GetTexture(Direction direction)
        {
            return this.texture;
        }

        //TODO
        public virtual string GetRegistryName()
        {
            return "";
        }
    }
}
