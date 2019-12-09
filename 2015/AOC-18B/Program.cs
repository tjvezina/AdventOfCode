using System;
using System.IO;

public static class Program {
    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");
        LightBoard board = new LightBoard(input);

        for (int i = 0; i < 100; ++i) {
            board.Update();
        }

        Console.WriteLine("Lights on: " + board.litCount);
    }
}
