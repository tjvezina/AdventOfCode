using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV2;

namespace AdventOfCode.Year2019 {
    public class Day05 : Challenge {
        public const int INPUT_TEST = 1;
        public const int INPUT_RUN = 5;

        private IntCode _intCode;
        private int _input;
        private List<int> _outputs = new List<int>();

        private void Init(string input) {
            _intCode = new IntCode();
            _intCode.OnInput += HandleOnInput;
            _intCode.OnOutput += HandleOnOutput;
            _intCode.Load(input);
        }

        protected override void Reset() {
            _intCode.Reset();
            _outputs.Clear();
        }

        protected override string SolvePart1() {
            _input = INPUT_TEST;
            _intCode.Execute();
            int output = _outputs.SingleOrDefault(o => o != 0);
            if (output != 0) {
                return $"{output}";
            }
            return "At least one system failed, check outputs for more info";
        }
        
        protected override string SolvePart2() {
            _input = INPUT_RUN;
            _intCode.Execute();
            return $"{_outputs.Single(o => o != 0)}";
        }

        private int HandleOnInput() {
            Console.WriteLine($"Input : {_input}");
            return _input;
        }

        private void HandleOnOutput(int output) {
            Console.WriteLine($"Output: {output}");
            _outputs.Add(output);
        }
    }
}
