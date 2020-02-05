using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day15 {
    public class Challenge : BaseChallenge {
        private struct Disc {
            public static Disc Parse(string data) {
                Match match = Regex.Match(data, @"has (\d+) positions; at time=0, it is at position (\d+)");
                int steps = int.Parse(match.Groups[1].Value);
                int positions = int.Parse(match.Groups[2].Value);
                return new Disc { steps = steps, position = positions };
            }

            public int steps;
            public int position;
        }

        private List<Disc> _discs;

        public Challenge() => _discs = inputList.Select(Disc.Parse).ToList();

        public override string part1ExpectedAnswer => "122318";
        public override (string message, object answer) SolvePart1() {
            return ("First chance to drop capsule: ", FindFirstAlignTime());
        }
        
        public override string part2ExpectedAnswer => "3208583";
        public override (string message, object answer) SolvePart2() {
            _discs.Add(new Disc { steps = 11, position = 0 });
            return ("First chance to drop capsule: ", FindFirstAlignTime());
        }

        private int FindFirstAlignTime() {
            Disc firstDisc = _discs[0];
            int startTime = firstDisc.steps - firstDisc.position - 1;
            for (int t = startTime; t < int.MaxValue; t += firstDisc.steps) {
                bool success = true;
                for (int i = 0; i < _discs.Count; i++) {
                    if ((_discs[i].position + t + i + 1) % _discs[i].steps != 0) {
                        success = false;
                        break;
                    }
                }

                if (success) {
                    return t;
                }
            }

            throw new Exception("Failed to find a time at which all discs align");
        }
    }
}
