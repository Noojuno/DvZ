using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public class TextureAsset
    {
        public string Identifier;
        public string Path;
        public Rect UV;
        public Texture2D Texture;

        public static TextureAsset blank = new TextureAsset("Textures/Blocks/empty", "blank");

        public TextureAsset(string path, string assetIdentifier)
        {
            this.Path = path;
            this.Texture = Resources.Load<Texture2D>(this.Path);

            this.Identifier = assetIdentifier;
        }

        public TextureAsset(Texture2D texture, string assetIdentifier)
        {
            this.Texture = texture;
            this.Path = this.Texture.name;

            this.Identifier = assetIdentifier;
        }

    }
}
