using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day10 {
    public class Challenge : BaseChallenge {
        private const string PATTERN_INPUT = @"value (\d+) goes to bot (\d+)";
        private const string PATTERN_BOT = @"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)";

        private InputNode[] _inputs;
        private BotNode[] _bots;
        private OutputNode[] _outputs;

        public override void InitPart1() {
            string[] inputNodeData = inputArray.Where(i => i.StartsWith("value")).ToArray();
            string[] botNodeData = inputArray.Where(i => i.StartsWith("bot")).ToArray();

            _bots = Enumerable.Range(0, botNodeData.Length).Select(i => new BotNode(i)).ToArray();
            _outputs = Enumerable.Range(0, inputNodeData.Length).Select(i => new OutputNode()).ToArray();

            foreach (string data in botNodeData) {
                Match match = Regex.Match(data, PATTERN_BOT);
                int index = int.Parse(match.Groups[1].Value);
                string minType = match.Groups[2].Value;
                int minIndex = int.Parse(match.Groups[3].Value);
                string maxType = match.Groups[4].Value;
                int maxIndex = int.Parse(match.Groups[5].Value);

                _bots[index].minOutput = (minType == "bot" ? (IOutputNode)_bots[minIndex] : _outputs[minIndex]);
                _bots[index].maxOutput = (maxType == "bot" ? (IOutputNode)_bots[maxIndex] : _outputs[maxIndex]);
            }

            InputNode ParseInputNode(string data) {
                Match match = Regex.Match(data, PATTERN_INPUT);
                int value = int.Parse(match.Groups[1].Value);
                int index = int.Parse(match.Groups[2].Value);

                return new InputNode { chip = value, output = _bots[index] };
            }

            _inputs = inputNodeData.Select(ParseInputNode).ToArray();
        }

        public override string part1Answer => "98";
        public override (string, object) SolvePart1() {
            Queue<INode> _openSet = new Queue<INode>(_inputs);

            void GiveChip(IOutputNode output, int chip) {
                if (output != null && output.TakeChip(chip)) {
                    _openSet.Enqueue(output);
                }
            }

            while (_openSet.TryDequeue(out INode node)) {
                switch (node) {
                    case InputNode inputNode:
                        GiveChip(inputNode.output, inputNode.chip);
                        break;
                    case BotNode bot:
                        GiveChip(bot.minOutput, bot.minChip);
                        GiveChip(bot.maxOutput, bot.maxChip);
                        break;
                }
            }
            
            BotNode answerBot = _bots.Single(b => b.minChip == 17 && b.maxChip == 61);

            return ("Bot {0} compares 17 & 61 microchips.", answerBot.number);
        }
        
        public override string part2Answer => "4042";
        public override (string, object) SolvePart2() {
            return ("Product of first 3 outputs: ", _outputs.Take(3).Select(o => o.chip).Aggregate((a, b) => a * b));
        }
    }
}
