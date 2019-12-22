using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day10 {
     public class Challenge : BaseChallenge {
        private const string INPUT = "1113122113";

        private DNA _dna;
        private int[] _elementInput;

        public override void InitPart1() {
            _dna = new DNA(LoadFile("DNA.txt"));

            _elementInput = new int[_dna.elementCount];
            _elementInput[86] = 1; // Element #87 (index 86) is equivalent to the input string 1113122113
        }

        public override string SolvePart1() {
            PrintIteration(40);
            return null;
        }
        
        public override string SolvePart2() {
            PrintIteration(50);
            PrintIteration(1000);
            PrintIteration(1000000);
            return null;
        }

        private void PrintIteration(int iter) {
            ulong iterLength = _dna.GetIterationLength(_elementInput, iter);
            Console.WriteLine($"Iter {iter:N0}: {iterLength}");
    }
    }
}
