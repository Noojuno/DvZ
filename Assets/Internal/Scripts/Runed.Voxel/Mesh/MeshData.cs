using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runed.Voxel
{
    [Serializable]
    public class MeshData
    {
        [SerializeField]
        protected List<List<int>> triangles = new List<List<int>>();

        [SerializeField]
        protected List<Vector3> uv = new List<Vector3>();

        [SerializeField]
        protected List<Vector3> vertices = new List<Vector3>();

        [SerializeField]
        protected List<Vector3> normals = new List<Vector3>();

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

        public virtual List<Vector3> Vertices => this.vertices;

        public virtual List<List<int>> Triangles => this.triangles;

        public virtual List<Vector3> UV => this.uv;

        public virtual List<Vector3> Normals => this.normals;

        public virtual void AddVertex(Vector3 vertex)
        {
            this.vertices.Add(vertex);
        }

        public virtual void AddNormal(Vector3 normal)
        {
            this.normals.Add(normal);
        }

        public virtual void AddTriangle(int subMesh, int triangle)
        {
            if (subMesh >= this.triangles.Count) return;
            this.triangles[subMesh].Add(triangle);
        }

        public virtual void AddUV(Vector3 uv)
        {
            this.uv.Add(uv);
        }

        public virtual void AddCube(Vector3 position, Rect[] uvs)
        {
            this.AddCube(position, 0, uvs);
        }

        public virtual void AddCube(Vector3 position, int subMesh, Rect[] uvs)
        {
            this.AddQuad(position, subMesh, Direction.Forward, uvs[0]);
            this.AddQuad(position, subMesh, Direction.Back, uvs[1]);
            this.AddQuad(position, subMesh, Direction.Right, uvs[2]);
            this.AddQuad(position, subMesh, Direction.Left, uvs[3]);
            this.AddQuad(position, subMesh, Direction.Up, uvs[4]);
            this.AddQuad(position, subMesh, Direction.Down, uvs[5]);
        }

        public virtual void AddQuad(Vector3 position, Direction direction, Rect uv)
        {
            this.AddQuad(position, 0, direction, uv);
        }

        public virtual void AddQuad(Vector3 position, int subMesh, Direction direction, int layer)
        {
            this.AddQuad(position, subMesh, direction, new Rect(0, 0, 1, 1), layer);
        }

        public virtual void AddQuad(Vector3 position, int subMesh, Direction direction, Rect uv, int layer = 0)
        {
            switch (direction)
            {
                case Direction.Forward:
                    this.vertices.Add(position + Vector3.forward + Vector3.left);
                    this.vertices.Add(position + Vector3.forward);
                    this.vertices.Add(position + Vector3.forward + Vector3.left + Vector3.up);
                    this.vertices.Add(position + Vector3.forward + Vector3.up);
                    break;
                case Direction.Back:
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3.left);
                    this.vertices.Add(position + Vector3.up);
                    this.vertices.Add(position + Vector3.left + Vector3.up);
                    break;
                case Direction.Right:
                    this.vertices.Add(position + Vector3.forward);
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3.forward + Vector3.up);
                    this.vertices.Add(position + Vector3.up);
                    break;
                case Direction.Left:
                    this.vertices.Add(position + Vector3.left);
                    this.vertices.Add(position + Vector3.left + Vector3.forward);
                    this.vertices.Add(position + Vector3.left + Vector3.up);
                    this.vertices.Add(position + Vector3.left + Vector3.forward + Vector3.up);
                    break;
                case Direction.Up:
                    this.vertices.Add(position + Vector3.up);
                    this.vertices.Add(position + Vector3.up + Vector3.left);
                    this.vertices.Add(position + Vector3.up + Vector3.forward);
                    this.vertices.Add(position + Vector3.up + Vector3.forward + Vector3.left);
                    break;
                case Direction.Down:
                    this.vertices.Add(position + Vector3.forward);
                    this.vertices.Add(position + Vector3.forward + Vector3.left);
                    this.vertices.Add(position);
                    this.vertices.Add(position + Vector3.left);
                    break;
            }

            this.triangles[subMesh].Add(this.vertices.Count - 4);
            this.triangles[subMesh].Add(this.vertices.Count - 3);
            this.triangles[subMesh].Add(this.vertices.Count - 2);
            this.triangles[subMesh].Add(this.vertices.Count - 3);
            this.triangles[subMesh].Add(this.vertices.Count - 1);
            this.triangles[subMesh].Add(this.vertices.Count - 2);
            this.uv.Add(new Vector3(uv.x + uv.width, uv.y, layer));
            this.uv.Add(new Vector3(uv.x, uv.y, layer));
            this.uv.Add(new Vector3(uv.x + uv.width, uv.y + uv.height, layer));
            this.uv.Add(new Vector3(uv.x, uv.y + uv.height, layer));
        }

        public virtual Mesh ToMesh()
        {
            var mesh = new Mesh();
            mesh.subMeshCount = this.triangles.Count;
            mesh.vertices = this.vertices.ToArray();
            for (var i = 0; i < this.triangles.Count; i++) mesh.SetTriangles(this.triangles[i], i, true);
            mesh.SetUVs(0, this.UV);
            //mesh.uv = this.uv.ToArray();
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return mesh;
        }
    }
}