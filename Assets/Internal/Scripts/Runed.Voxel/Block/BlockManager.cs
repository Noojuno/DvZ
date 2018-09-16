using System;
using System.Collections.Generic;
using System.Linq;
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

            Dictionary<string, int> map = new Dictionary<string, int> {{"air", 0}};

            var blockDefinitions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.IsClass && y.BaseType == typeof(BlockDefinition)))
                .Select(x => Activator.CreateInstance(x) as BlockDefinition)
                .ToArray();

            var orderedDefinitions = blockDefinitions.OrderBy(e => map.ContainsKey(e.Identifier) ? 0 : 1);

            foreach (var blockDefinition in orderedDefinitions)
            {
                var id = BlockManager.NextAvailableId;

                if (map.ContainsKey(blockDefinition.Identifier))
                {
                    id = map[blockDefinition.Identifier];
                }

                Debug.Log($"Name: {blockDefinition.DisplayName}, Identifier: {blockDefinition.Identifier}, Numeric: {id}");

                BlockManager.BlockTypes[id] = blockDefinition;
            }
        }

        public static BlockDefinition GetBlock(string identifier)
        {
            return BlockTypes.Values.First(x => x.Identifier == identifier);
        }

        public static BlockDefinition GetBlock(int id)
        {
            return BlockManager.BlockTypes[id];
        }

        public static string[] Load()
        {
            return new string[] { "0;air" };//{"0;blocktesttwo"}; 
        }

        public static string Export()
        {
            /* var list = BlockTypes.Select(kv => $"{kv.Key};{kv.Value.Identifier}").ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json; */

            return "";
        }
    }
}