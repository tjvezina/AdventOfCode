using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day17
{
     public class Challenge : BaseChallenge
     {
        private const int Total = 150;

        private readonly IReadOnlyList<int> _containers;

        public Challenge() => _containers = inputList.Select(int.Parse).ToList();

        public override object part1ExpectedAnswer => 4372;
        public override (string message, object answer) SolvePart1()
        {
            return ("Valid combinations: ", CountCombinations(write:false));
        }
        
        public override object part2ExpectedAnswer => 4;
        public override (string message, object answer) SolvePart2()
        {
            return ("Valid combinations: ", CountCombinations(write:true, minimize:true));
        }

        private int CountCombinations(bool write, bool minimize = false)
        {
            int combos = 0;

            for (int n = 1; n <= _containers.Count; n++)
            {
                int[] indices = Enumerable.Range(0, n).ToArray();

                do
                {
                    IEnumerable<int> usedContainers = indices.Select(i => _containers[i]);

                    if (usedContainers.Sum() == Total)
                    {
                        combos++;

                        if (write)
                        {
                            Console.WriteLine(usedContainers.Select(c => $"{c}").Aggregate((a, b) => $"{a} + {b}"));
                        }
                    }
                } while (DataUtil.NextCombination(indices, _containers.Count));

                if (minimize && combos > 0) break;
            }

            return combos;
        }
    }
}
