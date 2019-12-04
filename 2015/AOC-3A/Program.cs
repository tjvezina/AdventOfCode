using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private static void Main(string[] args) {
        string input = File.ReadAllLines("input.txt")[0];

        Point pos = Point.zero;
        Dictionary<Point, int> presentMap = new Dictionary<Point, int>();
        presentMap.Add(pos, 1);

        foreach (char dir in input) {
            switch (dir) {
                case '>': pos += Point.right; break;
                case '<': pos += Point.left; break;
                case '^': pos += Point.up; break;
                case 'v': pos += Point.down; break;
                default:
                    throw new Exception("Unknown direction: " + dir);
            }

            if (!presentMap.ContainsKey(pos)) presentMap[pos] = 0;

            ++presentMap[pos];
        }

        Console.WriteLine("Total houses visited: " + presentMap.Count);
    }
}
