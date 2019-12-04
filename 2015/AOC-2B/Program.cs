using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private struct Box {
        private List<int> _sides;

        public int l => _sides[0];
        public int w => _sides[1];
        public int h => _sides[2];

        public int volume => l * w * h;
        public int surfaceArea => (2 * l * w) + (2 * w * h) + (2 * h * l);
        public int smallestSideArea => _sides[0] * _sides[1];
        public int smallestSidePerim => (2 * _sides[0]) + (2 * _sides[1]);

        public Box(string data) {
            _sides = data.Split('x').Select(int.Parse).ToList();
            _sides.Sort();
        }
    }

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        IEnumerable<Box> boxes = input.Select(data => new Box(data));

        Console.WriteLine(boxes.Sum(b => b.smallestSidePerim + b.volume));
    }
}
