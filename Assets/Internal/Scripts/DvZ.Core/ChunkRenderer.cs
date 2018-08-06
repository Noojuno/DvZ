using UnityEngine;

namespace Runed.Voxel
{
    [RequireComponent(typeof(Mesh), typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        public Chunk Chunk;
        public MeshRenderer meshRenderer;
        public Mesh mesh;

        // Use this for initialization
        private void Start()
        {
            this.mesh = this.GetComponent<Mesh>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (this.Chunk != null && this.Chunk.Dirty)
            {
                //this.Chunk.Update();
            }
        }
    }
}