using System;
using System.Linq;
using TinyJSON;

namespace AdventOfCode.Year2015.Day12 {
    public class Challenge : BaseChallenge {
        private readonly Variant _root;

        public Challenge() => _root = JSON.Load(inputList[0]);

        public override string part1ExpectedAnswer => "119433";
        public override (string message, object answer) SolvePart1() {
            return ("Sum of all numbers: ", SumNumbers(_root));
        }
        
        public override string part2ExpectedAnswer => "68466";
        public override (string message, object answer) SolvePart2() {
            return ("Sum of all numbers: ", SumNumbers(_root, excludeRed:true));
        }

        private int SumNumbers(Variant variant, bool excludeRed = false) {
            if (variant is ProxyNumber number) {
                return (int)Convert.ChangeType(number, typeof(int));
            }

            if (variant is ProxyArray array) {
                return array.Sum(v => SumNumbers(v, excludeRed));
            }

            if (variant is ProxyObject obj) {
                if (excludeRed && obj.Any(pair => pair.Value is ProxyString str && str.ToString() == "red")) {
                    return 0;
                }

                return obj.Sum(pair => SumNumbers(pair.Value, excludeRed));
            }

            return 0;
        }
    }
}
