using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private const int TOTAL = 150;

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        int[] containers = new int[input.Length];
        for (int i = 0; i < input.Length; ++i) {
            containers[i] = int.Parse(input[i]);
        }

        int combos = 0;

        for (int n = 1; n <= containers.Length; ++n) {
            int[] indices = new int[n];
            for (int i = 0; i < n; ++i) {
                indices[i] = i;
            }

            do {
                IEnumerable<int> usedContainers = indices.Select(i => containers[i]);

                if (usedContainers.Sum() == TOTAL) {
                    ++combos;
                    Console.WriteLine(usedContainers.Select(c => $"{c}").Aggregate((a, b) => $"{a} + {b}"));
                }
            } while (Advance(indices, containers.Length));
        }

        Console.WriteLine("Valid combinations: " + combos);
    }

    private static bool Advance(int[] indices, int count) {
        int n = indices.Length;
        int i = n - 1;
        for (int j = count - 1; i >= 0; --i, --j) {
            if (indices[i] < j) {
                break;
            }
        }

        if (i < 0) {
            return false;
        }

        for (int j = indices[i] + 1; i < n; ++i, ++j) {
            indices[i] = j;
        }

        return true;
    }
}
