using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day06 : Challenge {
        private class Body {
            public string name;
            public Body parent;
        }

        private const char SEPARATOR = ')';

        private Dictionary<string, Body> _bodyMap = new Dictionary<string, Body>();

        private void Init(string[] input) {
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
        }

        protected override string SolvePart1() {
            int orbitTotal = 0;

            foreach (Body body in _bodyMap.Values) {
                Body nextBody = body;
                while (nextBody.parent != null) {
                    ++orbitTotal;
                    nextBody = nextBody.parent;
                }
            }

            return $"Total direct + indirect orbits: {orbitTotal}";
        }
        
        protected override string SolvePart2() {
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
            return $"Orbital transfers to santa: {transferCount}";
        }
    }
}
