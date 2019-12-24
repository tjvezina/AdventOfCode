using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV1;

namespace AdventOfCode.Year2019.Day02 {
     public class Challenge : BaseChallenge {
        private const int NOUN = 12;
        private const int VERB = 02;

        private IntCode _intCode;
        
        public override void InitPart1() {
            _intCode = new IntCode();
            _intCode.Load(input);
        }

        public override string part1Answer => "5434663";
        public override (string, object) SolvePart1() {
            _intCode.Execute(NOUN, VERB);

            return ($"Output of {NOUN}/{VERB}: ", _intCode[0]);
        }
        
        public override string part2Answer => "4559";
        public override (string, object) SolvePart2() {
            const int TARGET_OUTPUT = 19690720;

            for (int noun = 0; noun <= 99; ++noun) {
                for (int verb = 0; verb <= 99; ++verb) {
                    _intCode.Reset();
                    _intCode.Execute(noun, verb);

                    if (_intCode[0] == TARGET_OUTPUT) {
                        return ("Input found: ", 100 * noun + verb);
                    }
                }
            }

            throw new Exception("Failed to find input to produce target output.");
        }
    }
}
