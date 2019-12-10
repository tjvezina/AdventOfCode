using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static string _molecule;
    private static List<KeyValuePair<string, string>> _replacements = new List<KeyValuePair<string, string>>();

    private static void Main(string[] args) {
        LoadInput();
        CountStepToMolecule();
    }

    private static void LoadInput() {
        Queue<string> input = new Queue<string>(File.ReadAllLines("input.txt"));

        string data;
        while (!string.IsNullOrEmpty(data = input.Dequeue())) {
            string[] parts = data.Split(' ');
            _replacements.Add(new KeyValuePair<string, string>(parts[0], parts[2]));
        }

        _molecule = input.Dequeue();
    }

    private static void CountStepToMolecule() {
        int Count(string match) {
            int count = 0;
            for (int i = 0; (i = _molecule.IndexOf(match, i)) != -1; ++i, ++count);
            return count;
        }

        int steps = _molecule.Count(char.IsUpper) - Count("Rn") - Count("Ar") - 2 * Count("Y") - 1;

        Console.WriteLine("Steps to molecule: " + steps);
    }
}
