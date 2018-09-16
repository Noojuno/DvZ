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
        public static Dictionary<string, TextureAsset> Textures;
        public static Dictionary<Texture2D, Rect> UVs;
        public static Texture2D BlockAtlas;

        public static void Initialize()
        {
            TextureManager.Textures = new Dictionary<string, TextureAsset>();
            TextureManager.UVs = new Dictionary<Texture2D, Rect>();

            TextureManager.RegisterBlockTextures();
            TextureManager.PackBlockTextures();
        }

        private static void RegisterBlockTextures()
        {
            foreach (var blockType in BlockManager.BlockTypes)
            {
                var blockId = blockType.Value.Identifier;
                var texture = Resources.Load<Texture2D>($"Textures/Blocks/{blockId}");

                if (texture == null)
                {
                    texture = Texture2D.whiteTexture;
                }

                TextureManager.RegisterTexture($"blocks:{blockId}", texture);
            }
        }

        private static void PackBlockTextures()
        {
            TextureManager.BlockAtlas = new Texture2D(2048, 2048);

            var rects = TextureManager.BlockAtlas.PackTextures(Textures.Values.ToArray(), 0, 2048, false);

            int i = 0;
            foreach (var texture in Textures.Values)
            {
                TextureManager.UVs[texture] = rects[i++];
            }
        }

        public static void RegisterTexture(string key, Texture2D texture)
        {
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

        public static Texture2D GetBlockTexture(BlockDefinition blockDefinition)
        {
            return TextureManager.GetTexture($"blocks:{blockDefinition.Identifier}");
        }

        public static Rect GetBlockUVs(BlockDefinition blockDefinition)
        {
            return TextureManager.UVs[GetBlockTexture(blockDefinition)];
        }
    }
}