using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day24
{
    public class Challenge : BaseChallenge
    {
        private readonly int[] _packages;

        public Challenge() => _packages = inputList.Select(int.Parse).ToArray();

        public override object part1ExpectedAnswer => 10723906903;
        public override (string message, object answer) SolvePart1()
        {
            List<int[]> groups = FindBestGroups(groupCount:3);
            return ("Quantum entanglement of first group: ", GetQuantumEntanglement(groups[0]));
        }
        
        public override object part2ExpectedAnswer => 74850409;
        public override (string message, object answer) SolvePart2()
        {
            // Try some other counts, just for fun
            FindBestGroups(groupCount:6);
            //FindBestGroups(groupCount:8); // 8 is possible, but slow

            List<int[]> groups = FindBestGroups(groupCount:4);
            return ("Quantum entanglement of first group: ", GetQuantumEntanglement(groups[0]));
        }

        private long GetQuantumEntanglement(IList<int> group) => group.Select(p => (long)p).Aggregate((a, b) => a * b);

        private List<int[]> FindBestGroups(int groupCount)
        {
            int packagesSum = _packages.Sum();
            Debug.Assert(packagesSum % groupCount == 0, $"Unable to evenly divide packages into {groupCount} groups");
            int groupSum = packagesSum / groupCount;

            List<int[]> groups = new List<int[]>();

            int groupSize = 1;
            while (groupSize <= _packages.Length)
            {
                IList<int> group = _packages.Subsets(groupSize)
                    .Where(g => g.Sum() == groupSum)
                    .OrderBy(GetQuantumEntanglement)
                    .FirstOrDefault();

                if (group != null)
                {
                    groups.Add(group.ToArray());
                    break;
                }

                groupSize++;
            }

            Debug.Assert(groups.Count > 0, $"Failed to find any groups that total {groupSum}");

            bool FindNextGroup(IReadOnlyList<int> except, int index = 1)
            {
                if (index == groupCount - 1)
                {
                    int[] lastGroup = _packages.Except(except).ToArray();
                    groups.Insert(1, lastGroup);
                    return true;
                }

                foreach (int[] subGroup in FindGroups(groupSum, except))
                {
                    if (FindNextGroup(except.Concat(subGroup).ToArray(), index + 1))
                    {
                        groups.Insert(1, subGroup);
                        return true;
                    }
                }

                return false;
            }

            FindNextGroup(groups[0]);

            Debug.Assert(groups.Count == groupCount, "Failed to find secondary groups to match the first");

            Console.WriteLine($" -- {groupCount} Groups -- ");
            for (int i = 0; i < groups.Count; i++)
            {
                Console.WriteLine($"Group {i+1}: {groups[i].OrderBy(p => p).Select(p => $"{p}").Aggregate((a, b) => $"{a} {b}")}");
            }

            return groups;
        }

        private IEnumerable<int[]> FindGroups(int groupSum, IEnumerable<int> except)
        {
            int[] packages = _packages.Except(except).ToArray();

            Stack<int> indices = new Stack<int>(new[] { 0 });

            int iLast = packages.Length - 1;
            while (true)
            {
                int[] group = indices.Select(i => packages[i]).ToArray();

                int compare = group.Sum().CompareTo(groupSum);

                if (compare == 0) yield return group;

                if (compare >= 0 || indices.Peek() == iLast)
                {
                    // Base case: a single container matches the group size, or we're at the last index
                    if (indices.Count == 1) break;

                    indices.Pop();
                    indices.Push(indices.Pop() + 1);
                }
                else
                {
                    indices.Push(indices.Peek() + 1);
                }
            }
        }
    }
}
