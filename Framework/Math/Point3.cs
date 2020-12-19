using System;
using JetBrains.Annotations;

namespace AdventOfCode
{
    [PublicAPI]
    public struct Point3
    {
        public static Point3 zero => new Point3(0, 0, 0);

        public static implicit operator Point3(Point p) => new Point3(p.x, p.y, 0);
        public static explicit operator Point(Point3 p) => new Point(p.x, p.y);

        public int x;
        public int y;
        public int z;

        public int this[int i]
        {
            get
            {
                return i switch
                {
                    0 => x, 1 => y, 2 => z,
                    _ => throw new IndexOutOfRangeException()
                };
            }
            set
            {
                if (i == 0) x = value;
                else if (i == 1) y = value;
                else if (i == 2) z = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public Point3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Point3 operator-(Point3 p) => new Point3(-p.x, -p.y, -p.z);
        public static Point3 operator+(Point3 a, Point3 b) => new Point3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Point3 operator-(Point3 a, Point3 b) => a + -b;
        public static Point3 operator*(Point3 p, int m) => new Point3(p.x * m, p.y * m, p.z * m);
        public static Point3 operator/(Point3 p, int d) => new Point3(p.x / d, p.y / d, p.z / d);
        public static Point3 operator*(Point3 a, Point3 b) => new Point3(a.x * b.x, a.y * b.y, a.z * b.z);

        public bool Equals(Point3 p) => x == p.x && y == p.y && z == p.z;
        public override bool Equals(object obj) => obj is Point3 p && Equals(p);
        public override int GetHashCode() => x ^ y ^ z;
        public static bool operator==(Point3 a, Point3 b) => a.Equals(b);
        public static bool operator!=(Point3 a, Point3 b) => !(a == b);

        public override string ToString() => $"({x},{y},{z})";
    }
}
