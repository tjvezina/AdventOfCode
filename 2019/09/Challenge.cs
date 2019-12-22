using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day09 {
     public class Challenge : BaseChallenge {
        private IntCode _intCode;

        public override void InitPart1() {
            _intCode = new IntCode(input);
            _intCode.OnOutput += (o => Console.WriteLine("Output: " + o));
        }

        public override string SolvePart1() {
            _intCode.Begin();
            _intCode.Input(1);
            return null;
        }
        
        public override void InitPart2() {
            _intCode.Reset();
        }

        public override string SolvePart2() {
            _intCode.Begin();
            _intCode.Input(2);
            return null;
        }
    }
}
