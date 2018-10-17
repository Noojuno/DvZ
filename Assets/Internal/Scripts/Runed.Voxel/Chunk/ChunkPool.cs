using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Runed.Voxel
{
    public static class ChunkPool
    {
        private static Queue<ChunkRenderer> chunkRenderers = new Queue<ChunkRenderer>();
        private static GameObject chunkRendererPrefab;

        private static int maxSize = 64;
        private static int initialSize = 32;

        public static void Initialize(GameObject prefab, GameObject parent, int maximumSize = 64, int startingSize = 32)
        {
            chunkRendererPrefab = prefab;

            maxSize = maximumSize;
            initialSize = startingSize;

            for (int x = 0; x <= startingSize; x++)
            {
                var gameObject = GameObject.Instantiate(chunkRendererPrefab, Vector3.zero, Quaternion.identity);
                var chunkRenderer = gameObject.GetComponent<ChunkRenderer>();

                gameObject.transform.SetParent(parent.transform);
                gameObject.SetActive(false);

                chunkRenderers.Enqueue(chunkRenderer);
            }
        }

        public static ChunkRenderer Create(Vector3Int position)
        {
            ChunkRenderer chunkRenderer;

            if (chunkRenderers.Count >= maxSize || !chunkRenderers.Peek().gameObject.activeInHierarchy)
            {
                chunkRenderer = chunkRenderers.Dequeue();
                //chunkRenderer.chunkPosition = position;
                chunkRenderer.gameObject.SetActive(true);

                chunkRenderer.Start();
            }
            else
            {
                var gameObject = GameObject.Instantiate(chunkRendererPrefab, Vector3.zero, Quaternion.identity);
                chunkRenderer = gameObject.GetComponent<ChunkRenderer>();
                //chunkRenderer.chunkPosition = position;
            }

            chunkRenderers.Enqueue(chunkRenderer);

            return chunkRenderer;
        }
    }
}
