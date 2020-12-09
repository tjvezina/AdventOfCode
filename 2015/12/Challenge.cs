using System;
using System.Linq;
using TinyJSON;

namespace AdventOfCode.Year2015.Day12
{
    public class Challenge : BaseChallenge
    {
        private readonly Variant _root;

        public Challenge() => _root = JSON.Load(inputList[0]);

        public override object part1ExpectedAnswer => 119433;
        public override (string message, object answer) SolvePart1()
        {
            return ("Sum of all numbers: ", SumNumbers(_root));
        }
        
        public override object part2ExpectedAnswer => 68466;
        public override (string message, object answer) SolvePart2()
        {
            return ("Sum of all numbers: ", SumNumbers(_root, excludeRed:true));
        }

        private int SumNumbers(Variant variant, bool excludeRed = false)
        {
            switch (variant)
            {
                case ProxyNumber number:
                    return (int)Convert.ChangeType(number, typeof(int));
                case ProxyArray array:
                    return array.Sum(v => SumNumbers(v, excludeRed));
                case ProxyObject obj:
                    if (excludeRed && obj.Any(p => p.Value is ProxyString s && $"{s}" == "red"))
                    {
                        return 0;
                    }
                    return obj.Sum(p => SumNumbers(p.Value, excludeRed));
                default:
                    return 0;
            }
        }
    }
}
