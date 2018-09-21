using Runed.Voxel;
using UnityEditor;
using UnityEngine;
using Runed.Utilities;

namespace DvZ.Core
{
    public class Test : MonoBehaviour
    {
        public GameObject ChunkGameObject;
        public GameObject WorldGameObject;

        private int testY = 10;

        void Awake()
        {
            BlockManager.Initialize();

            //TODO: FIX ME
            BlockManager.BlockDefinitions.Add(1, new BlockDefinitionTest());
            BlockManager.BlockDefinitions.Add(2, new BlockDefinitionTestTwo());

            TextureManager.Initialize();

            //ChunkPool.Initialize(this.ChunkGameObject, this.WorldGameObject, 32);

            SimplePool.Preload(this.ChunkGameObject, 32);
        }

        void Start()
        {
            for (int x = -2; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    for (int z = -2; z < 2; z++)
                    {
                        var g = SimplePool.Spawn(this.ChunkGameObject, Vector3.zero, Quaternion.identity);
                        g.transform.SetParent(this.WorldGameObject.transform);
                        g.GetComponent<ChunkRenderer>().chunkPosition = new Vector3Int(x, y, z);
                    }
                }
            }
        }

        void OnGUI()
        {
            if(GUI.Button(new Rect(10, 10, 100, 25), "Rebuild Chunks"))
            {
                var seed = Random.Range(1, 1000000);
                WorldManager.Active.SetSeed(seed);

                Debug.Log(seed);

                foreach (var chunk in WorldManager.Active._chunks)
                {
                    WorldManager.Active.TerrainGenerator.Generate(chunk.Value);
                    chunk.Value.Dirty = true;
                }
            }

            if (GUI.Button(new Rect(10, 40, 100, 25), "Add Chunk"))
            {
                var g = SimplePool.Spawn(this.ChunkGameObject, Vector3.zero, Quaternion.identity);
                g.GetComponent<ChunkRenderer>().chunkPosition = new Vector3Int(0, -testY++, 0);

                //ChunkPool.Create(new Vector3Int(0, -testY++, 0));
            }
        }
    }
}
