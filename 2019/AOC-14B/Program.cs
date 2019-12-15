using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private struct ElementData {
        public static ElementData Parse(string rawData) {
            string[] parts = rawData.Split(' ');
            return new ElementData {
                name = parts[1],
                amount = long.Parse(parts[0])
            };
        }

        public string name;
        public long amount;

        public override string ToString() => $"{name} {amount}";
    }

    private struct Reaction {
        public static Reaction Parse(string rawData) {
            string[] parts = rawData.Split(new[] { ", ", " => " }, StringSplitOptions.RemoveEmptyEntries);
            return new Reaction {
                inputs = parts.Take(parts.Length - 1).Select(ElementData.Parse).ToArray(),
                output = ElementData.Parse(parts.Last()),
            };
        }

        public ElementData[] inputs;
        public ElementData output;
    }

    private class ElementMap : Dictionary<string, long> { }

    private const string ORE = "ORE";
    private const string FUEL = "FUEL";
    private const long INITIAL_ORE = 1_000_000_000_000;

    private static Dictionary<string, Reaction> _reactionMap = new Dictionary<string, Reaction>();
    private static ElementMap _available = new ElementMap();

    private static void Main(string[] args) {
        _reactionMap = File.ReadAllLines("input.txt").Select(Reaction.Parse).ToDictionary(r => r.output.name);

        ModifyAvailable(ORE, INITIAL_ORE);

        Create(FUEL, 1);
        long orePerFuel = INITIAL_ORE - GetAvailable(ORE);
        
        while (Create(FUEL, Math.Max(1, GetAvailable(ORE) / orePerFuel)));

        Console.WriteLine($"{orePerFuel} ore required for 1 fuel");
        Console.WriteLine($"{_available[FUEL]} fuel created from {INITIAL_ORE:N0} ore");
    }

    private static bool Consume(string element, long amount) {
        long available = GetAvailable(element);
        if (available < amount && !Create(element, amount - available)) return false;

        ModifyAvailable(element, -amount);
        return true;
    }

    private static bool Create(string element, long amount) {
        if (!_reactionMap.ContainsKey(element)) return false;

        Reaction reaction = _reactionMap[element];

        long reactionCount = ((amount - 1) / reaction.output.amount) + 1;
        long leftover = (reaction.output.amount * reactionCount) - amount;

        foreach (ElementData subElement in reaction.inputs) {
            if (!Consume(subElement.name, subElement.amount * reactionCount)) {
                return false;
            }
        }

        ModifyAvailable(element, reaction.output.amount * reactionCount);
        return true;
    }

    private static long GetAvailable(string element) => _available.ContainsKey(element) ? _available[element] : 0;

    private static void ModifyAvailable(string element, long amount) {
        if (amount == 0) return;

        if (!_available.ContainsKey(element)) {
            _available[element] = 0;
        }

        _available[element] += amount;

        if (_available[element] == 0) {
            _available.Remove(element);
        }
    }
}
