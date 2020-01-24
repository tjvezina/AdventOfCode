using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2016.Day06 {
    public class Challenge : BaseChallenge {
        public override void InitPart1() { }

        public override string part1Answer => "gebzfnbt";
        public override (string, object) SolvePart1() {
            char GetMostFrequent(char[] set) => set.GroupBy(c => c).OrderBy(g => g.Count()).Last().First();

            return ("Message: ", GetMessage(charSelector:GetMostFrequent));
        }
        
        public override string part2Answer => "fykjtwyn";
        public override (string, object) SolvePart2() {
            char GetLeastFrequent(char[] set) => set.GroupBy(c => c).OrderBy(g => g.Count()).First().First();

            return ("Message: ", GetMessage(charSelector:GetLeastFrequent));
        }

        private string GetMessage(Func<char[], char> charSelector) {
            int messageLength = inputArray[0].Length;
            
            char[][] characters = new char[messageLength][];
            for (int i = 0; i < messageLength; i++) {
                characters[i] = inputArray.Select(input => input[i]).ToArray();
            }

            char[] message = characters.Select(charSelector).ToArray();

            return new string(message);
        }
    }
}
