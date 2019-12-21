using System;

namespace AdventOfCode {
    public struct Range {
        public int a;
        public int b;
        
        public int min => a < b ? a : b;
        public int max => a > b ? a : b;
        
        public int size => max - min;
        
        public Range(int a, int b) {
            this.a = a;
            this.b = b;
        }
        
        public override string ToString() => $"[{a}, {b}]";

        public bool Contains(int value) => (min <= value && value <= max);
        
        public int Wrap(int value) => MathUtil.Mod(value - a, b - a) + a;
    }
}
