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
        public string Path;
        public Rect UV;
        public Texture2D Texture;

        public static TextureAsset blank = new TextureAsset(Texture2D.whiteTexture);

        public TextureAsset(string path)
        {
            this.Path = path;
            this.Texture = Resources.Load<Texture2D>(this.Path);
        }

        public TextureAsset(Texture2D texture)
        {
            this.Texture = texture;
            this.Path = this.Texture.name;
        }

    }
}
