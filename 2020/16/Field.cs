using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace AdventOfCode.Year2020.Day16
{
    [PublicAPI]
    public class Field
    {
        public static Field Parse(string input)
        {
            Match match = Regex.Match(input, @"([a-z ]+): (\d+)-(\d+) or (\d+)-(\d+)");

            return new Field(
                match.Groups[1].Value,
                new Range(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value)),
                new Range(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value))
            );
        }

        public string name { get; }
        public Range lowRange { get; }
        public Range highRange { get; }

        public Field(string name, Range lowRange, Range highRange)
        {
            this.name = name;
            this.lowRange = lowRange;
            this.highRange = highRange;
        }

        public bool IsValid(int value) => lowRange.Contains(value) || highRange.Contains(value);
    }
}
