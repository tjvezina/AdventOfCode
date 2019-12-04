using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static readonly char[] VOWELS = new char[] { 'a', 'e', 'i', 'o', 'u' };
    private static readonly List<string> BAD_STRINGS = new List<string> { "ab", "cd", "pq", "xy" };

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        Console.WriteLine("Nice strings: " + input.Count(IsNice));
    }

    private static bool IsNice(string str) {
        // If the string contains any of the bad strings, it is naughty
        if (BAD_STRINGS.Any(s => str.Contains(s))) return false;
        // If the string contains less than 3 vowels, it is naughty
        if (VOWELS.Sum(v => str.Count(c => c == v)) < 3) return false;

        char lastChar = str[0];
        for (int i = 1; i < str.Length; ++i) {
            char nextChar = str[i];

            // If the string contains 2 consecutive identical letters, it is nice
            if (lastChar == nextChar) return true;

            lastChar = nextChar;
        }

        return false;
    }
}