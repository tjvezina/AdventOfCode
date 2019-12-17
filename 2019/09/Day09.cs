using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019 {
    public class Day09 : Challenge {
        private IntCode _intCode;

        private void Init(string input) {
            _intCode = new IntCode(input);
            _intCode.OnOutput += (o => Console.WriteLine("Output: " + o));
        }

        protected override void Reset() {
            _intCode.Reset();
        }

        protected override string SolvePart1() {
            _intCode.Begin();
            _intCode.Input(1);
            return null;
        }
        
        protected override string SolvePart2() {
            _intCode.Begin();
            _intCode.Input(2);
            return null;
        }
    }
}
