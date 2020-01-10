using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2016.Day01 {
    public class Challenge : BaseChallenge {
        private static readonly Point StartPos = Point.zero;
        private static readonly Direction StartDir = Direction.Up;

        private Step[] _steps;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YUp;
            _steps = input.Split(", ").Select(d => new Step(d)).ToArray();
        }

        public override string part1Answer => "332";
        public override (string, object) SolvePart1() {
            Point pos = StartPos;
            Direction dir = StartDir;

            foreach (Step step in _steps) {
                step.Apply(ref pos, ref dir);
            }

            return ("Blocks to target: ", pos.taxiLength);
        }
        
        public override string part2Answer => "166";
        public override (string, object) SolvePart2() {
            Point pos = StartPos;
            Direction dir = StartDir;

            HashSet<Point> visited = new HashSet<Point>{ pos };

            foreach (Step step in _steps) {
                step.ApplyTurn(ref dir);

                for (int i = 0; i < step.distance; ++i) {
                    pos += dir;

                    if (!visited.Add(pos)) {
                        return ($"First location visited twice: {pos.x}, {pos.y} ({{0}} blocks)", pos.taxiLength);
                    }
                }
            }

            throw new Exception("No location visited twice, failed to find HQ");
        }
    }
}
