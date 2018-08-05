using UnityEngine;

namespace Runed.Utilities
{
    // A Vector3 struct using ints
    public struct Vector3i
    {
        public int X, Y, Z;

        public static readonly Vector3i zero = new Vector3i(0, 0, 0);
        public static readonly Vector3i one = new Vector3i(1, 1, 1);
        public static readonly Vector3i forward = new Vector3i(0, 0, 1);
        public static readonly Vector3i back = new Vector3i(0, 0, -1);
        public static readonly Vector3i up = new Vector3i(0, 1, 0);
        public static readonly Vector3i down = new Vector3i(0, -1, 0);
        public static readonly Vector3i left = new Vector3i(-1, 0, 0);
        public static readonly Vector3i right = new Vector3i(1, 0, 0);

        public static readonly Vector3i[] Directions =
        {
            left, right,
            down, up,
            back, forward
        };

        public Vector3i(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3i(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Z = 0;
        }

        public static int DistanceSquared(Vector3i a, Vector3i b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            var dz = b.Z - a.Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public int DistanceSquared(Vector3i v)
        {
            return DistanceSquared(this, v);
        }

        private const int X_PRIME = 1619;
        private const int Y_PRIME = 31337;
        private const int Z_PRIME = 6971;

        public override int GetHashCode()
        {
            return (this.X * X_PRIME) ^ (this.Y * Y_PRIME) ^ (this.Z * Z_PRIME);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3i))
                return false;
            var vector = (Vector3i) other;
            return this.X == vector.X && this.Y == vector.Y && this.Z == vector.Z;
        }

        public override string ToString()
        {
            return "Vector3i(" + this.X + " " + this.Y + " " + this.Z + ")";
        }

        public static bool operator ==(Vector3i a, Vector3i b)
        {
            return a.X == b.X &&
                   a.Y == b.Y &&
                   a.Z == b.Z;
        }

        public static bool operator !=(Vector3i a, Vector3i b)
        {
            return a.X != b.X ||
                   a.Y != b.Y ||
                   a.Z != b.Z;
        }

        public static bool operator >=(Vector3i a, int b)
        {
            return a.X >= b &&
                   a.Y >= b &&
                   a.Z >= b;
        }

        public static bool operator <=(Vector3i a, int b)
        {
            return a.X <= b &&
                   a.Y <= b &&
                   a.Z <= b;
        }

        public static Vector3i operator -(Vector3i a, Vector3i b)
        {
            return new Vector3i(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3i operator +(Vector3i a, Vector3i b)
        {
            return new Vector3i(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3i operator *(Vector3i a, int b)
        {
            return new Vector3i(a.X * b, a.Y * b, a.Z * b);
        }

        public static Vector3i operator /(Vector3i a, int b)
        {
            return new Vector3i(a.X / b, a.Y / b, a.Z / b);
        }

        public static Vector3i operator %(Vector3i a, int b)
        {
            return new Vector3i(a.X % b, a.Y % b, a.Z % b);
        }

        // UNITY METHODS

        public Vector3i(Vector3 v3)
        {
            this.X = Mathf.RoundToInt(v3.x);
            this.Y = Mathf.RoundToInt(v3.y);
            this.Z = Mathf.RoundToInt(v3.z);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }

        public static Vector3 operator *(Vector3i a, Vector3 b)
        {
            return new Vector3(a.X * b.x, a.Y * b.y, a.Z * b.z);
        }

        public static explicit operator Vector3(Vector3i v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
    }
}