using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day19
{
    public class Challenge : BaseChallenge
    {
        private readonly string _molecule;
        private readonly List<KeyValuePair<string, string>> _replacements = new List<KeyValuePair<string, string>>();

        public Challenge()
        {
            Queue<string> input = new Queue<string>(inputList);

            string data;
            while (!string.IsNullOrEmpty(data = input.Dequeue()))
            {
                Match match = Regex.Match(data, @"(\w+) => (\w+)");
                _replacements.Add(new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value));
            }

            _molecule = input.Dequeue();
        }

        public override string part1ExpectedAnswer => "576";
        public override (string message, object answer) SolvePart1()
        {
            HashSet<string> subMolecules = new HashSet<string>();

            foreach ((string pattern, string replace) in _replacements)
            {
                int i = 0;
                while ((i = _molecule.IndexOf(pattern, i)) != -1)
                {
                    subMolecules.Add(_molecule.Remove(i, pattern.Length).Insert(i, replace));
                    i++;
                }
            }

            return ("Distinct submolecules: ", subMolecules.Count);
        }

        public override string part2ExpectedAnswer => "207";
        public override (string message, object answer) SolvePart2()
        {
            int Count(string match)
            {
                int count = 0;
                for (int i = 0; (i = _molecule.IndexOf(match, i)) != -1; i++, count++);
                return count;
            }

            int steps = _molecule.Count(char.IsUpper) - Count("Rn") - Count("Ar") - 2 * Count("Y") - 1;

            return ("Steps to molecule: ", steps);
        }
    }
}
