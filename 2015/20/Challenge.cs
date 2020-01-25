using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day20 {
    public class Challenge : BaseChallenge {
        private const int Target = 29_000_000;
        private const int HouseLimit = 50; // Max houses/elf

        public override string part1ExpectedAnswer => "665280";
        public override (string message, object answer) SolvePart1() {
            for (int i = 1; ; i++) {
                if (CountPresents(i) >= Target) {
                    return ("House ", i);
                }
            }
        }

        public override string part2ExpectedAnswer => "705600";
        public override (string message, object answer) SolvePart2() {
            for (int i = 1; ; i++) {
                if (CountPresentsLimited(i) >= Target) {
                    return ("House ", i);
                }
            }
        }

        private int CountPresents(int house) {
            int presents = 0;
            for (int i = 1; i <= Math.Sqrt(house); i++) {
                if (house % i == 0) {
                    presents += i;
                    if (house / i != i) {
                        presents += house / i;
                    }
                }
            }
            return presents * 10;
        }

        private int CountPresentsLimited(int h) {
            int p = 0;
            int min = (int)Math.Ceiling((double)h / HouseLimit);
            for (int i = 1; i <= Math.Sqrt(h); i++) {
                if (h % i == 0) {
                    int j = h / i;
                    if (i >= min) p += i;
                    if (j >= min && j != i) p += j;
                }
            }
            return p * 11;
        }
    }
}
