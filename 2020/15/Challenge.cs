using System.Linq;

namespace AdventOfCode.Year2020.Day15
{
    public class Challenge : BaseChallenge
    {
        private const string Input = "0,13,1,16,6,17";

        public override object part1ExpectedAnswer => 234;
        public override (string message, object answer) SolvePart1()
        {
            return ("On turn 2020, the number is ", PlayUntilTurn(2020));
        }
        
        public override object part2ExpectedAnswer => 8984;
        public override (string message, object answer) SolvePart2()
        {
            return ("On turn 30 million, the number is ", PlayUntilTurn(30_000_000));
        }

        private int PlayUntilTurn(int lastTurn)
        {
            int[] starting = Input.Split(',').Select(int.Parse).ToArray();

            // A dictionary would be more intuitive, but is way slower when setting values
            int[] lastSeen = new int[lastTurn]; // Values do not exceed turn count

            for (int i = 0; i < starting.Length - 1; i++)
            {
                lastSeen[starting[i]] = i + 1;
            }

            int turn = starting.Length + 1;
            int lastNumber = starting.Last();

            while (turn <= lastTurn)
            {
                int nextNumber = lastSeen[lastNumber] != 0 ? turn - 1 - lastSeen[lastNumber] : 0;

                lastSeen[lastNumber] = turn - 1;

                lastNumber = nextNumber;
                turn++;
            }

            Profiler.FlushResults();

            return lastNumber;
        }
    }
}
