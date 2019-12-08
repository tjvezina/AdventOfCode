using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        SpaceImage image = new SpaceImage(25, 6, File.ReadAllLines("input.txt")[0]);

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

        Console.WriteLine(leastZerosValueCounts[1] * leastZerosValueCounts[2]);
    }
}
