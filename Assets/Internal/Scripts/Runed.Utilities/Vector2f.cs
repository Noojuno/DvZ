using UnityEngine;

namespace Runed.Utilities
{
    // A Vector3 struct using ints
    public struct Vector2f
    {
        public float X, Y;

        public static readonly Vector2f zero = new Vector2f(0, 0);
        public static readonly Vector2f one = new Vector2f(1, 1);
        public static readonly Vector2f up = new Vector2f(0, 1);
        public static readonly Vector2f down = new Vector2f(0, -1);
        public static readonly Vector2f left = new Vector2f(-1, 0);
        public static readonly Vector2f right = new Vector2f(1, 0);

        public static readonly Vector2f[] Directions =
        {
            left, right,
            down, up
        };


        public Vector2f(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static float DistanceSquared(Vector2f a, Vector2f b)
        {
            var dx = b.X - a.X;
            var dy = b.Y - a.Y;
            return dx * dx + dy * dy;
        }

        public float DistanceSquared(Vector2f v)
        {
            return DistanceSquared(this, v);
        }

        private const int X_PRIME = 1619;
        private const int Y_PRIME = 31337;

        public override int GetHashCode()
        {
            return ((int)this.X * X_PRIME) ^ ((int)this.Y * Y_PRIME);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2f))
                return false;
            var vector = (Vector2f) other;
            return this.X == vector.X && this.Y == vector.Y;
        }

        public override string ToString()
        {
            return "Vector2f(" + this.X + " " + this.Y + ")";
        }

        public static bool operator ==(Vector2f a, Vector2f b)
        {
            return a.X == b.X &&
                   a.Y == b.Y;
        }

        public static bool operator !=(Vector2f a, Vector2f b)
        {
            return a.X != b.X ||
                   a.Y != b.Y;
        }

        public static bool operator >=(Vector2f a, float b)
        {
            return a.X >= b &&
                   a.Y >= b;
        }

        public static bool operator <=(Vector2f a, float b)
        {
            return a.X <= b &&
                   a.Y <= b;
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2f operator *(Vector2f a, float b)
        {
            return new Vector2f(a.X * b, a.Y * b);
        }

        public static Vector2f operator /(Vector2f a, float b)
        {
            return new Vector2f(a.X / b, a.Y / b);
        }

        public static Vector2f operator %(Vector2f a, float b)
        {
            return new Vector2f(a.X % b, a.Y % b);
        }

        // UNITY METHODS

        public Vector2f(Vector2 v3)
        {
            this.X = Mathf.RoundToInt(v3.x);
            this.Y = Mathf.RoundToInt(v3.y);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(this.X, this.Y);
        }

        public static Vector2 operator *(Vector2f a, Vector2 b)
        {
            return new Vector2(a.X * b.x, a.Y * b.y);
        }

        public static explicit operator Vector2(Vector2f v)
        {
            return new Vector2(v.X, v.Y);
        }
    }
}