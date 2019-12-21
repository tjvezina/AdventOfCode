using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015 {
    public class Day01 : Challenge {
        private string _input;

        private void Init(string input) => _input = input;

        protected override string SolvePart1() {
            int up = _input.Count(c => c == '(');
            int down = _input.Length - up;

            return $"Floor: {up - down}";
        }
        
        protected override string SolvePart2() {
            int floor = 0;

            for (int i = 0; i < _input.Length; ++i) {
                if (_input[i] == '(') ++floor;
                else                  --floor;

                if (floor < 0) {
                    return $"Entered basement at position: {i + 1}";
                }
            }

            return "Did not enter basement.";
        }
    }
}
