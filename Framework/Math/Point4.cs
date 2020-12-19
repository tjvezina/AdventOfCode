using System;
using JetBrains.Annotations;

namespace AdventOfCode
{
    [PublicAPI]
    public struct Point4
    {
        public static Point4 zero => new Point4(0, 0, 0, 0);

        public static implicit operator Point4(Point3 p) => new Point4(p.x, p.y, p.z, 0);
        public static implicit operator Point4(Point p) => new Point4(p.x, p.y, 0, 0);
        public static implicit operator Point3(Point4 p) => new Point3(p.x, p.y, p.z);
        public static implicit operator Point(Point4 p) => new Point(p.x, p.y);

        public int x;
        public int y;
        public int z;
        public int w;

        public int this[int i]
        {
            get
            {
                return i switch
                {
                    0 => x, 1 => y, 2 => z, 3 => w,
                    _ => throw new IndexOutOfRangeException()
                };
            }
            set
            {
                if (i == 0) x = value;
                else if (i == 1) y = value;
                else if (i == 2) z = value;
                else if (i == 3) w = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public Point4(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Point4 operator-(Point4 p) => new Point4(-p.x, -p.y, -p.z, -p.w);
        public static Point4 operator+(Point4 a, Point4 b) => new Point4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Point4 operator-(Point4 a, Point4 b) => a + -b;
        public static Point4 operator*(Point4 p, int m) => new Point4(p.x * m, p.y * m, p.z * m, p.w * m);
        public static Point4 operator/(Point4 p, int d) => new Point4(p.x / d, p.y / d, p.z / d, p.w / d);
        public static Point4 operator*(Point4 a, Point4 b) => new Point4(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);

        public bool Equals(Point4 p) => x == p.x && y == p.y && z == p.z && w == p.w;
        public override bool Equals(object obj) => obj is Point4 p && Equals(p);
        public override int GetHashCode() => x ^ y ^ z ^ w;
        public static bool operator==(Point4 a, Point4 b) => a.Equals(b);
        public static bool operator!=(Point4 a, Point4 b) => !(a == b);

        public override string ToString() => $"({x},{y},{z},{w})";
    }
}
