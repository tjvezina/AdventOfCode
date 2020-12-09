using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day01
{
    [CoordSystem(CoordSystem.YUp)]
    public class Challenge : BaseChallenge
    {
        private static readonly Point StartPos = Point.zero;
        private const Direction StartDir = Direction.Up;

        private readonly Step[] _steps;

        public Challenge() => _steps = inputList[0].Split(", ").Select(d => new Step(d)).ToArray();

        public override object part1ExpectedAnswer => 332;
        public override (string message, object answer) SolvePart1()
        {
            Point pos = StartPos;
            Direction dir = StartDir;

            foreach (Step step in _steps)
            {
                step.Apply(ref pos, ref dir);
            }

            return ("Blocks to target: ", pos.taxiLength);
        }
        
        public override object part2ExpectedAnswer => 166;
        public override (string message, object answer) SolvePart2()
        {
            Point pos = StartPos;
            Direction dir = StartDir;

            HashSet<Point> visited = new HashSet<Point>{ pos };

            foreach (Step step in _steps)
            {
                step.ApplyTurn(ref dir);

                for (int i = 0; i < step.distance; i++)
                {
                    pos += dir;

                    if (!visited.Add(pos))
                    {
                        return ($"First location visited twice: {pos.x}, {pos.y} ({{0}} blocks)", pos.taxiLength);
                    }
                }
            }

            throw new Exception("No location visited twice, failed to find HQ");
        }
    }
}
