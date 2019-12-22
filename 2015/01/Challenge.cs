using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day01 {
    public class Challenge : BaseChallenge {
        public override string SolvePart1() {
            int up = input.Count(c => c == '(');
            int down = input.Length - up;

            return $"Floor: {up - down}";
        }
        
        public override string SolvePart2() {
            int floor = 0;

            for (int i = 0; i < input.Length; ++i) {
                if (input[i] == '(') ++floor;
                else                 --floor;

                if (floor < 0) {
                    return $"Entered basement at position: {i + 1}";
                }
            }

            return "Did not enter basement.";
        }
    }
}
