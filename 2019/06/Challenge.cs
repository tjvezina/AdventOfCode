using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019.Day06 {
     public class Challenge : BaseChallenge {
        private class Body {
            public string name;
            public Body parent;
        }

        private const char SEPARATOR = ')';

        private Dictionary<string, Body> _bodyMap = new Dictionary<string, Body>();

        public override void InitPart1() {
            foreach (string data in inputSet) {
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

        public override string part1Answer => "130681";
        public override (string, object) SolvePart1() {
            int orbitTotal = 0;

            foreach (Body body in _bodyMap.Values) {
                Body nextBody = body;
                while (nextBody.parent != null) {
                    ++orbitTotal;
                    nextBody = nextBody.parent;
                }
            }

            return ("Total direct + indirect orbits: ", orbitTotal);
        }
        
        public override string part2Answer => "313";
        public override (string, object) SolvePart2() {
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
            return ("Orbital transfers to santa: ", transferCount);
        }
    }
}
