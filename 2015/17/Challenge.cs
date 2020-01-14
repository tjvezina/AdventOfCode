using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day17 {
     public class Challenge : BaseChallenge {
        private const int Total = 150;

        private int[] _containers;

        public override void InitPart1() {
            _containers = new int[inputSet.Length];
            for (int i = 0; i < inputSet.Length; ++i) {
                _containers[i] = int.Parse(inputSet[i]);
            }
        }

        public override string part1Answer => "4372";
        public override (string, object) SolvePart1() {
            return ("Valid combinations: ", CountCombinations(write:false));
        }
        
        public override string part2Answer => "4";
        public override (string, object) SolvePart2() {
            return ("Valid combinations: ", CountCombinations(write:true, minimize:true));
        }

        private int CountCombinations(bool write, bool minimize = false) {
            int combos = 0;

            for (int n = 1; n <= _containers.Length; ++n) {
                int[] indices = new int[n];
                for (int i = 0; i < n; ++i) {
                    indices[i] = i;
                }

                do {
                    IEnumerable<int> usedContainers = indices.Select(i => _containers[i]);

                    if (usedContainers.Sum() == Total) {
                        ++combos;

                        if (write) {
                            Console.WriteLine(usedContainers.Select(c => $"{c}").Aggregate((a, b) => $"{a} + {b}"));
                        }
                    }
                } while (DataUtil.NextCombination(indices, _containers.Length));

                if (minimize && combos > 0) break;
            }

            return combos;
        }
    }
}
