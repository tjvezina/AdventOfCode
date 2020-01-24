using System;
using System.Collections;
using System.Collections.Generic;

namespace AdventOfCode {
    public struct Range : IEnumerable<int> {
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
        
        public int Wrap(int value) => MathUtil.Wrap(value, a, b);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() {
            int sign = (a <= b ? 1 : -1);
            for (int i = 0; i < size; i++) {
                yield return a + (i * sign);
            }
        }
    }
}
