using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day17 {
     public class Challenge : BaseChallenge {
        private const int TOTAL = 150;

        private int[] _containers;

        public override void InitPart1() {
            _containers = new int[inputSet.Length];
            for (int i = 0; i < inputSet.Length; ++i) {
                _containers[i] = int.Parse(inputSet[i]);
            }
        }

        public override string SolvePart1() {
            return $"Valid combinations: {CountCombinations(write:false)}";
        }
        
        public override string SolvePart2() {
            return $"Valid combinations: {CountCombinations(write:true, minimize:true)}";
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

                    if (usedContainers.Sum() == TOTAL) {
                        ++combos;

                        if (write) {
                            Console.WriteLine(usedContainers.Select(c => $"{c}").Aggregate((a, b) => $"{a} + {b}"));
                        }
                    }
                } while (Advance(indices, _containers.Length));

                if (minimize && combos > 0) break;
            }

            return combos;
        }

        private bool Advance(int[] indices, int count) {
            int n = indices.Length;
            int i = n - 1;
            for (int j = count - 1; i >= 0; --i, --j) {
                if (indices[i] < j) {
                    break;
                }
            }

            if (i < 0) {
                return false;
            }

            for (int j = indices[i] + 1; i < n; ++i, ++j) {
                indices[i] = j;
            }

            return true;
        }
    }
}
