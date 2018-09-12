using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    //TODO: SPRITESHEET OR TEXTURE ARRAY MANAGEMENT
    public static class TextureManager
    {
        public static Dictionary<string, Texture2D> Textures;

        public static void Initialize()
        {
            TextureManager.Textures = new Dictionary<string, Texture2D>();

            TextureManager.RegisterBlockTextures();
        }

        private static void RegisterBlockTextures()
        {
            foreach (var blockType in BlockManager.BlockTypes)
            {
                var blockId = blockType.Value.Identifier;
                var texture = Resources.Load<Texture2D>($"Textures/Blocks/{blockId}");

                if (texture != null)
                {
                    TextureManager.RegisterTexture($"blocks:{blockId}", texture);
                }
            }
        }

        public static void RegisterTexture(string key, Texture2D texture)
        {
            Debug.Log(key + " + " + texture.name);

            TextureManager.Textures.Add(key, texture);
        }

        public static Texture2D GetTexture(string key)
        {
            if (!TextureManager.Textures.ContainsKey(key))
            {
                return Texture2D.whiteTexture;
            }

            return TextureManager.Textures[key];
        }
    }
}
