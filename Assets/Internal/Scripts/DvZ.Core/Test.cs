using Runed.Voxel;
using UnityEditor;
using UnityEngine;
using Runed.Utilities;

namespace DvZ.Core
{
    public class Test : MonoBehaviour
    {
        public Texture2D texture;
        public Texture2D tex2;

        public Rect rect;

        public Vector3Int pos;

        public GameObject ChunkGameObject;

        void Awake()
        {
            BlockManager.Initialize();
            TextureManager.Initialize();

            /* for (int x = -2; x < 2; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    for (int z = -2; z < 2; z++)
                    {
                        ChunkRenderer chunk = Instantiate<ChunkRenderer>(this.ChunkGameObject, Vector3.zero, Quaternion.identity, this.transform);
                        chunk.chunkPosition = new Vector3Int(x, y, z);
                    }
                }
            } */

            ObjectPool.Initialise(this.ChunkGameObject, 32);
            ObjectPool.Spawn(this.ChunkGameObject, Vector3.zero, Quaternion.identity);
        }

        void Start()
        {
            this.texture = TextureManager.GetTexture("blocks:blocktesttwo").Texture;
            this.rect = TextureManager.GetTexture("blocks:blocktesttwo").UV;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.G))
            {
                Debug.Log(WorldManager.Active.GetBlock(pos).Definition.Identifier);
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
        }
    }
}
