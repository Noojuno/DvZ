using UnityEngine;

namespace Runed.Voxel.Unity
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        public Chunk Chunk;
        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;

        // Use this for initialization
        private void Start()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.Chunk != null && this.Chunk.Dirty)
            {
                this.Chunk.Update();
            }
        }
    }
}