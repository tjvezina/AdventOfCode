using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private struct Body {
        public Point3 position;
        public Point3 velocity;

        public int potentialEnergy => Math.Abs(position.x) + Math.Abs(position.y) + Math.Abs(position.z);
        public int kineticEnergy => Math.Abs(velocity.x) + Math.Abs(velocity.y) + Math.Abs(velocity.z);
        public int totalEnergy => potentialEnergy * kineticEnergy;
    }

    private static List<Body> _bodies = new List<Body>();

    private static void Main(string[] args) {
        LoadData();

        for (int i = 0; i < 1000; ++i) {
            UpdateVelocities();
            UpdatePositions();
        }

        int totalEnergy = _bodies.Sum(b => b.totalEnergy);
        Console.WriteLine($"Total energy: {totalEnergy}");
    }

    private static void LoadData() {
        string[] input = File.ReadAllLines("input.txt");

        foreach (string data in input) {
            string[] parts = data.Split(',');
            Point3 pos = new Point3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
            _bodies.Add(new Body { position = pos });
        }
    }

    private static void UpdateVelocities() {
        for (int i = 0; i < _bodies.Count; ++i) {
            Body body = _bodies[i];
            Point3 newVelocity = Point3.zero;
            for (int a = 0; a < 3; ++a) {
                newVelocity[a] = _bodies.Select(b => b.position[a] - body.position[a])
                                        .Sum(d => d == 0 ? 0 : d / Math.Abs(d));
            }
            body.velocity += newVelocity;
            _bodies[i] = body;
        }
    }

    private static void UpdatePositions() {
        for (int i = 0; i < _bodies.Count; ++i) {
            Body body = _bodies[i];
            body.position += body.velocity;
            _bodies[i] = body;
        }
    }
}
