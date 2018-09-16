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

        // Use this for initialization
        void Start()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshRenderer.material.mainTexture = TextureManager.GetBlockTexture(BlockManager.GetBlock("blocktesttwo"));

            this.mesh = new Mesh();

            this.Chunk = new Chunk(WorldManager.Active, new Vector3Int(0, a, 0));
            a++;

            this.transform.position = this.Chunk.Position * Chunk.Size;
            this.gameObject.name = "Chunk " + this.Chunk.Position;


            /* for (var x = 0; x < Chunk.Size; x++)
            {
                for (var z = 0; z < Chunk.Size; z++)
                {
                    for (var y = 0; y < Chunk.Size; y++)
                    {
                        if (y == 0  || (y == 1 && x >=4 && x <= 11) || (y == 2 && x >= 6 && x <= 9 && z >= 4 && z <= 11) || y == 8 || x == 0 && z == 0 )
                        {
                            this.Chunk.Blocks[x, y, z] = new Block(BlockManager.GetBlock("blocktesttwo"));
                        }
                        else
                        {

                            this.Chunk.Blocks[x, y, z] = new Block(BlockManager.GetBlock("air"));
                        }
                    }
                }
            } */

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