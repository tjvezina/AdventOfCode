using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day01 {
    public class Challenge : BaseChallenge {
        private readonly string _input;

        public Challenge() => _input = inputList[0];

        public override string part1ExpectedAnswer => "138";
        public override (string message, object answer) SolvePart1() {
            int up = _input.Count(c => c == '(');
            int down = _input.Length - up;

            return ("Floor: ", up - down);
        }
        
        public override string part2ExpectedAnswer => "1771";
        public override (string message, object answer) SolvePart2() {
            int floor = 0;

            for (int i = 0; i < _input.Length; i++) {
                if (_input[i] == '(') floor++;
                else                 floor--;

                if (floor < 0) {
                    return ("Entered basement at position: ", i + 1);
                }
            }

            throw new Exception("Did not enter basement");
        }
    }
}
