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
        public static Texture2D BlockAtlas;

        public static void Initialize()
        {
            TextureManager.Textures = new Dictionary<string, TextureAsset>();

            TextureManager.RegisterBlockTextures();
            TextureManager.PackBlockTextures();
        }

        private static void RegisterBlockTextures()
        {
            foreach (var blockType in BlockManager.BlockTypes)
            {
                var blockId = blockType.Value.Identifier;

                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    var texture = blockType.Value.GetTexture(direction);

                    if (!TextureManager.Textures.ContainsKey(texture.Identifier))
                    {
                        TextureManager.RegisterTexture(texture.Identifier, texture);
                    }
                }
            }
        }

        //TODO: MIPMAPS
        private static void PackBlockTextures()
        {
            TextureManager.BlockAtlas = new Texture2D(2048, 2048)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            var rects = TextureManager.BlockAtlas.PackTextures(Textures.Values.Select(x => x.Texture).ToArray(), 2, 2048, false);

            int i = 0;
            foreach (var texture in Textures.Values)
            {
                texture.UV = rects[i++];
            }
        }

        public static void RegisterTexture(string key, TextureAsset texture)
        {
            TextureManager.Textures.Add(key, texture);
        }

        public static TextureAsset GetTexture(string key)
        {
            if (!TextureManager.Textures.ContainsKey(key))
            {
                return TextureAsset.blank;
            }

            return TextureManager.Textures[key];
        }

        public static TextureAsset GetBlockTexture(BlockDefinition blockDefinition)
        {
            return TextureManager.GetTexture($"blocks:{blockDefinition.Identifier}");
        }
    }
}