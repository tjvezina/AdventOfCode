using System.Collections.Generic;

namespace AdventOfCode.Year2016.Day13
{
    [CoordSystem(CoordSystem.YDown)]
    public class Challenge : BaseChallenge
    {
        private const int Input = 1364;
        private static readonly Point Start = new Point(1, 1);
        private static readonly Point End = new Point(31, 39);

        public override object part1ExpectedAnswer => 86;
        public override (string message, object answer) SolvePart1()
        {
            Stack<Point> path = Pathfinder.FindPathInGrid(Start, End, isValid:IsOpen);

            return ("Shortest path: {0} steps", path.Count);
        }
        
        public override object part2ExpectedAnswer => 127;
        public override (string message, object answer) SolvePart2()
        {
            return ("Reachable spaces: ", CountReachable(50));
        }

        private bool IsOpen(Point point)
        {
            (int x, int y) = point;
            return x >= 0 && y >= 0 && HasEvenBits(x*x + 3*x + 2*x*y + y + y*y + Input);
        }

        private bool HasEvenBits(int value)
        {
            for (int x = sizeof(int) * 8 / 2; x >= 1; x /= 2)
            {
                value ^= value >> x;
            }
            return (~value & 1) == 1;
        }

        private int CountReachable(int distance)
        {
            HashSet<Point> visited = new HashSet<Point>{ Start };
            Queue<(Point, int)> queue = new Queue<(Point, int)>(new[] { (Start, 0) });

            while (queue.Count > 0)
            {
                (Point next, int steps) = queue.Dequeue();
                steps++;

                foreach (Direction dir in EnumUtil.GetValues<Direction>())
                {
                    Point neighbor = next + dir;

                    if (IsOpen(neighbor) && visited.Add(neighbor) && steps < distance)
                    {
                        queue.Enqueue((neighbor, steps));
                    }
                }
            }

            return visited.Count;
        }
    }
}
