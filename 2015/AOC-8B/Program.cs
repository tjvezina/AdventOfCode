using System;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        Console.WriteLine(input.Sum(GetEscapedCount) - input.Sum(s => s.Length));
    }

    private static int GetEscapedCount(string str) {
        for (int i = str.Length - 1; i >= 0; --i) {
            if (str[i] == '\"' || str[i] == '\\') {
                str = str.Substring(0, i) + "\\" + str.Substring(i);
            }
        }

        str = $"\"{str}\"";

        return str.Length;
    }
}
