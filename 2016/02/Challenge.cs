using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2016.Day02 {
    public class Challenge : BaseChallenge {
        private static readonly Point StartPos = new Point(2, 2);

        private Direction[][] _instructions;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YDown;

            Direction ToDirection(char c) {
                return c switch {
                    'L' => Direction.Left,
                    'R' => Direction.Right,
                    'U' => Direction.Up,
                    'D' => Direction.Down,
                    _   => throw new Exception($"Unrecognized direction '{c}'")
                };
            }

            _instructions = inputArray.Select(i => i.Select(ToDirection).ToArray()).ToArray();
        }

        public override string part1Answer => "56983";
        public override (string, object) SolvePart1() {
            string code = GetCode(Point.one, new CharMap(new[] {
                "123",
                "456",
                "789"
            }));

            return ("Bathroom code: ", code);
        }
        
        public override string part2Answer => "8B8B1";
        public override (string, object) SolvePart2() {
            string code = GetCode(new Point(0, 2), new CharMap(new[] {
                "  1  ",
                " 234 ",
                "56789",
                " ABC ",
                "  D  "
            }));

            return ("Bathroom code: ", code);
        }

        private string GetCode(Point start, CharMap keypad) {
            string code = string.Empty;
            Point pos = start;

            foreach (Direction[] steps in _instructions) {
                foreach (Direction dir in steps) {
                    Point nextPos = pos + dir;
                    
                    bool outOfBounds = nextPos.x < 0 || nextPos.x >= keypad.width ||
                                       nextPos.y < 0 || nextPos.y >= keypad.height;
                    
                    if (!outOfBounds && keypad[nextPos] != ' ') {
                        pos = nextPos;
                    }
                }

                code += keypad[pos];
            }

            return code;
        }
    }
}
