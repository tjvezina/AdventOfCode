using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public static class Program {
    private struct ElementData {
        public string name;
        public int amount;

        public ElementData(string rawData) {
            string[] parts = rawData.Trim().Split(' ');
            name = parts[1];
            amount = int.Parse(parts[0]);
        }

        public override string ToString() => $"{name} {amount}";
    }

    private struct Reaction {
        public List<ElementData> inputs;
        public ElementData output;
    }

    private class ElementMap : Dictionary<string, int> { }

    private const string INPUT = "ORE";
    private const string OUTPUT = "FUEL";
    private const string SEPARATOR = " => ";

    private static List<Reaction> _reactionList = new List<Reaction>();

    private static void Main(string[] args) {
        LoadData();
        ElementMap output = Reduce(new ElementMap { { OUTPUT, 1 } });

        Console.WriteLine($"{output[INPUT]} {INPUT} required");
    }

    private static void LoadData() {
        foreach (string reaction in File.ReadAllLines("input.txt")) {
            int i = reaction.IndexOf(SEPARATOR);
            string inputData = reaction.Substring(0, i);
            string outputData = reaction.Substring(i + SEPARATOR.Length);

            List<ElementData> inputs = new List<ElementData>();
            foreach (string input in inputData.Split(',')) {
                inputs.Add(new ElementData(input));
            }
            ElementData output = new ElementData(outputData);
            _reactionList.Add(new Reaction { output = output, inputs = inputs });
        }
    }

    private static ElementMap Reduce(ElementMap elements) {
        ElementMap extraElements = new ElementMap();
        bool wasElementReduced;
        do {
            wasElementReduced = false;
            ElementMap nextElements = new ElementMap();

            foreach ((string element, int needed) in elements) {
                int extra = (extraElements.ContainsKey(element) ? extraElements[element] : 0);
                if (extra >= needed) {
                    AdjustMap(extraElements, element, -needed);
                    continue;
                }

                Reaction[] reactions = _reactionList.Where(p => p.output.name == element).ToArray();
                Debug.Assert(reactions.Count() <= 1, $"Multiple ways to create {element} found, branching options not supported");
                if (reactions.Count() == 0) {
                    AdjustMap(nextElements, element, needed);
                    continue;
                }
                Reaction reaction = reactions.First();

                wasElementReduced = true;

                int created = reaction.output.amount;
                int actualNeeded = needed - extra;
                int iterations = ((actualNeeded - 1) / created) + 1;
                int leftover = (created * iterations) - actualNeeded;

                AdjustMap(extraElements, element, -extra + leftover);

                foreach (ElementData input in reaction.inputs) {
                    AdjustMap(nextElements, input.name, input.amount * iterations);
                }
            }

            elements = nextElements;
        } while (wasElementReduced);

        return elements;
    }

    private static void AdjustMap(ElementMap map, string element, int amount) {
        if (amount == 0) return;
        
        if (!map.ContainsKey(element)) {
            map[element] = 0;
        }

        map[element] += amount;

        if (map[element] == 0) {
            map.Remove(element);
        }
    }
}
