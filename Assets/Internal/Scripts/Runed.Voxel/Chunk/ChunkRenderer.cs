using UnityEngine;

namespace Runed.Voxel
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        public Chunk Chunk;
        public Mesh mesh;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;
        public MeshCollider meshCollider;

        public Vector3Int chunkPosition = Vector3Int.zero;

        // Use this for initialization
        public void Start()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshCollider = this.GetComponent<MeshCollider>();
            this.meshRenderer.material.mainTexture = TextureManager.BlockAtlas;

            this.mesh = new Mesh();

            this.transform.position = this.Chunk.Position * Chunk.Size;
            this.gameObject.name = "Chunk " + this.Chunk.Position;

            this.meshFilter.sharedMesh = this.mesh;
            this.meshCollider.sharedMesh = this.mesh;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                this.Chunk[4, 8, 4] = new Block(BlockManager.GetBlock("air"));
                this.Chunk[4, 8, 4] = new Block(BlockManager.GetBlock("air"));
            }

            if (this.Chunk != null && this.Chunk.Loaded && this.Chunk.Dirty)
            {
                this.mesh = MeshBuilder.BuildChunk(this.Chunk).ToMesh();
                this.meshFilter.sharedMesh = this.mesh;
                this.meshCollider.sharedMesh = this.mesh;

                this.Chunk.Rendered = true;
            }
        }
    }
}