using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV2;

namespace AdventOfCode.Year2019.Day05 {
     public class Challenge : BaseChallenge {
        public const int InputTest = 1;
        public const int InputRun = 5;

        private IntCode _intCode;
        private int _input;
        private List<int> _outputs = new List<int>();

        public override void InitPart1() {
            _intCode = new IntCode();
            _intCode.OnInput += HandleOnInput;
            _intCode.OnOutput += HandleOnOutput;
            _intCode.Load(input);
        }

        public override string part1Answer => "10987514";
        public override (string, object) SolvePart1() {
            _input = InputTest;
            _intCode.Execute();
            int output = _outputs.SingleOrDefault(o => o != 0);
            if (output != 0) {
                return ("Final test output: ", output);
            }
            throw new Exception("At least one system failed, check outputs for more info");
        }
        
        public override void InitPart2() {
            _intCode.Reset();
            _outputs.Clear();
        }

        public override string part2Answer => "14195011";
        public override (string, object) SolvePart2() {
            _input = InputRun;
            _intCode.Execute();
            return ("Final output: ", _outputs.Single(o => o != 0));
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
