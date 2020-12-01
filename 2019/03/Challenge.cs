using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day03
{
     public class Challenge : BaseChallenge
     {
        private struct Step
        {
            public Point start;
            public Direction direction;
            public int distance;
            public int length;

            public override string ToString() => $"{start}{length}+{direction}{distance}";
        }

        private class Wire
        {
            public List<Step> horizontalSteps = new List<Step>();
            public List<Step> verticalSteps = new List<Step>();
        }

        public override CoordSystem? coordSystem => CoordSystem.YUp;

        private readonly Wire _wireA;
        private readonly Wire _wireB;

        public Challenge()
        {
            _wireA = ParseWireInput(inputList[0]);
            _wireB = ParseWireInput(inputList[1]);
        }

        public override string part1ExpectedAnswer => "1519";
        public override (string message, object answer) SolvePart1()
        {
            return ("Nearest intersection is {0} units from origin.", FindClosestIntersection());
        }
        
        public override string part2ExpectedAnswer => "14358";
        public override (string message, object answer) SolvePart2()
        {
            return ("Shortest intersection is {0} units along wires.", FindShortestIntersection());
        }

        private Wire ParseWireInput(string input)
        {
            Wire wire = new Wire();
            Step step = new Step { start = Point.zero };

            foreach (string stepData in input.Split(','))
            {
                char dirChar = stepData[0];
                step.distance = int.Parse(stepData.Substring(1));

                switch (dirChar)
                {
                    case 'R':   step.direction = Direction.Right;   break;
                    case 'L':   step.direction = Direction.Left;    break;
                    case 'U':   step.direction = Direction.Up;      break;
                    case 'D':   step.direction = Direction.Down;    break;
                    default:
                        throw new Exception("Unrecognized step direction: " + dirChar);
                }

                if (step.direction.IsHorizontal())
                {
                    wire.horizontalSteps.Add(step);
                } else
                {
                    wire.verticalSteps.Add(step);
                }

                step.start += (Point)step.direction * step.distance;
                step.length += step.distance;
            }

            return wire;
        }

        private int TaxiDist(Point p) => Math.Abs(p.x) + Math.Abs(p.y);

        private int FindClosestIntersection()
        {
            int closest = int.MaxValue;

            foreach (Step hStepA in _wireA.horizontalSteps)
            {
                foreach (Step vStepB in _wireB.verticalSteps)
                {
                    if (TryGetIntersection(hStepA, vStepB, out Point intersection, out int distance))
                    {
                        closest = Math.Min(closest, TaxiDist(intersection));
                    }
                }
            }

            foreach (Step vStepA in _wireA.verticalSteps)
            {
                foreach (Step hStepB in _wireB.horizontalSteps)
                {
                    if (TryGetIntersection(vStepA, hStepB, out Point intersection, out int distance))
                    {
                        closest = Math.Min(closest, TaxiDist(intersection));
                    }
                }
            }

            return closest;
        }

        private int FindShortestIntersection()
        {
            int shortest = int.MaxValue;

            foreach (Step hStepA in _wireA.horizontalSteps)
            {
                foreach (Step vStepB in _wireB.verticalSteps)
                {
                    if (TryGetIntersection(hStepA, vStepB, out Point p, out int distance))
                    {
                        shortest = Math.Min(shortest, distance);
                    }
                }
            }

            foreach (Step vStepA in _wireA.verticalSteps)
            {
                foreach (Step hStepB in _wireB.horizontalSteps)
                {
                    if (TryGetIntersection(vStepA, hStepB, out Point p, out int distance))
                    {
                        shortest = Math.Min(shortest, distance);
                    }
                }
            }

            return shortest;
        }

        // Assumes the given steps are perpendicular
        private bool TryGetIntersection(Step stepA, Step stepB, out Point intersection, out int distance)
        {
            int axisA = (stepA.direction.IsHorizontal() ? 0 : 1);
            int axisB = (axisA == 0 ? 1 : 0);

            int startA = stepA.start[axisA];
            int startB = stepB.start[axisB];

            int signA = stepA.direction.Sign();
            int signB = stepB.direction.Sign();

            Range rangeA = new Range(startA + signA, startA + stepA.distance * signA);
            Range rangeB = new Range(startB + signB, startB + stepB.distance * signB);

            int rowA = stepA.start[axisB];
            int rowB = stepB.start[axisA];

            if (rangeA.Contains(rowB) && rangeB.Contains(rowA))
            {
                intersection = new Point();
                intersection[axisB] = rowA;
                intersection[axisA] = rowB;

                distance = stepA.length + stepB.length;
                distance += Math.Abs(rowB - startA);
                distance += Math.Abs(rowA - startB);
                return true;
            }
            
            intersection = default;
            distance = default;
            return false;
        }
    }
}
