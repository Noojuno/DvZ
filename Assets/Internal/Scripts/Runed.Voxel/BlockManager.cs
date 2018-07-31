using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Runed.Voxel
{
    public static class BlockManager
    {
        public static Dictionary<int, BlockType> BlockTypes = new Dictionary<int, BlockType>();
        public static int NextAvailableId => BlockManager.BlockTypes.Count > 0 ? BlockManager.BlockTypes.Keys.Max() + 1 : 0;

        public static void Initialize()
        {
            string[] list = {"69;blocktesttwo"};

            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.IsClass && y.BaseType == typeof(BlockType)))
                .Select(x => Activator.CreateInstance(x) as BlockType)
                .ToArray();

            //TODO: FIX THIS DOGSHIT
            foreach (var blockType in types)
            {
                var a = list.First(value => value.Split(';')[1] == blockType.Identifier);

                if (a.Any())
                {
                    BlockManager.BlockTypes[int.Parse(a.Split(';')[0])] = blockType;
                }
                else
                {
                    BlockManager.BlockTypes[BlockManager.NextAvailableId] = blockType;
                }
            }
        }

        public static BlockType GetBlock(int id)
        {
            return BlockManager.BlockTypes[id];
        }

        public static string Export()
        {
            var list = BlockTypes.Select(kv => $"{kv.Key};{kv.Value.Identifier}").ToArray();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            return json;
        }
    }
}