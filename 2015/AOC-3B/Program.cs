using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private static void Main(string[] args) {
        string input = File.ReadAllLines("input.txt")[0];

        Point posA = Point.zero;
        Point posB = Point.zero;
        Dictionary<Point, int> presentMap = new Dictionary<Point, int>();
        presentMap.Add(posA, 2); // Both start at first house

        for (int i = 0; i < input.Length; ++i) {
            Point move;
            switch (input[i]) {
                case '>': move = Point.right; break;
                case '<': move = Point.left;  break;
                case '^': move = Point.up;    break;
                case 'v': move = Point.down;  break;
                default:
                    throw new Exception("Unknown direction: " + input[i]);
            }

            Point newPos;
            if (i % 2 == 0) {
                posA += move;
                newPos = posA;
            } else {
                posB += move;
                newPos = posB;
            }

            if (!presentMap.ContainsKey(newPos)) presentMap[newPos] = 0;

            ++presentMap[newPos];
        }

        Console.WriteLine("Total houses visited: " + presentMap.Count);
    }
}
