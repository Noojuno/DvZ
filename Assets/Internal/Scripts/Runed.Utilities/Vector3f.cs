using UnityEngine;

namespace Runed.Utilities
{
    // A Vector3 struct using ints
    public struct Vector3f
    {
        public float X, Y, Z;

        public static readonly Vector3f zero = new Vector3f(0, 0, 0);
        public static readonly Vector3f one = new Vector3f(1, 1, 1);
        public static readonly Vector3f forward = new Vector3f(0, 0, 1);
        public static readonly Vector3f back = new Vector3f(0, 0, -1);
        public static readonly Vector3f up = new Vector3f(0, 1, 0);
        public static readonly Vector3f down = new Vector3f(0, -1, 0);
        public static readonly Vector3f left = new Vector3f(-1, 0, 0);
        public static readonly Vector3f right = new Vector3f(1, 0, 0);

        public static readonly Vector3f[] Directions =
        {
            left, right,
            down, up,
            back, forward
        };

        public Vector3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3f(float x, float y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        public static float DistanceSquared(Vector3f a, Vector3f b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var dz = b.Z - a.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public float DistanceSquared(Vector3f v)
        {
            return DistanceSquared(this, v);
        }

        private const int X_PRIME = 1619;
        private const int Y_PRIME = 31337;
        private const int Z_PRIME = 6971;

        public override int GetHashCode()
        {
            return ((int)this.X * X_PRIME) ^ ((int)this.Y * Y_PRIME) ^ ((int)this.Z * Z_PRIME);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3f))
                return false;
            var vector = (Vector3f) other;
            return this.X == vector.X && this.Y == vector.Y && this.Z == vector.Z;
        }

        public override string ToString()
        {
            return "Vector3f(" + this.X + " " + this.Y + " " + this.Z + ")";
        }

        public static bool operator ==(Vector3f a, Vector3f b)
        {
            return a.X == b.X &&
                   a.Y == b.Y &&
                   a.Z == b.Z;
        }

        public static bool operator !=(Vector3f a, Vector3f b)
        {
            return a.X != b.X ||
                   a.Y != b.Y ||
                   a.Z != b.Z;
        }

        public static bool operator >=(Vector3f a, float b)
        {
            return a.X >= b &&
                   a.Y >= b &&
                   a.Z >= b;
        }

        public static bool operator <=(Vector3f a, float b)
        {
            return a.X <= b &&
                   a.Y <= b &&
                   a.Z <= b;
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return new Vector3f(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3f operator /(Vector3f a, float b)
        {
            return new Vector3f(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vector3f operator %(Vector3f a, float b)
        {
            return new Vector3f(a.X % b, a.Y % b, a.Z % b);
        }

        // UNITY METHODS

        public Vector3f(Vector3 v3)
        {
            this.X = Mathf.RoundToInt(v3.x);
            this.Y = Mathf.RoundToInt(v3.y);
            this.Z = Mathf.RoundToInt(v3.z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }

        public static Vector3 operator *(Vector3f a, Vector3 b)
        {
            return new Vector3(a.X * b.x, a.Y * b.y, a.Z * b.z);
        }

        public static explicit operator Vector3(Vector3f v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
    }
}