using System;

public struct Range {
    public int a;
    public int b;

    public int min => Math.Min(a, b);
    public int max => Math.Max(a, b);

    public Range(int a, int b) {
        this.a = a;
        this.b = b;
    }

    public bool Contains(int value) {
        return value >= min && value <= max;
    }
}
