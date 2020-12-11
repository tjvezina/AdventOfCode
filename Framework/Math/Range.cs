using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AdventOfCode
{
    [PublicAPI]
    public struct Range : IEnumerable<int>
    {
        public int a;
        public int b;
        
        public readonly int min => a < b ? a : b;
        public readonly int max => a > b ? a : b;
        
        public int count => (max - min) + 1;
        
        public Range(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        
        public override string ToString() => $"[{a}, {b}]";

        public readonly bool Contains(int value) => (min <= value && value <= max);
        
        public readonly int Wrap(int value) => MathUtil.Wrap(value, a, b);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator()
        {
            int sign = (a <= b ? 1 : -1);
            for (int i = 0; i < count; i++)
            {
                yield return a + (i * sign);
            }
        }
    }
}
