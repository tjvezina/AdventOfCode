using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day16 {
     public class Challenge : BaseChallenge {
        private static readonly int[] PATTERN = new[] { 0, 1, 0, -1 };

        private int[] _input;

        public override void InitPart1() {
            _input = input.Select(c => c - '0').ToArray();
        }

        public override string SolvePart1() {
            int[] output = new int[_input.Length];
            _input.CopyTo(output, 0);
            for (int i = 0; i < 100; ++i) {
                output = ApplyPhase(output);
            }

            return $"First 8 digits of result: {output.Take(8).Aggregate((a, b) => a * 10 + b)}";
        }
        
        public override string SolvePart2() {
            int offset = _input.Take(7).Aggregate((a, b) => a * 10 + b);
            int fullLength = _input.Length * 10_000;
            int length = fullLength - offset;

            double frac = 1 - (double)offset / fullLength;
            frac = 1 - (frac * frac);

            Console.WriteLine($"Offset is {offset}, skipping {frac * 100:0.000}% of calculations");

            int[] output = new int[length];
            for (int i = 0; i < length; ++i) {
                output[i] = _input[(i + offset) % _input.Length];
            }
            for (int i = 0; i < 100; ++i) {
                output = ApplyPhaseShortcut(output);
            }

            return $"First 8 digits of result: {output.Take(8).Aggregate((a, b) => a * 10 + b)}";
        }

        private int[] ApplyPhase(int[] input) {
            int[] output = new int[input.Length];

            int GetPatternValue(int iOut, int iIn) => PATTERN[((iIn + 1) / (iOut + 1)) % PATTERN.Length];

            for (int iOut = 0; iOut < output.Length; ++iOut) {
                int sum = 0;
                for (int iIn = 0; iIn < input.Length; ++iIn) {
                    sum += input[iIn] * GetPatternValue(iOut, iIn);
                }
                output[iOut] = Math.Abs(sum % 10);
            }

            return output;
        }

        private int[] ApplyPhaseShortcut(int[] input) {
            int[] output = new int[input.Length];

            int sum = 0;
            for (int i = output.Length - 1; i >= 0; --i) {
                sum += input[i];
                output[i] = sum % 10;
            }

            return output;
        }
    }
}
