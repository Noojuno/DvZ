using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runed.Voxel
{
    public static class BlockManager
    {
        public static Dictionary<int, BlockDefinition> BlockDefinitions = new Dictionary<int, BlockDefinition>();
        public static int NextAvailableId => BlockManager.BlockDefinitions.Count > 0 ? BlockManager.BlockDefinitions.Keys.Max() + 1 : 0;

        //TODO: GET AUTO REFLECTION WORKING
        public static void Initialize()
        {
            BlockManager.BlockDefinitions = new Dictionary<int, BlockDefinition>();

            /* Dictionary<string, int> map = new Dictionary<string, int> {{"air", 0}};

            var blockDefinitions = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes().Where(y => y.IsClass && y.BaseType == typeof(BlockDefinition)))
                .Select(x => Activator.CreateInstance(x) as BlockDefinition)
                .ToArray();

            Debug.Log(blockDefinitions[0]);

            var orderedDefinitions = blockDefinitions.OrderBy(e => map.ContainsKey(e.Identifier) ? 0 : 1);

            foreach (var blockDefinition in orderedDefinitions)
            {
                var id = BlockManager.NextAvailableId;

                if (map.ContainsKey(blockDefinition.Identifier))
                {
                    id = map[blockDefinition.Identifier];
                }

                Debug.Log($"Name: {blockDefinition.DisplayName}, Identifier: {blockDefinition.Identifier}, Numeric: {id}");

                BlockManager.BlockDefinitions[id] = blockDefinition;
            } */

            BlockManager.BlockDefinitions.Add(0, BlockDefinition.Air);
        }

        public static BlockDefinition GetBlock(string identifier)
        {
            var def = BlockDefinitions.Values.First(x => x.Identifier == identifier);

            if (def == null)
            {
                throw new NullReferenceException();
            }

            return def;
        }

        public static BlockDefinition GetBlock(int id)
        {
            return BlockManager.BlockDefinitions[id];
        }

        public static int GetNumericalId(string identifier)
        {
            return BlockDefinitions.FirstOrDefault(x => x.Value.Identifier == identifier).Key;
        }

        public static string[] Load()
        {
            return new string[] { "0;air" };//{"0;blocktesttwo"}; 
        }

        public static string Export()
        {
            /* var list = BlockDefinitions.Select(kv => $"{kv.Key};{kv.Value.Identifier}").ToArray();
            var json = JsonConvert.SerializeObject(list);
            return json; */

            return "";
        }
    }
}