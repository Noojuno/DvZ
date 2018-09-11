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
        private void Start()
        {
            BlockManager.Initialize();

            Chunk = new Chunk(null);

            for (var x = 0; x < Chunk.Size; x++)
            {
                for (var z = 0; z < Chunk.Size; z++)
                {
                    for (var y = 0; y < Chunk.Size; y++)
                    {
                        if (y == 0  || (y == 1 && x >=4 && x <= 11) || (y == 2 && x >= 6 && x <= 9 && z >= 4 && z <= 11) || y == 8 )
                        {
                            Chunk.Blocks[x, y, z] = new Block(BlockManager.GetBlock(1));
                        }
                        else
                        {

                            Chunk.Blocks[x, y, z] = new Block(BlockManager.GetBlock("air"));
                        }
                    }
                }
            }

            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = material;

            mesh = Chunk.ToMeshData().ToMesh();

            this.meshFilter.sharedMesh = mesh;
        }
    }
}