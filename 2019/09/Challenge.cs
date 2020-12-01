using System;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day09
{
     public class Challenge : BaseChallenge
     {
        private readonly IntCode _intCode;

        private long _lastOutput;

        public Challenge()
        {
            _intCode = new IntCode(inputList[0]);
            _intCode.OnOutput += HandleOnOutput;
        }

        public override string part1ExpectedAnswer => "4261108180";
        public override (string message, object answer) SolvePart1()
        {
            _intCode.Begin();
            _intCode.Input(1);
            return ("Final output: ", _lastOutput);
        }
        
        public override string part2ExpectedAnswer => "77944";
        public override (string message, object answer) SolvePart2()
        {
            _intCode.Reset();
            _intCode.Begin();
            _intCode.Input(2);
            return ("Final output: ", _lastOutput);
        }

        private void HandleOnOutput(long output)
        {
            _lastOutput = output;
            Console.WriteLine($"Output: {output}");
        }
    }
}
