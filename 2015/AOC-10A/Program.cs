using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class Program {
    private const string INPUT = "1113122113";
    private const int ITERATIONS = 40;

    private static Regex regex = new Regex(@"((\d)\2*)");

    private static void Main(string[] args) {
        string data = INPUT;

        Console.WriteLine($"Iter 0: {data.Length}");
        for (int i = 0; i < ITERATIONS; ++i) {
            data = LookAndSay(data);
            Console.WriteLine($"Iter {i + 1}: {data.Length}");
        }
    }

    private static string LookAndSayRegex(string input) {
        return regex.Matches(input).Select(m => $"{m.Length}{m.Value[0]}").Aggregate((a, b) => a + b);
    }

    private static string LookAndSay(string input) {
        string output = string.Empty;

        int prevGroupIndex = 0;
        char prevChar = input[0];
        for (int i = 1; i < input.Length; ++i) {
            char nextChar = input[i];

            if (prevChar != nextChar) {
                // "Look" at each group and "say" it (ex. 111 = 3 1's = 31)
                output += $"{i - prevGroupIndex}{prevChar}";
                prevGroupIndex = i;

                prevChar = nextChar;
            }
        }

        // Handle the final group
        output += $"{input.Length - prevGroupIndex}{prevChar}";

        return output;
    }
}
