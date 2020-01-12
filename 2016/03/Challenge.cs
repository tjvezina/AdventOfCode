using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day03 {
    public class Challenge : BaseChallenge {
        private struct Triangle {
            public readonly int[] sides;

            public bool isValid => sides[0] + sides[1] > sides[2];

            public Triangle(IEnumerable<int> sides) => this.sides = sides.OrderBy(s => s).ToArray();
        }

        private int[][] _sides;

        public override void InitPart1() {
            _sides = new int[inputSet.Length][];

            for (int y = 0; y < inputSet.Length; y++) {
                string line = inputSet[y];
                _sides[y] = Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray();
            }
        }

        public override string part1Answer => "1050";
        public override (string, object) SolvePart1() {
            int validCount = _sides.Select(s => new Triangle(s)).Count(t => t.isValid);
            return ("{0} valid triangles", validCount);
        }
        
        public override string part2Answer => "1921";
        public override (string, object) SolvePart2() {
            List<Triangle> triangles = new List<Triangle>();

            for (int y = 0; y < _sides.Length; y += 3) {
                for (int x = 0; x < _sides[y].Length; ++x) {
                    triangles.Add(new Triangle(Enumerable.Range(0, 3).Select(y2 => _sides[y + y2][x])));
                }
            }

            return ("{0} valid triangles", triangles.Count(t => t.isValid));
        }
    }
}
