using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private enum PropType {
        Equal,
        Greater,
        Less
    }

    private class Sue {
        public Dictionary<string, int> properties = new Dictionary<string, int>();
    }

    private static readonly Dictionary<string, PropType> PROP_TYPES = new Dictionary<string, PropType> {
        { "cats", PropType.Greater },
        { "tress", PropType.Greater },
        { "pomeranians", PropType.Less },
        { "goldfish", PropType.Less }
    };

    private static PropType GetPropType(string prop) {
        return (PROP_TYPES.ContainsKey(prop) ? PROP_TYPES[prop] : PropType.Equal);
    }

    private static void Main(string[] args) {
        List<Sue> sues = new List<Sue>();
        foreach (string data in File.ReadAllLines("input.txt")) {
            Sue sue = new Sue();
            int splitIndex = data.IndexOf(':');
            foreach (string prop in data.Substring(splitIndex + 1).Split(',')) {
                string[] parts = prop.Split(':');
                sue.properties[parts[0].Trim()] = int.Parse(parts[1].Trim());
            }
            sues.Add(sue);
        }

        Dictionary<string, int> knownProps = new Dictionary<string, int>();
        foreach (string prop in File.ReadAllLines("properties.txt")) {
            string[] parts = prop.Split(':');
            knownProps[parts[0].Trim()] = int.Parse(parts[1].Trim());
        }

        int sueIndex = -1;
        for (int i = 0; i < sues.Count; ++i) {
            Sue sue = sues[i];
            bool isMatch = true;
            foreach ((string prop, int value) in sue.properties) {
                switch (GetPropType(prop)) {
                    case PropType.Equal:
                        isMatch &= (value == knownProps[prop]);
                        break;
                    case PropType.Greater:
                        isMatch &= (value > knownProps[prop]);
                        break;
                    case PropType.Less:
                        isMatch &= (value < knownProps[prop]);
                        break;
                }
            }
            if (isMatch) {
                if (sueIndex != -1) {
                    Console.WriteLine("Multiple matches found! Problem is unsolvable.");
                    return;
                }
                sueIndex = i;
            }
        }

        Console.WriteLine($"Matching aunt Sue found: #{sueIndex+1}");
    }
}
