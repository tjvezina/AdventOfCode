using System;
using System.Collections.Generic;
using System.IO;

class Program {
    private struct Point {
        public static readonly Point zero = new Point(0, 0);

        public int x;
        public int y;

        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    private static List<Point> _wireA;
    private static List<Point> _wireB;

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        ParseWireInput(input[0], _wireA);
        ParseWireInput(input[1], _wireB);
    }

    private static void ParseWireInput(string input, List<Point> wire) {
        Point p = Point.zero;
        wire.Add(p);

        foreach (string step in input.Split(',')) {
            char dir = step[0];
            int dist = int.Parse(step.Substring(1));

            switch (dir) {
                case 'R':   p.x += dist;    break;
                case 'L':   p.x -= dist;    break;
                case 'U':   p.y += dist;    break;
                case 'D':   p.y -= dist;    break;
                default:
                    throw new Exception("Unrecognized step direction: " + dir);
            }

            wire.Add(p);
        }
    }

    private static int FindClosestIntersection() {
        
    }
}
