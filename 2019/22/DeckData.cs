using System.Numerics;

namespace AdventOfCode.Year2019.Day22
{
    public class DeckData
    {
        public BigInteger count;

        private BigInteger _offset = 0;
        public BigInteger offset
        {
            get => _offset;
            set => _offset = Mod(value);
        }

        private BigInteger _increment = 1;
        public BigInteger increment
        {
            get => _increment;
            set => _increment = Mod(value);
        }

        public DeckData(BigInteger count) => this.count = count;

        private BigInteger Mod(BigInteger value)
        {
            BigInteger mod = value % count;
            return (mod >= 0 ? mod : mod + count);
        }

        public override string ToString() => $"({offset,15}, {increment,15})";
    }
}
