using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        List<Body> myHierarchy = new List<Body>();
        List<Body> santaHierarchy = new List<Body>();

        Body body = _bodyMap["YOU"];
        while (body.parent != null) {
            myHierarchy.Add(body.parent);
            body = body.parent;
        }
        body = _bodyMap["SAN"];
        while (body.parent != null) {
            santaHierarchy.Add(body.parent);
            body = body.parent;
        }

        Body commonParent = null;
        foreach (Body parent in myHierarchy) {
            if (santaHierarchy.Contains(parent)) {
                commonParent = parent;
                break;
            }
        }

        Debug.Assert(commonParent != null, "Failed to find common parent body in hierarchies, santa is unreachable!");

        int transferCount = myHierarchy.IndexOf(commonParent) + santaHierarchy.IndexOf(commonParent);
        Console.WriteLine("Orbital transfers to santa: " + transferCount);
    }
}
