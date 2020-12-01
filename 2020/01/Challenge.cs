using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day01
{
    public class Challenge : BaseChallenge
    {
        private const int Target = 2020;

        public override string part1ExpectedAnswer => "1014624";
        public override (string message, object answer) SolvePart1()
        {
            int[] numbers = inputList.Select(int.Parse).ToArray();

            for (int iA = 0; iA < numbers.Length; iA++)
            {
                for (int iB = iA + 1; iB < numbers.Length; iB++)
                {
                    int a = numbers[iA];
                    int b = numbers[iB];

                    if (a + b == Target)
                    {
                        return ($"[{iA}] {a} x [{iB}] {b} = ", a * b);
                    }
                }
            }

            throw new Exception($"Failed to find a pair of numbers that sum to {Target}!");
        }
        
        public override string part2ExpectedAnswer => "80072256";
        public override (string message, object answer) SolvePart2()
        {
            int[] numbers = inputList.Select(int.Parse).ToArray();

            for (int iA = 0; iA < numbers.Length; iA++)
            {
                for (int iB = iA + 1; iB < numbers.Length; iB++)
                {
                    for (int iC = iB + 1; iC < numbers.Length; iC++)
                    {
                        int a = numbers[iA];
                        int b = numbers[iB];
                        int c = numbers[iC];

                        if (a + b + c == Target)
                        {
                            return ($"[{iA}] {a} x [{iB}] {b} x [{iC}] {c} = ", a * b * c);
                        }
                    }
                }
            }

            throw new Exception($"Failed to find a set of three numbers that sum to {Target}!");
        }
    }
}
