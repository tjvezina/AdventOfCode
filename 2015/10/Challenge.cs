using System;

namespace AdventOfCode.Year2015.Day10
{
    public class Challenge : BaseChallenge
    {
        private const string Input = "1113122113";

        private readonly DNA _dna;
        private readonly int[] _elementInput;

        public Challenge()
        {
            _dna = new DNA(LoadFileLines("DNA.txt"));

            _elementInput = new int[_dna.elementCount];
            _elementInput[86] = 1; // Element #87 (index 86) is equivalent to the input string 1113122113
        }

        public override string part1ExpectedAnswer => "360154";
        public override (string message, object answer) SolvePart1()
        {
            const int Iteration = 40;
            return ($"Iteration {Iteration}: ", GetIterationLength(Iteration));
        }
        
        public override string part2ExpectedAnswer => "5103798";
        public override (string message, object answer) SolvePart2()
        {
            const int Iteration = 50;

            void PrintIter(int iter) => Console.WriteLine($"Iteration {iter:N0}: {GetIterationLength(iter)}");

            // Some huge values, just for fun
            PrintIter(1_000);
            PrintIter(1_000_000);

            return ($"Iteration {Iteration}: ", GetIterationLength(Iteration));
        }

        private ulong GetIterationLength(int iter) => _dna.GetIterationLength(_elementInput, iter);
    }
}
