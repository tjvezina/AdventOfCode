using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day16 {
     public class Challenge : BaseChallenge {
        private enum PropType {
            Equal,
            Greater,
            Less
        }

        private class Sue {
            public Dictionary<string, int> properties = new Dictionary<string, int>();
        }

        private List<Sue> _sues = new List<Sue>();
        private Dictionary<string, int> _knownProps = new Dictionary<string, int>();

        public override void InitPart1() {
            foreach (string data in inputArray) {
                Sue sue = new Sue();
                int splitIndex = data.IndexOf(':');
                foreach (string prop in data.Substring(splitIndex + 1).Split(',')) {
                    string[] parts = prop.Split(':');
                    sue.properties[parts[0].Trim()] = int.Parse(parts[1].Trim());
                }
                _sues.Add(sue);
            }
            
            foreach (string prop in LoadFileLines("properties.txt")) {
                string[] parts = prop.Split(':');
                _knownProps[parts[0].Trim()] = int.Parse(parts[1].Trim());
            }
        }

        public override string part1Answer => "40";
        public override (string, object) SolvePart1() {
            PropType GetPropType(string prop) => PropType.Equal;

            return ("Matching aunt Sue found: #", FindSue(GetPropType));
        }

        public override string part2Answer => "241";
        public override (string, object) SolvePart2() {
            PropType GetPropType(string prop) {
                return prop switch {
                    "cats" => PropType.Greater,
                    "trees" => PropType.Greater,
                    "pomeranians" => PropType.Less,
                    "goldfish" => PropType.Less,
                    _ => PropType.Equal
                };
            }

            return ("Matching aunt Sue found: #", FindSue(GetPropType));
        }
        
        private int FindSue(Func<string, PropType> getPropType) {
            int sueIndex = -1;
            for (int i = 0; i < _sues.Count; ++i) {
                Sue sue = _sues[i];
                bool isMatch = true;
                foreach ((string prop, int value) in sue.properties) {
                    isMatch &= getPropType(prop) switch {
                        PropType.Equal => (value == _knownProps[prop]),
                        PropType.Greater => (value > _knownProps[prop]),
                        PropType.Less => (value < _knownProps[prop]),
                        _ => throw new Exception("Unknown property type")
                    };
                }
                if (isMatch) {
                    if (sueIndex != -1) {
                        Console.WriteLine("Multiple matches found! Problem is unsolvable.");
                        break;
                    }
                    sueIndex = i;
                }
            }

            return sueIndex + 1;
        }
    }
}
