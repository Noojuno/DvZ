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

        public Material material;

        // Use this for initialization
        void Start()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();

            this.mesh = new Mesh();

            this.Chunk = new Chunk(WorldManager.Active, Vector3Int.zero);

            for (var x = 0; x < Chunk.Size; x++)
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
            }

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
                this.mesh = this.Chunk.ToMeshData().ToMesh();
                this.meshFilter.sharedMesh = this.mesh;
            }
        }
    }
}