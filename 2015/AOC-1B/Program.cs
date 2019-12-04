using System;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string input = File.ReadAllLines("input.txt")[0];

        int floor = 0;

        for (int i = 0; i < input.Length; ++i) {
            if (input[i] == '(') ++floor;
            else                 --floor;

            if (floor < 0) {
                Console.WriteLine("Entered basement at position " + (i + 1));
                return;
            }
        }

        Console.WriteLine("Did not enter basement.");
    }
}
