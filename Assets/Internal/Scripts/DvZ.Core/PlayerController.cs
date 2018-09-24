using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runed.Voxel;
using UnityEngine;

namespace DvZ.Core
{
    public class PlayerController : MonoBehaviour
    {
        // CHUNK LOADING
        [Header("Chunk Loading")]
        public int LoadRadius = 3;
        public Queue<Vector3Int> ChunkQueue = new Queue<Vector3Int>();

        public Transform WorldTransform;
        public GameObject ChunkPrefab;

        public int MaxActive = 4;
        public int CurrActive = 0;

        void Start()
        {
            SimplePool.Preload(this.ChunkPrefab, this.LoadRadius ^ 3);

            for (int x = 0; x < this.LoadRadius; x++)
            {
                for (int y = 0; y < this.LoadRadius; y++)
                {
                    for (int z = 0; z < this.LoadRadius; z++)
                    {
                        var pos = new Vector3Int(x, y, z);

                        this.ChunkQueue.Enqueue(pos);
                        WorldManager.Active.CreateChunk(pos);
                    }
                }
            }
        }

        void Update()
        {
            if (this.CurrActive <= this.MaxActive && this.ChunkQueue.Count > 0)
            {
                this.LoadNext();
                this.CurrActive++;
            }
        }

        void LoadNext()
        {
            var pos = this.ChunkQueue.Dequeue();
            var chunk = WorldManager.Active.GetChunk(pos);

            var g = SimplePool.Spawn(this.ChunkPrefab, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(this.WorldTransform);

            WorldManager.Active.TerrainGenerator.Generate(chunk);

            g.GetComponent<ChunkRenderer>().Chunk = chunk;

            chunk.Loaded = true;
            chunk.Dirty = true;

            this.CurrActive--;
        }
    }
}
