using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day05 {
     public class Challenge : BaseChallenge {
        private static readonly char[] VOWELS = new char[] { 'a', 'e', 'i', 'o', 'u' };
        private static readonly List<string> BAD_STRINGS = new List<string> { "ab", "cd", "pq", "xy" };

        public override string part1Answer => "236";
        public override (string, object) SolvePart1() => ("Nice strings (rule set A): ", inputSet.Count(IsNiceRuleSetA));
        
        public override string part2Answer => "51";
        public override (string, object) SolvePart2() => ("Nice strings (rule set B): ", inputSet.Count(IsNiceRuleSetB));

        private bool IsNiceRuleSetA(string str) {
            // If the string contains any of the bad strings, it is naughty
            if (BAD_STRINGS.Any(s => str.Contains(s))) return false;
            // If the string contains less than 3 vowels, it is naughty
            if (VOWELS.Sum(v => str.Count(c => c == v)) < 3) return false;

            char lastChar = str[0];
            for (int i = 1; i < str.Length; ++i) {
                char nextChar = str[i];

                // If the string contains 2 consecutive identical letters, it is nice
                if (lastChar == nextChar) return true;

                lastChar = nextChar;
            }

            return false;
        }

        private bool IsNiceRuleSetB(string str) {
            return CheckRule1(str) && CheckRule2(str);
        }

        private bool CheckRule1(string str) {
            for (int i = 0; i < str.Length - 3; ++i) {
                string pairA = str.Substring(i, 2);
                for (int j = i + 2; j < str.Length - 1; ++j) {
                    string pairB = str.Substring(j, 2);
                    if (pairA == pairB) {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckRule2(string str) {
            for (int i = 0; i < str.Length - 2; ++i) {
                if (str[i] == str[i + 2]) {
                    return true;
                }
            }

            return false;
        }
    }
}