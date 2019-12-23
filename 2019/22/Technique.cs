using System.Numerics;

namespace AdventOfCode.Year2019.Day22 {
    public abstract class Technique {
        public abstract BigInteger Apply(BigInteger card, BigInteger count);
        public abstract void Apply(DeckData deck);
    }

    public class ReverseTechnique : Technique {
        public override BigInteger Apply(BigInteger card, BigInteger count) => count - 1 - card;

        public override void Apply(DeckData deck) {
            deck.increment *= -1;
            deck.offset += deck.increment;
        }
    }

    public class CutTechnique : Technique {
        public BigInteger pos { get; }

        public CutTechnique(BigInteger pos) => this.pos = pos;

        public override BigInteger Apply(BigInteger card, BigInteger count) => (card - pos) % count;

        public override void Apply(DeckData deck) {
            deck.offset += deck.increment * pos;
        }
    }

    public class DistributeTechnique : Technique {
        public BigInteger interval { get; }

        public DistributeTechnique(BigInteger interval) => this.interval = interval;

        public override BigInteger Apply(BigInteger card, BigInteger count) => (card * interval) % count;

        public override void Apply(DeckData deck) {
            // Shortcut only works when deck.count is prime
            BigInteger r = MathUtil.ModPower(interval, deck.count - 2, deck.count);
            deck.increment = (BigInteger)((deck.increment * r) % deck.count);
        }
    }
}
