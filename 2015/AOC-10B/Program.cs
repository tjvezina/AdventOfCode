using System;

public static class Program {
    private const string INPUT = "1113122113";
    private const int ITERATIONS = 50;

    private static void Main(string[] args) {
        MatrixInt input = new MatrixInt(new int[,] { {1}, {1}, {1} });

        MatrixInt dna = new MatrixInt(new int[,] {
            { 1, 1, 0 },
            { 0, 0, 2 },
            { 0, 1, 0 }
        });
        
        Console.WriteLine(MatrixInt.Power(dna, 4) * input);
    }
}
