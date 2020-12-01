using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV2;

namespace AdventOfCode.Year2019.Day05
{
     public class Challenge : BaseChallenge
     {
        public const int InputTest = 1;
        public const int InputRun = 5;

        private readonly IntCode _intCode;
        private int _input;
        private List<int> _outputs = new List<int>();

        public Challenge()
        {
            _intCode = new IntCode();
            _intCode.OnInput += HandleOnInput;
            _intCode.OnOutput += HandleOnOutput;
            _intCode.Load(inputList[0]);
        }

        public override string part1ExpectedAnswer => "10987514";
        public override (string message, object answer) SolvePart1()
        {
            _input = InputTest;
            _intCode.Execute();
            int output = _outputs.SingleOrDefault(o => o != 0);
            if (output != 0)
            {
                return ("Final test output: ", output);
            }
            throw new Exception("At least one system failed, check outputs for more info");
        }
        
        public override string part2ExpectedAnswer => "14195011";
        public override (string message, object answer) SolvePart2()
        {
            _intCode.Reset();
            _outputs.Clear();

            _input = InputRun;
            _intCode.Execute();
            return ("Final output: ", _outputs.Single(o => o != 0));
        }

        private int HandleOnInput()
        {
            Console.WriteLine($"Input : {_input}");
            return _input;
        }

        private void HandleOnOutput(int output)
        {
            Console.WriteLine($"Output: {output}");
            _outputs.Add(output);
        }
    }
}
