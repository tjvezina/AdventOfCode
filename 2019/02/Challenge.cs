using System;
using AdventOfCode.Year2019.IntCodeV1;

namespace AdventOfCode.Year2019.Day02
{
     public class Challenge : BaseChallenge
     {
        private const int Noun = 12;
        private const int Verb = 02;

        private readonly IntCode _intCode;
        
        public Challenge()
        {
            _intCode = new IntCode();
            _intCode.Load(inputList[0]);
        }

        public override object part1ExpectedAnswer => 5434663;
        public override (string message, object answer) SolvePart1()
        {
            _intCode.Execute(Noun, Verb);

            return ($"Output of {Noun}/{Verb}: ", _intCode[0]);
        }
        
        public override object part2ExpectedAnswer => 4559;
        public override (string message, object answer) SolvePart2()
        {
            const int TargetOutput = 19690720;

            for (int noun = 0; noun <= 99; noun++)
            {
                for (int verb = 0; verb <= 99; verb++)
                {
                    _intCode.Reset();
                    _intCode.Execute(noun, verb);

                    if (_intCode[0] == TargetOutput)
                    {
                        return ("Input found: ", 100 * noun + verb);
                    }
                }
            }

            throw new Exception("Failed to find input to produce target output.");
        }
    }
}
