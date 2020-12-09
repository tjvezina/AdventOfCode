using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2015.Day03
{
    [CoordSystem(CoordSystem.YUp)]
    public class Challenge : BaseChallenge
    {
        private readonly string _input;

        public Challenge() => _input = inputList[0];

        public override object part1ExpectedAnswer => 2081;
        public override (string message, object answer) SolvePart1()
        {
            Point pos = Point.zero;
            Dictionary<Point, int> presentMap = new Dictionary<Point, int> { [pos] = 1 };

            foreach (char dir in _input)
            {
                pos += dir switch
                {
                    '>' => Direction.Right,
                    '<' => Direction.Left,
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    _ => throw new Exception("Unknown direction: " + dir)
                };

                if (!presentMap.ContainsKey(pos)) presentMap[pos] = 0;

                presentMap[pos]++;
            }

            return ("Total houses visited: ", presentMap.Count);
        }
        
        public override object part2ExpectedAnswer => 2341;
        public override (string message, object answer) SolvePart2()
        {
            Point posA = Point.zero;
            Point posB = Point.zero;
            Dictionary<Point, int> presentMap = new Dictionary<Point, int> { [posA] = 2 }; // Both start at first house

            for (int i = 0; i < _input.Length; i++)
            {
                Point move = _input[i] switch
                {
                    '>' => Direction.Right,
                    '<' => Direction.Left,
                    '^' => Direction.Up,
                    'v' => Direction.Down,
                    _ => throw new Exception("Unknown direction: " + _input[i])
                };

                Point newPos;
                if (i % 2 == 0)
                {
                    posA += move;
                    newPos = posA;
                } else
                {
                    posB += move;
                    newPos = posB;
                }

                if (!presentMap.ContainsKey(newPos)) presentMap[newPos] = 0;

                presentMap[newPos]++;
            }

            return ("Total houses visited: ", presentMap.Count);
        }
    }
}
