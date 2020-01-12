using System;
using System.Collections.Generic;
using System.Linq;
using TinyJSON;

namespace AdventOfCode.Year2015.Day12 {
     public class Challenge : BaseChallenge {
        private Variant _root;

        public override void InitPart1() {
            _root = JSON.Load(input);
        }

        public override string part1Answer => "119433";
        public override (string, object) SolvePart1() {
            return ("Sum of all numbers: ", SumNumbers(_root));
        }
        
        public override string part2Answer => "68466";
        public override (string, object) SolvePart2() {
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