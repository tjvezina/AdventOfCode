using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019 {
    public class Day19 : Challenge {
        private enum Result {
            Stationary = 0,
            Pulled = 1
        }

        private IntCode _intCode;
        private int _pulledCount = 0;

        private void Init(string input) {
            SpaceUtil.system = CoordSystem.YDown;
            _intCode = new IntCode(input);
        }

        protected override string SolvePart1() {
            for (int y = 0; y < 50; ++y) {
                for (int x = 0; x < 50; ++x) {
                    char c = '.';
                    if (IsPointPulled(x, y)) {
                        ++_pulledCount;
                        c = '#';
                    }
                    Console.Write(c);
                }
                Console.WriteLine(string.Empty);
            }

            return $"Drone was pulled from {_pulledCount} locations";
        }

        protected override string SolvePart2() {
            Point p = new Point(100, 100);
            List<Direction> moves = new List<Direction>();

            while (true) {
                moves.Add((IsPointPulled(p) ? Direction.Right : Direction.Down));
                if (moves.Count > 2) {
                    moves.RemoveAt(0);
                    if (moves[0] == Direction.Down && moves[1] == Direction.Right) {
                        Point topLeft = p + new Point(-99, 0);
                        if (IsPointPulled(topLeft)) {
                            Point botLeft = topLeft + new Point(0, 99);
                            if (IsPointPulled(botLeft)) {
                                return $"Result: {topLeft.x * 10_000 + topLeft.y}";
                            }
                        }
                    }
                }

                p += moves.Last();
            }
        }

        private bool IsPointPulled(Point p) => IsPointPulled(p.x, p.y);
        private bool IsPointPulled(int x, int y) {
            bool isPulled = false;
            void HandleOutput(long o) => isPulled = (o == 1);
            _intCode.OnOutput += HandleOutput;
            _intCode.Reset();
            _intCode.Begin();
            _intCode.Input(x);
            _intCode.Input(y);
            _intCode.OnOutput -= HandleOutput;
            return isPulled;
        }
    }
}
