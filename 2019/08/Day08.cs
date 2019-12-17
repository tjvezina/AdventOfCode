using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day08 : Challenge {
        private SpaceImage image;

        private void Init(string input) {
            image = new SpaceImage(25, 6, input);
        }

        protected override string SolvePart1() {
            int leastZerosLayer = -1;
            int leastZeros = int.MaxValue;
            int[] leastZerosValueCounts = new int[3];

            for (int z = 0; z < image.depth; ++z) {
                int[] valueCounts = new int[3];
                for (int y = 0; y < image.height; ++y) {
                    for (int x = 0; x < image.width; ++x) {
                        ++valueCounts[image[x, y, z]];
                    }
                }

                if (valueCounts[0] < leastZeros) {
                    leastZeros = valueCounts[0];
                    leastZerosLayer = z;
                    valueCounts.CopyTo(leastZerosValueCounts, 0);
                }
            }

            return $"Image validation output: {leastZerosValueCounts[1] * leastZerosValueCounts[2]}";
        }
        
        protected override string SolvePart2() {
           image.Draw();
           return null;
        }
    }
}
