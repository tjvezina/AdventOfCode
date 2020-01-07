using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day24 {
    public class Challenge : BaseChallenge {
        private long[] _packages;
        private long _groupSize;

        public override void InitPart1() {
            _packages = inputSet.Select(long.Parse).ToArray();
            _groupSize = _packages.Sum() / 3;
        }

        public override string part1Answer => "10723906903";
        public override (string, object) SolvePart1() {
            (long[] groupA, long[] groupB, long[] groupC) = FindBestGroups();

            Console.WriteLine(groupA.Select(p => $"{p}").Aggregate((a, b) => $"{a}, {b}"));
            Console.WriteLine(groupB.Select(p => $"{p}").Aggregate((a, b) => $"{a}, {b}"));
            Console.WriteLine(groupC.Select(p => $"{p}").Aggregate((a, b) => $"{a}, {b}"));

            return ("Quantum entanglement of Group A: ", groupA.Aggregate((a, b) => a * b));
        }
        
        public override string part2Answer => null;
        public override (string, object) SolvePart2() {
            return (null, null);
        }

        private (long[] a, long[] b, long[] c) FindBestGroups() {
            int groupCount = 1;

            while (groupCount <= _packages.Length) {
                IEnumerable<long[]> groups = DataUtil.GetAllCombinations(_packages, groupCount)
                    .Where(g => g.Sum() == _groupSize);

                if (groups.Count() > 0) {
                    long[] groupA = groups.OrderBy(g => g.Aggregate((a, b) => a * b)).First();
                    FindSubGroups(groupA, out long[] groupB, out long[] groupC);
                    return (groupA, groupB, groupC);
                }
                
                ++groupCount;
            }

            throw new Exception($"Failed to find a group of packages that satisfy challenge conditions");
        }

        private void FindSubGroups(long[] groupA, out long[] groupB, out long[] groupC) {
            long[] packages = _packages.Except(groupA).ToArray();

            Stack<int> indices = new Stack<int>();
            indices.Push(0);

            int iLast = packages.Length - 1;
            while (true) {
                groupB = indices.Select(i => packages[i]).ToArray();

                int compare = groupB.Sum().CompareTo(_groupSize);

                if (compare == 0) {
                    groupC = new long[packages.Length - indices.Count];
                    int iC = 0;
                    for (int i = 0; i < packages.Length; i++) {
                        if (!indices.Contains(i)) {
                            groupC[iC++] = packages[i];
                        }
                    }

                    if (groupC.Sum() == _groupSize) return;
                }

                if (compare >= 0 || indices.Peek() == iLast) {
                    // Base case: a single container matches the group size, or we're at the last index
                    if (indices.Count == 1) break;

                    indices.Pop();
                    indices.Push(indices.Pop() + 1);
                }
                else {
                    indices.Push(indices.Peek() + 1);
                }
            }

            throw new Exception("Failed to find sub groups from the given set");
        }
    }
}
