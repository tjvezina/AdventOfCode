using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day14 {
     public class Challenge : BaseChallenge {
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

        private Dictionary<string, Reaction> _reactionMap = new Dictionary<string, Reaction>();
        private ElementMap _available = new ElementMap();
        private long _orePerFuel;

        public override void InitPart1() {
            _reactionMap = inputSet.Select(Reaction.Parse).ToDictionary(r => r.output.name);
        }

        public override string part1Answer => "1037742";
        public override (string, object) SolvePart1() {
            ModifyAvailable(ORE, INITIAL_ORE);

            Create(FUEL, 1);

            _orePerFuel = INITIAL_ORE - GetAvailable(ORE);
            return ("{{0}} ore required for 1 fuel", _orePerFuel);
        }
        
        public override void InitPart2() {
            _available.Clear();
        }

        public override string part2Answer => "1572358";
        public override (string, object) SolvePart2() {
            ModifyAvailable(ORE, INITIAL_ORE);
            
            while (Create(FUEL, Math.Max(1, GetAvailable(ORE) / _orePerFuel)));

            return ($"{{0}} fuel created from {INITIAL_ORE} ore", _available[FUEL]);
        }

        private bool Consume(string element, long amount) {
            long available = GetAvailable(element);
            if (available < amount && !Create(element, amount - available)) return false;

            ModifyAvailable(element, -amount);
            return true;
        }

        private bool Create(string element, long amount) {
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

        private long GetAvailable(string element) => _available.ContainsKey(element) ? _available[element] : 0;

        private void ModifyAvailable(string element, long amount) {
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
}