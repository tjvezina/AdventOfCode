using System;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        Console.WriteLine(input.Sum(s => s.Length) - input.Sum(GetCharCount));
    }

    private static int GetCharCount(string str) {
        // Remove enclosing quotes
        str = str.Substring(1, str.Length - 2);

        int length = str.Length;

        int escIndex;
        while ((escIndex = str.IndexOf('\\')) != -1) {
            int delta;
            if (str[escIndex + 1] == 'x') {
                delta = 3;
            } else {
                delta = 1;
            }

            length -= delta;
            str = str.Substring(escIndex + 1 + delta);
        }

        return length;
    }
}
