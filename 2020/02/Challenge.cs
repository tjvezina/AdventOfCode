using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day02
{
    public class Challenge : BaseChallenge
    {
        public override string part1ExpectedAnswer => "474";
        public override (string message, object answer) SolvePart1()
        {
            int validCount = 0;

            foreach (string input in inputList)
            {
                Match match = Regex.Match(input, @"(\d+)-(\d+) ([a-z]): ([a-z]+)");
                int min = int.Parse(match.Groups[1].Value);
                int max = int.Parse(match.Groups[2].Value);
                char ruleChar = match.Groups[3].Value[0];
                string password = match.Groups[4].Value;

                int ruleCount = Regex.Matches(password, $"{ruleChar}").Count;

                if (min <= ruleCount && ruleCount <= max)
                {
                    validCount++;
                }
            }

            return ("There are {0} valid passwords", validCount);
        }
        
        public override string part2ExpectedAnswer => "745";
        public override (string message, object answer) SolvePart2()
        {
            int validCount = 0;

            foreach (string input in inputList)
            {
                Match match = Regex.Match(input, @"(\d+)-(\d+) ([a-z]): ([a-z]+)");
                int iA = int.Parse(match.Groups[1].Value) - 1;
                int iB = int.Parse(match.Groups[2].Value) - 1;
                char ruleChar = match.Groups[3].Value[0];
                string password = match.Groups[4].Value;

                if (password.Length > Math.Max(iA, iB) && (password[iA] == ruleChar ^ password[iB] == ruleChar))
                {
                    validCount++;
                }
            }

            return ("There are {0} valid passwords", validCount);
        }
    }
}
