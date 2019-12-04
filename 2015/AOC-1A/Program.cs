using System;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string input = File.ReadAllLines("input.txt")[0];

        int up = input.Count(c => c == '(');
        int down = input.Length - up;

        Console.WriteLine(up - down);
    }
}
