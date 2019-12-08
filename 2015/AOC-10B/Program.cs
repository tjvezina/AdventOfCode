using System;
using System.IO;

public static class Program {
    private const string INPUT = "1113122113";
    private const int ITERATIONS = 50;

    private static void Main(string[] args) {
        DNA dna = new DNA(File.ReadAllLines("DNA.txt"));
        
        int[] input = new int[dna.elementCount];
        input[86] = 1;

        for (int i = 0; i <= 40; ++i) {
            Console.WriteLine($"Iter {i}: {dna.GetIterationLength(input, i)}");
        }
    }
}
