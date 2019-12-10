using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private static string _molecule;
    private static List<KeyValuePair<string, string>> _replacements = new List<KeyValuePair<string, string>>();

    private static void Main(string[] args) {
        LoadInput();
        CountSubMolecules();
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

    private static void CountSubMolecules() {
        HashSet<string> subMolecules = new HashSet<string>();

        foreach ((string pattern, string replace) in _replacements) {
            int i = 0;
            while ((i = _molecule.IndexOf(pattern, i)) != -1) {
                subMolecules.Add(_molecule.Remove(i, pattern.Length).Insert(i, replace));
                ++i;
            }
        }

        Console.WriteLine("Distinct submolecules: " + subMolecules.Count);
    }
}
