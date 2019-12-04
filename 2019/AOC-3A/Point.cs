using System;

public struct Point {
    public static readonly Point zero  = new Point( 0,  0);
    public static readonly Point right = new Point( 1,  0);
    public static readonly Point left  = new Point(-1,  0);
    public static readonly Point up    = new Point( 0,  1);
    public static readonly Point down  = new Point( 0, -1);

    public int x;
    public int y;

    public int TaxiDist => Math.Abs(x) + Math.Abs(y);
    public int this[int i]
    {
        get {
            if (i == 0) return x;
            if (i == 1) return y;
            throw new IndexOutOfRangeException();
        }
        set {
            if (i == 0) x = value;
            else if (i == 1) y = value;
            else throw new IndexOutOfRangeException();
        }
    }

    public Point(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static Point operator-(Point a) => new Point(-a.x, -a.y);
    public static Point operator+(Point a, Point b) => new Point(a.x + b.x, a.y + b.y);
    public static Point operator-(Point a, Point b) => a + -b;
    public static Point operator*(Point p, int m) => new Point(p.x * m, p.y * m);

    public override string ToString() => $"({x},{y})";
}

