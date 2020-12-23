using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day19
{
    public class Challenge : BaseChallenge
    {
        private string[] _rules;
        private string[] _testStrings;

        public override object part1ExpectedAnswer => 250;
        public override (string message, object answer) SolvePart1()
        {
            string[][] parts = inputFile.Split("\n\n").Select(x => x.Split('\n')).ToArray();

            _rules = new string[parts[0].Length];
            foreach (string ruleData in parts[0])
            {
                Match match = Regex.Match(ruleData, @"(\d+): (.*)");
                int i = int.Parse(match.Groups[1].Value);
                _rules[i] = match.Groups[2].Value;
            }

            _testStrings = parts[1];

            string pattern = $"^{ExpandRule(0)}$";

            return ("Matching strings: ", _testStrings.Count(x => Regex.IsMatch(x, pattern)));
        }
        
        public override object part2ExpectedAnswer => 359;
        public override (string message, object answer) SolvePart2()
        {
            _rules[8] = "42 | 42 8";
            _rules[11] = "42 31 | 42 11 31";

            string pattern = $"^{ExpandRule(0)}$";

            return ("Matching strings: ", _testStrings.Count(x => Regex.IsMatch(x, pattern)));
        }

        private string ExpandRule(int index)
        {
            string rule = _rules[index];

            Match textMatch = Regex.Match(rule, @"""(.*)""");
            if (textMatch.Success)
            {
                return textMatch.Groups[1].Value;
            }

            Match pipeMatch = Regex.Match(rule, @"(.*) \| (.*)");
            if (pipeMatch.Success)
            {
                int[] groupA = ParseGroup(pipeMatch.Groups[1].Value);
                int[] groupB = ParseGroup(pipeMatch.Groups[2].Value);

                // Handle specific recursive cases
                if (groupA.Contains(index) || groupB.Contains(index))
                {
                    // "X = Y | Y X" = "Y+"
                    if (groupA.Length == 1 && groupB.Length == 2 && groupA[0] == groupB[0] && groupB[1] == index)
                    {
                        return $"({ExpandRule(groupA[0])})+";
                    }

                    if (groupA.Length == 2 && groupB.Length == 3 && groupA[0] == groupB[0] && groupA[1] == groupB[2])
                    {
                        string patternA = ExpandRule(groupA[0]);
                        string patternB = ExpandRule(groupA[1]);

                        return $"(?<pair>{patternA})+(?<-pair>{patternB})+(?(pair)z)";
                    }

                    throw new NotSupportedException("Only certain self-referencing rules are allowed");
                }

                return $"({ExpandGroup(groupA)}|{ExpandGroup(groupB)})";
            }

            return ExpandGroup(ParseGroup(rule));

            int[] ParseGroup(string group) => group.Split(' ').Select(int.Parse).ToArray();
            string ExpandGroup(int[] group) => group.Select(ExpandRule).Aggregate((a, b) => a + b);
        }
    }
}
