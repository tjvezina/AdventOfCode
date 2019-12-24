using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day09 {
     public class Challenge : BaseChallenge {
        private IntCode _intCode;

        private long _lastOutput;

        public override void InitPart1() {
            _intCode = new IntCode(input);
            _intCode.OnOutput += HandleOnOutput;
        }

        public override string part1Answer => "4261108180";
        public override (string, object) SolvePart1() {
            _intCode.Begin();
            _intCode.Input(1);
            return ("Final output: ", _lastOutput);
        }
        
        public override void InitPart2() {
            _intCode.Reset();
        }

        public override string part2Answer => "77944";
        public override (string, object) SolvePart2() {
            _intCode.Begin();
            _intCode.Input(2);
            return ("Final output: ", _lastOutput);
        }

        private void HandleOnOutput(long output) {
            _lastOutput = output;
            Console.WriteLine($"Output: {output}");
        }
    }
}
