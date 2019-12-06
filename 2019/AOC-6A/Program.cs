using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private class Body {
        public string name;
        public Body parent;
    }

    private const char SEPARATOR = ')';

    private static Dictionary<string, Body> _bodyMap = new Dictionary<string, Body>();

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        foreach (string data in input) {
            string[] bodies = data.Split(SEPARATOR);
            string bodyA = bodies[0];
            string bodyB = bodies[1];

            if (!_bodyMap.ContainsKey(bodyA)) {
                _bodyMap[bodyA] = new Body { name = bodyA };
            }
            if (!_bodyMap.ContainsKey(bodyB)) {
                _bodyMap[bodyB] = new Body { name = bodyB };
            }

            _bodyMap[bodyB].parent = _bodyMap[bodyA];
        }

        int orbitTotal = 0;

        foreach (Body body in _bodyMap.Values) {
            Body nextBody = body;
            while (nextBody.parent != null) {
                ++orbitTotal;
                nextBody = nextBody.parent;
            }
        }

        Console.WriteLine("Total direct + indirect orbits: " + orbitTotal);
    }
}
