using System.Linq;

namespace AdventOfCode.Year2020.Day06
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 6387;
        public override (string message, object answer) SolvePart1()
        {
            int sum = inputFile.Split("\n\n").Sum(x => x.Replace("\n", "").Distinct().Count());

            return ("Sum of all positive answers in each group: ", sum);
        }
        
        public override object part2ExpectedAnswer => 3039;
        public override (string message, object answer) SolvePart2()
        {
            int sum = 0;

            foreach (string group in inputFile.Split("\n\n"))
            {
                int groupSize = group.Split('\n').Length;

                sum += group.Replace("\n", "").Distinct().Count(c => group.Count(x => x == c) == groupSize);
            }

            return ("Sum of common positive answers in each group: ", sum);
        }
    }
}
