using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private class AxisData {
        public int initial;
        public int pos;
        public int vel;

        public bool HasLooped => vel == 0 && pos == initial;

        public AxisData(int initial) {
            this.initial = initial;
            pos = initial;
            vel = 0;
        }

        public void UpdateVelocity(IEnumerable<AxisData> axisData) {
            vel += axisData.Select(d => d.pos - pos).Sum(d => d == 0 ? 0 : d / Math.Abs(d));
        }

        public void UpdatePosition() => pos += vel;
    }

    private static List<Point3> _bodies = new List<Point3>();
    private static List<AxisData> _axisData;

    private static void Main(string[] args) {
        LoadData();

        long[] cycleLengths = new long[3];

        // Each axis is independent; brute-force simulate to determine how long each takes to repeat
        for (int axis = 0; axis < 3; ++axis) {
            cycleLengths[axis] = CalculateCycleLength(axis);
        }

        // Find the LCM of all 3 axes; the total number of steps before all bodies are back where they started
        long totalSteps = LCM(cycleLengths[0], LCM(cycleLengths[1], cycleLengths[2]));

        Console.WriteLine("System cycle length: " + totalSteps);
    }

    private static void LoadData() {
        string[] input = File.ReadAllLines("input.txt");

        foreach (string data in input) {
            string[] parts = data.Split(',');
            _bodies.Add(new Point3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])));
        }
    }

    private static long CalculateCycleLength(int axis) {
        _axisData = _bodies.Select(b => new AxisData(b[axis])).ToList();

        bool HasLooped() => _axisData.All(d => d.vel == 0 && d.pos == d.initial);

        long cycle = 0;
        do {
            _axisData.ForEach(d => d.UpdateVelocity(_axisData));
            _axisData.ForEach(d => d.UpdatePosition());
            ++cycle;
        } while (!HasLooped());

        return cycle;
    }

    private static long LCM(long a, long b) => (a * b) / GCD(a, b);

    private static long GCD(long a, long b) {
        a = Math.Abs(a);
        b = Math.Abs(b);

        if (a < b) {
            long hold = a;
            a = b;
            b = hold;
        }

        while (b > 0) {
            long r = a % b;
            a = b;
            b = r;
        }

        return a;
    }

}
