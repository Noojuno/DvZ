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

        public Vector3Int chunkPosition = Vector3Int.zero;

        public static int a = -1;
        public static int b = -1;

        public static int c = 0;

        // Use this for initialization
        void OnEnable()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshRenderer.material.mainTexture = TextureManager.BlockAtlas;

            this.mesh = new Mesh();

            this.chunkPosition = new Vector3Int(a, 0, b);

            if (c == 0)
            {
                a++;
                c++;
            }
            else
            {
                c--;
                b++;
            }
            this.Chunk = new Chunk(WorldManager.Active, this.chunkPosition);
            a++;

            this.transform.position = this.Chunk.Position * Chunk.Size;
            this.gameObject.name = "Chunk " + this.Chunk.Position;

            WorldManager.Active.TerrainGenerator.Generate(this.Chunk);

            this.Chunk.Loaded = true;
            this.Chunk.Dirty = true;

            this.meshFilter.sharedMesh = this.mesh;
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

                this.Chunk.Rendered = true;
            }
        }
    }
}