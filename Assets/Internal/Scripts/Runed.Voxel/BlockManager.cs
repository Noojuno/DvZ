using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Runed.Voxel
{
    public static class BlockManager
    {
        public static Dictionary<int, BlockDefinition> BlockTypes = new Dictionary<int, BlockDefinition>();
        public static int NextAvailableId => BlockManager.BlockTypes.Count > 0 ? BlockManager.BlockTypes.Keys.Max() + 1 : 0;

        public static void Initialize()
        {
            BlockManager.BlockTypes = new Dictionary<int, BlockDefinition>();

            string[] list = {"69;blocktesttwo"};

            var blockDefinitions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.IsClass && y.BaseType == typeof(BlockDefinition)))
                .Select(x => Activator.CreateInstance(x) as BlockDefinition)
                .ToArray();

            //TODO: FIX THIS DOGSHIT
            foreach (var blockDefinition in blockDefinitions)
            {
                var a = list.Where(value => value.Split(';')[1] == blockDefinition.Identifier);

                if (a.Any())
                {
                    BlockManager.BlockTypes[int.Parse(a.First().Split(';')[0])] = blockDefinition;
                }
                else
                {
                    BlockManager.BlockTypes[BlockManager.NextAvailableId] = blockDefinition;
                }
            }
        }

        public static BlockDefinition GetBlock(int id)
        {
            return BlockManager.BlockTypes[id];
        }

        public static string Export()
        {
            var list = BlockTypes.Select(kv => $"{kv.Key};{kv.Value.Identifier}").ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}