using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV1;

namespace AdventOfCode.Year2019 {
    public class Day02 : Challenge {
        private const int NOUN = 12;
        private const int VERB = 02;

        private IntCode _intCode;
        
        private void Init(string input) {
            _intCode = new IntCode();
            _intCode.Load(input);
        }

        protected override string SolvePart1() {
            _intCode.Execute(NOUN, VERB);

            return $"Output of {NOUN}/{VERB}: {_intCode[0]}";
        }
        
        protected override string SolvePart2() {
            const int TARGET_OUTPUT = 19690720;

            for (int noun = 0; noun <= 99; ++noun) {
                for (int verb = 0; verb <= 99; ++verb) {
                    _intCode.Reset();
                    _intCode.Execute(noun, verb);

                    if (_intCode[0] == TARGET_OUTPUT) {
                        return $"Input found: {(100 * noun + verb)}";
                    }
                }
            }

            return "Failed to find input to produce target output.";
        }
    }
}
