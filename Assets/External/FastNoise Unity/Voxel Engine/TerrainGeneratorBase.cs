using UnityEngine;

namespace VoxelEngine
{
    [ExecuteInEditMode]
    public abstract class TerrainGeneratorBase : MonoBehaviour
    {
        protected int interpBitStep;
        protected float interpScale;
        protected int interpSize;
        protected int interpSizeSq;
        protected float maxHeight = float.MaxValue;
        protected Voxel maxVoxel = Voxel.Empty;
        protected float minHeight = float.MinValue;

        protected Voxel minVoxel = Voxel.Solid;

        public abstract void GenerateChunk(Chunk chunk);
        public abstract Color32 DensityColor(Voxel voxel);

        public float MinHeight()
        {
            return this.minHeight;
        }

        public float MaxHeight()
        {
            return this.maxHeight;
        }

        public Voxel MinVoxel()
        {
            return this.minVoxel;
        }

        public Voxel MaxVoxel()
        {
            return this.maxVoxel;
        }

        public static void ChunkFillUpdate(Chunk chunk, Voxel voxel)
        {
            switch (chunk.fillType)
            {
                case Chunk.FillType.Empty:
                    if (voxel.IsSolid())
                        chunk.fillType = Chunk.FillType.Mixed;
                    break;

                case Chunk.FillType.Solid:
                    if (!voxel.IsSolid())
                        chunk.fillType = Chunk.FillType.Mixed;
                    break;

                case Chunk.FillType.Null:
                    chunk.fillType = voxel.IsSolid() ? Chunk.FillType.Solid : Chunk.FillType.Empty;
                    break;
            }
        }

        public virtual void Awake()
        {
        }

        // The higher the interpBitStep the less noise samples are taken and more interpolation is used, this is faster but can create less detailed terrain
        protected void SetInterpBitStep(int interpBitStep)
        {
            this.interpBitStep = interpBitStep;
            this.interpSize = (Chunk.SIZE >> interpBitStep) + 1;
            this.interpSizeSq = this.interpSize * this.interpSize;
            this.interpScale = 1f / (1 << interpBitStep);
        }

        protected int InterpLookupIndex(int interpX, int interpY, int interpZ)
        {
            return interpZ + interpY * this.interpSize + interpX * this.interpSizeSq;
        }

        protected float VoxelInterpLookup(int localX, int localY, int localZ, float[] interpLookup)
        {
            var xs = (localX + 0.5f) * this.interpScale;
            var ys = (localY + 0.5f) * this.interpScale;
            var zs = (localZ + 0.5f) * this.interpScale;

            var x0 = FastFloor(xs);
            var y0 = FastFloor(ys);
            var z0 = FastFloor(zs);

            xs = xs - x0;
            ys = ys - y0;
            zs = zs - z0;

            var lookupIndex = this.InterpLookupIndex(x0, y0, z0);

            return Lerp(Lerp(
                Lerp(interpLookup[lookupIndex], interpLookup[lookupIndex + this.interpSizeSq], xs),
                Lerp(interpLookup[lookupIndex + this.interpSize],
                    interpLookup[lookupIndex + this.interpSizeSq + this.interpSize], xs),
                ys), Lerp(
                Lerp(interpLookup[++lookupIndex], interpLookup[lookupIndex + this.interpSizeSq], xs),
                Lerp(interpLookup[lookupIndex + this.interpSize],
                    interpLookup[lookupIndex + this.interpSizeSq + this.interpSize], xs),
                ys), zs);
        }

        private static float Lerp(float a, float b, float t)
        {
            return a + t * (b - a);
        }

        private static int FastFloor(float f)
        {
            return f >= 0.0f ? (int) f : (int) f - 1;
        }
    }
}