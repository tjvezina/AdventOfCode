using System;

namespace AdventOfCode
{
    public struct Point
    {
        public static Point zero => new Point(0, 0);
        public static Point one => new Point(1, 1);

        public static implicit operator Direction(Point point) => point.ToDirection();
        public static implicit operator Point(Direction direction) => direction.ToPoint();

        public static int TaxiDist(Point a, Point b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

        public int x;
        public int y;

        public int taxiLength => TaxiDist(this, zero);

        public int this[int i]
        {
            get
            {
                return i switch
                {
                    0 => x, 1 => y,
                    _ => throw new IndexOutOfRangeException()
                };
            }
            set
            {
                if (i == 0) x = value;
                else if (i == 1) y = value;
                else throw new IndexOutOfRangeException();
            }
        }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public static Point operator-(Point a) => new Point(-a.x, -a.y);
        public static Point operator+(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
        public static Point operator-(Point a, Point b) => a + -b;
        public static Point operator*(Point p, int m) => new Point(p.x * m, p.y * m);
        public static Point operator/(Point p, int d) => new Point(p.x / d, p.y / d);
        public static Point operator*(Point a, Point b) => new Point(a.x * b.x, a.y * b.y);

        public bool Equals(Point p) => x == p.x && y == p.y;
        public override bool Equals(object obj) => obj is Point p && Equals(p);
        public override int GetHashCode() => x ^ y;
        public static bool operator==(Point a, Point b) => a.Equals(b);
        public static bool operator!=(Point a, Point b) => !(a == b);

        public override string ToString() => $"({x},{y})";
    }
}
