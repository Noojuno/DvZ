using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

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

        public MeshData meshData;

        private bool _building = false;

        // Use this for initialization
        public void Start()
        {
            this.meshFilter = this.GetComponent<MeshFilter>();
            this.meshRenderer = this.GetComponent<MeshRenderer>();
            this.meshCollider = this.GetComponent<MeshCollider>();
            this.meshRenderer.material.mainTexture = TextureManager.BlockArray;
            //this.meshRenderer.materials[1].mainTexture = TextureManager.BlockArray;
            //this.meshRenderer.material.SetTexture("_Textures", TextureManager.BlockArray);

            this.mesh = new Mesh();

            this.transform.position = this.Chunk.Position * Chunk.Size;
            this.gameObject.name = "Chunk " + this.Chunk.Position;

            this.meshFilter.sharedMesh = this.mesh;
            this.meshCollider.sharedMesh = this.mesh;
        }

        private void BuildChunk()
        {
            this._building = true;

            if (!this.Chunk.Loaded)
            {
                WorldManager.Active.TerrainGenerator.Generate(this.Chunk);
            }

            Profiler.BeginSample("Build Chunk " + this.Chunk.Position);
            this.meshData = MeshBuilder.CreateGreedyMesh(this.Chunk);
            Profiler.EndSample();
            this._building = false;
        }

        void Update()
        {
            if (this.Chunk != null)
            {
                if (this.Chunk.Dirty && !this._building)
                {
                    ThreadPool.QueueUserWorkItem(c => BuildChunk());
                }
                else if (this.Chunk.Rendered && this.Chunk.Loaded)
                {
                    this.mesh = this.meshData.ToMesh();
                    this.meshFilter.sharedMesh = this.mesh;
                    this.meshCollider.sharedMesh = this.mesh;

                    this.Chunk.Rendered = false;
                }
            }
        }
    }
}