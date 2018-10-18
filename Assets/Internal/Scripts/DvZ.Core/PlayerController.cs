using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Runed.Voxel;
using UnityEngine;
using Random = UnityEngine.Random;

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
                for (int y = -1; y < this.LoadRadius; y++)
                {
                    for (int z = 0; z < this.LoadRadius; z++)
                    {
                        var pos = new Vector3Int(x, y, z);

                        this.ChunkQueue.Enqueue(pos);
                    }
                }
            }
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 40, 100, 25), "Add Chunk"))
            {
                this.ChunkQueue.Enqueue(new Vector3Int(16, Random.Range(-100, 100), 16));
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
            var chunk = WorldManager.Active.CreateChunk(pos);

            var g = SimplePool.Spawn(this.ChunkPrefab, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(this.WorldTransform);

            g.GetComponent<ChunkRenderer>().Chunk = chunk;

            chunk.Dirty = true;

            this.CurrActive--;
        }
    }
}
