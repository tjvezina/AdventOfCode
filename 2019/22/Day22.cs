using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019.Day22 {
    public class Challenge : BaseChallenge {
        private List<Technique> _techniques = new List<Technique>();

        public override void InitPart1() {
            foreach (string input in inputSet) {
                if (input == "deal into new stack") {
                    _techniques.Add(new ReverseTechnique());
                    continue;
                }

                Match match = Regex.Match(input, @"cut (-?\d+)");
                if (match.Success) {
                    _techniques.Add(new CutTechnique(int.Parse(match.Groups[1].Value)));
                    continue;
                }

                match = Regex.Match(input, @"deal with increment (\d+)");
                if (match.Success) {
                    _techniques.Add(new DistributeTechnique(int.Parse(match.Groups[1].Value)));
                    continue;
                }

                throw new Exception($"Failed to parse technique: {input}");
            }
        }

        public override string SolvePart1() {
            const long DECK_SIZE = 10_007;

            BigInteger card = 2019;
            foreach (Technique technique in _techniques) {
                card = technique.Apply(card, DECK_SIZE);
            }
            return $"Final card position: {card}";
        }
        
        public override string SolvePart2() {
            const long DECK_SIZE  = 119_315_717_514_047;
            const long ITERATIONS = 101_741_582_076_661;

            DeckData passData = new DeckData(DECK_SIZE);

            // Determine the result of a single shuffle pass on the deck state
            foreach (Technique technique in _techniques) {
                technique.Apply(passData);
            }

            // Calculate the result of applying the shuffle many times
            DeckData finalData = new DeckData(DECK_SIZE);
            finalData.increment = MathUtil.ModPower(passData.increment, ITERATIONS, DECK_SIZE);
            finalData.offset = (passData.offset * (1 - finalData.increment) *
                               MathUtil.ModPower(1 - passData.increment, DECK_SIZE - 2, DECK_SIZE)) % DECK_SIZE;

            BigInteger card = (finalData.offset + (finalData.increment * 2020)) % DECK_SIZE;

            return $"Card 2020 after {ITERATIONS} shuffles: {card}";
        }
    }
}
