using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2015.Day03
{
     public class Challenge : BaseChallenge
     {
        public override CoordSystem? coordSystem => CoordSystem.YUp;

        private readonly string _input;

        public Challenge() => _input = inputList[0];

        public override string part1ExpectedAnswer => "2081";
        public override (string message, object answer) SolvePart1()
        {
            Point pos = Point.zero;
            Dictionary<Point, int> presentMap = new Dictionary<Point, int>();
            presentMap.Add(pos, 1);

            foreach (char dir in _input)
            {
                switch (dir)
                {
                    case '>': pos += Direction.Right; break;
                    case '<': pos += Direction.Left;  break;
                    case '^': pos += Direction.Up;    break;
                    case 'v': pos += Direction.Down;  break;
                    default:
                        throw new Exception("Unknown direction: " + dir);
                }

                if (!presentMap.ContainsKey(pos)) presentMap[pos] = 0;

                presentMap[pos]++;
            }

            return ("Total houses visited: ", presentMap.Count);
        }
        
        public override string part2ExpectedAnswer => "2341";
        public override (string message, object answer) SolvePart2()
        {
            Point posA = Point.zero;
            Point posB = Point.zero;
            Dictionary<Point, int> presentMap = new Dictionary<Point, int>();
            presentMap.Add(posA, 2); // Both start at first house

            for (int i = 0; i < _input.Length; i++)
            {
                Point move;
                switch (_input[i])
                {
                    case '>': move = Direction.Right; break;
                    case '<': move = Direction.Left;  break;
                    case '^': move = Direction.Up;    break;
                    case 'v': move = Direction.Down;  break;
                    default:
                        throw new Exception("Unknown direction: " + _input[i]);
                }

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
