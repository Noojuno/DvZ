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

            string[] list = BlockManager.Load();

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

            foreach (var type in BlockTypes)
            {
                Debug.Log(type.Value.DisplayName + " " + type.Value.Identifier);
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