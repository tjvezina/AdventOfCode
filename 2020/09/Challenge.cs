using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day09
{
    public class Challenge : BaseChallenge
    {
        private const int PreambleSize = 25;

        private readonly long[] _xmasData;

        private long _invalidNumber;

        public Challenge()
        {
            _xmasData = inputList.Select(long.Parse).ToArray();
        }

        public override object part1ExpectedAnswer => 21806024;
        public override (string message, object answer) SolvePart1()
        {
            Queue<long> subset = new Queue<long>(_xmasData.Take(PreambleSize));

            for (int i = PreambleSize; i < _xmasData.Length; i++)
            {
                long next = _xmasData[i];

                if (subset.ToArray().Subsets(2).All(pair => pair.Sum() != next))
                {
                    _invalidNumber = next;
                    return ($"{{0}} at index {i} breaks the XMAS data rule", _invalidNumber);
                }

                subset.Dequeue();
                subset.Enqueue(next);
            }

            throw new Exception("Failed to find exception, all values followed the rule");
        }
        
        public override object part2ExpectedAnswer => 2986195;
        public override (string message, object answer) SolvePart2()
        {
            for (int iStart = _xmasData.Length - 2; iStart >= 0; iStart--)
            {
                long sum = _xmasData[iStart];
                int iEnd = iStart + 1;

                do
                {
                    sum += _xmasData[iEnd++];
                } while (sum < _invalidNumber && iEnd < _xmasData.Length);

                if (sum == _invalidNumber)
                {
                    long[] values = _xmasData[iStart..iEnd];

                    Console.WriteLine("  " + values.Select((x, i) => $"[{iStart + i}] {x}").Aggregate((a, b) => $"{a}\n+ {b}"));
                    Console.WriteLine($"=       {_invalidNumber}");

                    long[] sorted = values.OrderBy(x => x).ToArray();

                    return ("Encryption weakness: ", sorted.First() + sorted.Last());
                }
            }

            throw new Exception("Failed to find sequential values that sum to the invalid number");
        }
    }
}
