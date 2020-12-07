using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019.Day22
{
    public class Challenge : BaseChallenge
    {
        private List<Technique> _techniques = new List<Technique>();

        public Challenge()
        {
            foreach (string input in inputList)
            {
                if (input == "deal into new stack")
                {
                    _techniques.Add(new ReverseTechnique());
                    continue;
                }

                Match match = Regex.Match(input, @"cut (-?\d+)");
                if (match.Success)
                {
                    _techniques.Add(new CutTechnique(int.Parse(match.Groups[1].Value)));
                    continue;
                }

                match = Regex.Match(input, @"deal with increment (\d+)");
                if (match.Success)
                {
                    _techniques.Add(new DistributeTechnique(int.Parse(match.Groups[1].Value)));
                    continue;
                }

                throw new Exception($"Failed to parse technique: {input}");
            }
        }

        public override object part1ExpectedAnswer => 4684;
        public override (string message, object answer) SolvePart1()
        {
            const long DeckSize = 10_007;

            BigInteger card = 2019;
            foreach (Technique technique in _techniques)
            {
                card = technique.Apply(card, DeckSize);
            }
            return ("Final card position: ", card);
        }
        
        public override object part2ExpectedAnswer => 452290953297;
        public override (string message, object answer) SolvePart2()
        {
            const long DeckSize  = 119_315_717_514_047;
            const long Iterations = 101_741_582_076_661;

            DeckData passData = new DeckData(DeckSize);

            // Determine the result of a single shuffle pass on the deck state
            foreach (Technique technique in _techniques)
            {
                technique.Apply(passData);
            }

            // Calculate the result of applying the shuffle many times
            DeckData finalData = new DeckData(DeckSize);
            finalData.increment = MathUtil.ModPower(passData.increment, Iterations, DeckSize);
            finalData.offset = (passData.offset * (1 - finalData.increment) *
                               MathUtil.ModPower(1 - passData.increment, DeckSize - 2, DeckSize)) % DeckSize;

            BigInteger card = (finalData.offset + (finalData.increment * 2020)) % DeckSize;

            return ($"Card 2020 after {Iterations} shuffles:", card);
        }
    }
}
