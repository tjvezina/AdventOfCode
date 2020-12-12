using System;

namespace AdventOfCode.Year2020.Day12
{
    [CoordSystem(CoordSystem.YUp)]
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 1645;
        public override (string message, object answer) SolvePart1()
        {
            Direction direction = Direction.Right;
            Point position = Point.zero;

            foreach (string input in inputList)
            {
                char action = input[0];
                int value = int.Parse(input.Substring(1));

                switch (action)
                {
                    case 'N': position.y += value; break;
                    case 'S': position.y -= value; break;
                    case 'E': position.x += value; break;
                    case 'W': position.x -= value; break;
                    case 'F': position += (Point)direction * value; break;
                    case 'L':
                        for (int r = 0; r < value; r += 90)
                        {
                            direction = direction.RotateCCW();
                        }
                        break;
                    case 'R':
                        for (int r = 0; r < value; r += 90)
                        {
                            direction = direction.RotateCW();
                        }
                        break;
                    default:
                        throw new Exception($"Unrecognized action: {action}");
                }
            }

            return ($"The ship's final position ({position.x}, {position.y}) = ", position.taxiLength);
        }
        
        public override object part2ExpectedAnswer => 35292;
        public override (string message, object answer) SolvePart2()
        {
            Point position = Point.zero;
            Point waypoint = new Point(10, 1);

            foreach (string input in inputList)
            {
                char action = input[0];
                int value = int.Parse(input.Substring(1));

                switch (action)
                {
                    case 'N': waypoint.y += value; break;
                    case 'S': waypoint.y -= value; break;
                    case 'E': waypoint.x += value; break;
                    case 'W': waypoint.x -= value; break;
                    case 'L':
                        for (int r = 0; r < value; r += 90)
                        {
                            waypoint = waypoint.RotateCCW();
                        }
                        break;
                    case 'R':
                        for (int r = 0; r < value; r += 90)
                        {
                            waypoint = waypoint.RotateCW();
                        }
                        break;
                    case 'F':
                        for (int i = 0; i < value; i++)
                        {
                            position += waypoint;
                        }
                        break;
                }
            }

            return ($"The ship's final position ({position.x}, {position.y}) = ", position.taxiLength);
        }
    }
}
