using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private struct Box {
        public int l;
        public int w;
        public int h;

        public int surfaceArea => 2*l*w + 2*w*h + 2*h*l;
        public int smallestSideArea {
            get {
                List<int> sides = new List<int> { l, w, h };
                sides.Sort();
                return sides[0] * sides[1];
            }
        }

        public Box(string data) {
            int[] dimensions = data.Split('x').Select(int.Parse).ToArray();
            l = dimensions[0];
            w = dimensions[1];
            h = dimensions[2];
        }
    }

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        IEnumerable<Box> boxes = input.Select(data => new Box(data));

        Console.WriteLine(boxes.Sum(b => b.surfaceArea + b.smallestSideArea));
    }
}
