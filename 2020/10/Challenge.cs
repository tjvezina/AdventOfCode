using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day10
{
    public class Challenge : BaseChallenge
    {
        private readonly int[] _adapters;

        public Challenge()
        {
            // Sort the adapters, including the output (0) and device (max + 3)
            _adapters = inputList.Select(int.Parse).OrderBy(x => x).ToArray();
            _adapters = _adapters.Prepend(0).Append(_adapters[^1] + 3).ToArray();
        }

        public override object part1ExpectedAnswer => 1890;
        public override (string message, object answer) SolvePart1()
        {
            List<int> differences = new List<int>();

            for (int i = 0; i < _adapters.Length - 1; i++)
            {
                differences.Add(_adapters[i+1] - _adapters[i]);
            }
            
            int ones = differences.Count(x => x == 1);
            int threes = differences.Count(x => x == 3);

            return ($"1-jolt difference count ({ones}) x 3-jolt difference count ({threes}) = ", ones * threes);
        }
        
        public override object part2ExpectedAnswer => 49607173328384;
        public override (string message, object answer) SolvePart2()
        {
            Dictionary<int, long> pathCountMap = new Dictionary<int, long>();

            return ("There are {0} possible arrangements", GetPathCount(0));

            long GetPathCount(int index)
            {
                if (index == _adapters.Length - 1)
                {
                    return 1;
                }

                int value = _adapters[index];

                long pathSum = 0;

                bool IsIndexValid(int i) => i < _adapters.Length && _adapters[i] - value <= 3;

                foreach (int iNext in Enumerable.Range(index + 1, 3).TakeWhile(IsIndexValid))
                {
                    if (!pathCountMap.ContainsKey(iNext))
                    {
                        pathCountMap[iNext] = GetPathCount(iNext);
                    }

                    pathSum += pathCountMap[iNext];
                }

                return pathSum;
            }
        }
    }
}
