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
        public static Texture2DArray BlockArray;

        public static int BlockTextureSize = 16;

        public static void Initialize()
        {
            TextureManager.Textures = new Dictionary<string, TextureAsset>();

            TextureManager.RegisterBlockTextures();
            TextureManager.PackBlockTextures();
        }

        private static void RegisterBlockTextures()
        {
            foreach (var blockType in BlockManager.BlockDefinitions)
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

            var blockArray = new Texture2DArray(16, 16, Textures.Count, TextureFormat.ARGB32, true);
            blockArray.filterMode = FilterMode.Point;
            blockArray.wrapMode = TextureWrapMode.Clamp;

            int x = 0;
            for (int i = 0; i < TextureManager.Textures.Count; i++)
            {
                var textureAsset = TextureManager.Textures.Values.ToArray()[i];

                if (textureAsset.Texture == null) continue;

                blockArray.SetPixels(textureAsset.Texture.GetPixels(0), x++, 0);
            }

            int z = 0;
            foreach (var texture in Textures.Values)
            {
                texture.UV = new Rect(0, 0, 1, 1);//rects[z++];
            }

            TextureManager.BlockArray = blockArray;
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