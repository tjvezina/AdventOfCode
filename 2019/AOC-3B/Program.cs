using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private struct Step {
        public Point start;
        public Direction direction;
        public int distance;
        public int length;

        public override string ToString() => $"{start}{length}+{direction}{distance}";
    }

    private class Wire {
        public List<Step> horizontalSteps = new List<Step>();
        public List<Step> verticalSteps = new List<Step>();
    }

    private static Wire _wireA;
    private static Wire _wireB;

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        _wireA = ParseWireInput(input[0]);
        _wireB = ParseWireInput(input[1]);

        Console.WriteLine(FindShortestIntersection());
    }

    private static Wire ParseWireInput(string input) {
        Wire wire = new Wire();
        Step step = new Step { start = Point.zero };

        foreach (string stepData in input.Split(',')) {
            char dirChar = stepData[0];
            step.distance = int.Parse(stepData.Substring(1));

            switch (dirChar) {
                case 'R':   step.direction = Direction.Right;   break;
                case 'L':   step.direction = Direction.Left;    break;
                case 'U':   step.direction = Direction.Up;      break;
                case 'D':   step.direction = Direction.Down;    break;
                default:
                    throw new Exception("Unrecognized step direction: " + dirChar);
            }

            if (step.direction.IsHorizontal()) {
                wire.horizontalSteps.Add(step);
            } else {
                wire.verticalSteps.Add(step);
            }

            step.start += step.direction.ToPoint() * step.distance;
            step.length += step.distance;
        }

        return wire;
    }

    // Assumes wires only cross (not overlap), and not at a corner
    private static int FindShortestIntersection() {
        int shortest = int.MaxValue;

        foreach (Step hStepA in _wireA.horizontalSteps) {
            foreach (Step vStepB in _wireB.verticalSteps) {
                if (TryGetIntersection(hStepA, vStepB, out int distance)) {
                    shortest = Math.Min(shortest, distance);
                }
            }
        }

        foreach (Step vStepA in _wireA.verticalSteps) {
            foreach (Step hStepB in _wireB.horizontalSteps) {
                if (TryGetIntersection(vStepA, hStepB, out int distance)) {
                    shortest = Math.Min(shortest, distance);
                }
            }
        }

        return shortest;
    }

    // Assumes the given steps are perpendicular
    private static bool TryGetIntersection(Step stepA, Step stepB, out int distance) {
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

        if (rangeA.Contains(rowB) && rangeB.Contains(rowA)) {
            distance = stepA.length + stepB.length;
            distance += Math.Abs(rowB - startA);
            distance += Math.Abs(rowA - startB);
            return true;
        }
        
        distance = default;
        return false;
    }
}
