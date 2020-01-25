using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day04 {
     public class Challenge : BaseChallenge {
        private const int Digits = 6;
        private const int RangeMin = 146810;
        private const int RangeMax = 612564;

        public override string part1ExpectedAnswer => "1748";
        public override (string message, object answer) SolvePart1() {
            int matchCount = Enumerable.Range(RangeMin, RangeMax - RangeMin).Count(MatchesRuleSet1);

            return ("Valid passwords (rule set 1): ", matchCount);
        }
        
        public override string part2ExpectedAnswer => "1180";
        public override (string message, object answer) SolvePart2() {
            int matchCount = Enumerable.Range(RangeMin, RangeMax - RangeMin).Count(MatchesRuleSet2);

            return ("Valid passwords (rule set 2): ", matchCount);
        }

        private bool MatchesRuleSet1(int password) => IsMatch(password, allow3Consecutive:true);
        private bool MatchesRuleSet2(int password) => IsMatch(password, allow3Consecutive:false);
        private bool IsMatch(int password, bool allow3Consecutive) {
            int doubleCount = 0;
            bool containsDouble = false;

            void CheckDouble() => containsDouble |= (doubleCount == 1 || (doubleCount > 1 && allow3Consecutive));

            int lastDigit = password % 10;
            for (int i = 1; i < Digits; i++) {
                password /= 10;
                int nextDigit = password % 10;
                
                if (nextDigit > lastDigit) return false;

                if (nextDigit == lastDigit) {
                    doubleCount++;
                } else {
                    CheckDouble();
                    doubleCount = 0;
                }

                lastDigit = nextDigit;
            }

            // Check if the final pair of digits was a double
            CheckDouble();

            return containsDouble;
        }
    }
}
