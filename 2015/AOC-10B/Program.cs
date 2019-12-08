using System;
using System.IO;

public static class Program {
    private const string INPUT = "1113122113";

    private static void Main(string[] args) {
        DNA dna = new DNA(File.ReadAllLines("DNA.txt"));
        
        int[] input = new int[dna.elementCount];
        input[86] = 1; // Element #87 (index 86) is equivalent to the input string 1113122113

        void PrintIteration(int iter) => Console.WriteLine($"Iter {iter}: {dna.GetIterationLength(input, iter)}");

        PrintIteration(40);
        PrintIteration(50);
        PrintIteration(1000);
        PrintIteration(1000000);
    }
}
