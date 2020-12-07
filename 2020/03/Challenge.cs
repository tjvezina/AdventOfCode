using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day03
{
    public class Challenge : BaseChallenge
    {
        private static readonly IReadOnlyList<Point> Part2Slopes = Array.AsReadOnly(new []
        {
            new Point(1, 1),
            new Point(3, 1),
            new Point(5, 1),
            new Point(7, 1),
            new Point(1, 2),
        });

        private readonly CharMap _map;

        public Challenge()
        {
            _map = new CharMap(inputList.ToArray());
        }

        public override object part1ExpectedAnswer => 242;
        public override (string message, object answer) SolvePart1()
        {
            return ("The toboggan hits {0} trees", CheckSlope(new Point(3, 1)));
        }

        public override object part2ExpectedAnswer => 2265549792;
        public override (string message, object answer) SolvePart2()
        {
            long product = Part2Slopes.Select(x => (long)CheckSlope(x)).Aggregate((a, b) => a * b);

            return ("Product of trees hit on each slope is ", product);
        }

        private int CheckSlope(in Point slope)
        {
            int treeCount = 0;

            int x = 0;
            for (int y = slope.y; y < _map.height; y += slope.y)
            {
                x = MathUtil.Wrap(x + slope.x, 0, _map.width);
                if (_map[x, y] == '#')
                {
                    treeCount++;
                }
            }

            return treeCount;
        }
    }
}
