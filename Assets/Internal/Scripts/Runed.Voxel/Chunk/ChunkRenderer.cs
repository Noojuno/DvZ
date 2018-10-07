using System.Collections.Generic;
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

        public List<Texture2D> tex;

        // Use this for initialization
        public void Start()
        {
            this.tex = new List<Texture2D>();

            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshCollider = this.GetComponent<MeshCollider>();
            //this.meshRenderer.material.mainTexture = TextureManager.BlockArray;
            this.meshRenderer.material.SetTexture("_MyArr", TextureManager.BlockArray);

            for (int c = 0; c < TextureManager.BlockArray.depth; c++)
            {
                var t = new Texture2D(16, 16);
                t.SetPixels(TextureManager.BlockArray.GetPixels(c));
                t.Apply();

                this.tex.Add(t);
            }

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
                this.Chunk[4, 8, 4] = new Block(BlockDefinition.Air);
                this.Chunk[4, 8, 4] = new Block(BlockDefinition.Air);
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