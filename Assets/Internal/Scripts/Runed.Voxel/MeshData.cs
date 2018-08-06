using System;
using System.Collections.Generic;
using System.Linq;
using Runed.Utilities;
using UnityEngine;

namespace Runed.Voxel
{
    [Serializable]
    public class MeshData
    {
        [SerializeField]
        protected List<List<int>> triangles = new List<List<int>>();

        [SerializeField]
        protected List<Vector2f> uv = new List<Vector2f>();

        [SerializeField]
        protected List<Vector3f> vertices = new List<Vector3f>();

        public MeshData() : this(1)
        {
        }

        public MeshData(int subMeshCount = 1)
        {
            this.SubMeshCount = subMeshCount;
        }

        public virtual int SubMeshCount
        {
            get { return this.triangles.Count; }
            set
            {
                if (value > this.triangles.Count)
                {
                    var difference = value - this.triangles.Count;
                    for (var i = 0; i < difference; i++) this.triangles.Add(new List<int>());
                }
                else if (this.triangles.Count > value)
                {
                    var difference = this.triangles.Count - value;
                    for (var i = 0; i < difference; i++) this.triangles.RemoveAt(this.triangles.Count - i);
                }
            }
        }

        public virtual List<Vector3f> Vertices => this.vertices;

        public virtual List<List<int>> Triangles => this.triangles;

        public virtual List<Vector2f> UV => this.uv;

        public virtual void AddVertex(Vector3f vertex)
        {
            this.vertices.Add(vertex);
        }

        public virtual void AddTriangle(int subMesh, int triangle)
        {
            if (subMesh >= this.triangles.Count) return;
            this.triangles[subMesh].Add(triangle);
        }

        public virtual void AddUV(Vector2f uv)
        {
            this.uv.Add(uv);
        }

        public virtual void AddCube(Vector3f position, Rect[] uvs)
        {
            this.AddCube(position, 0, uvs);
        }

        public virtual void AddCube(Vector3f position, int subMesh, Rect[] uvs)
        {
            this.AddQuad(position, subMesh, BlockDirection.Forward, uvs[0]);
            this.AddQuad(position, subMesh, BlockDirection.Back, uvs[1]);
            this.AddQuad(position, subMesh, BlockDirection.Right, uvs[2]);
            this.AddQuad(position, subMesh, BlockDirection.Left, uvs[3]);
            this.AddQuad(position, subMesh, BlockDirection.Up, uvs[4]);
            this.AddQuad(position, subMesh, BlockDirection.Down, uvs[5]);
        }

        public virtual void AddQuad(Vector3f position, BlockDirection direction, Rect uv)
        {
            this.AddQuad(position, 0, direction, uv);
        }

        public virtual void AddQuad(Vector3f position, int subMesh, BlockDirection direction, Rect uv)
        {
            switch (direction)
            {
                case BlockDirection.Forward:
                    this.vertices.Add(position + Vector3f.forward + Vector3f.left);
                    this.vertices.Add(position + Vector3f.forward);
                    this.vertices.Add(position + Vector3f.forward + Vector3f.left + Vector3f.up);
                    this.vertices.Add(position + Vector3f.forward + Vector3f.up);
                    break;
                case BlockDirection.Back:
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3f.left);
                    this.vertices.Add(position + Vector3f.up);
                    this.vertices.Add(position + Vector3f.left + Vector3f.up);
                    break;
                case BlockDirection.Right:
                    this.vertices.Add(position + Vector3f.forward);
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3f.forward + Vector3f.up);
                    this.vertices.Add(position + Vector3f.up);
                    break;
                case BlockDirection.Left:
                    this.vertices.Add(position + Vector3f.left);
                    this.vertices.Add(position + Vector3f.left + Vector3f.forward);
                    this.vertices.Add(position + Vector3f.left + Vector3f.up);
                    this.vertices.Add(position + Vector3f.left + Vector3f.forward + Vector3f.up);
                    break;
                case BlockDirection.Up:
                    this.vertices.Add(position + Vector3f.up);
                    this.vertices.Add(position + Vector3f.up + Vector3f.left);
                    this.vertices.Add(position + Vector3f.up + Vector3f.forward);
                    this.vertices.Add(position + Vector3f.up + Vector3f.forward + Vector3f.left);
                    break;
                case BlockDirection.Down:
                    this.vertices.Add(position + Vector3f.forward);
                    this.vertices.Add(position + Vector3f.forward + Vector3f.left);
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3f.left);
                    break;
            }

            this.triangles[subMesh].Add(this.vertices.Count - 4);
            this.triangles[subMesh].Add(this.vertices.Count - 3);
            this.triangles[subMesh].Add(this.vertices.Count - 2);
            this.triangles[subMesh].Add(this.vertices.Count - 3);
            this.triangles[subMesh].Add(this.vertices.Count - 1);
            this.triangles[subMesh].Add(this.vertices.Count - 2);
            this.uv.Add(new Vector2f(uv.x + uv.width, uv.y));
            this.uv.Add(new Vector2f(uv.x, uv.y));
            this.uv.Add(new Vector2f(uv.x + uv.width, uv.y + uv.height));
            this.uv.Add(new Vector2f(uv.x, uv.y + uv.height));
        }

        public virtual Mesh BuildMesh()
        {
            var mesh = new Mesh();
            mesh.subMeshCount = this.triangles.Count;
            mesh.vertices = this.vertices.Cast<Vector3>().ToArray();
            for (var i = 0; i < this.triangles.Count; i++) mesh.SetTriangles(this.triangles[i], i, true);
            mesh.uv = this.uv.Cast<Vector2>().ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }
    }
}