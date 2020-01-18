using System;

namespace AdventOfCode.Year2016.Day10 {
    public interface INode { }

    public interface IOutputNode : INode {
        bool TakeChip(int chip);
    }

    public class InputNode : INode {
        public IOutputNode output;
        public int chip;
    }

    public class BotNode : IOutputNode {
        public readonly int number;

        public int? chipA;
        public int? chipB;

        public int minChip => chipA != null && chipB != null ? Math.Min(chipA.Value, chipB.Value) : -1;
        public int maxChip => chipA != null && chipB != null ? Math.Max(chipA.Value, chipB.Value) : -1;

        public IOutputNode minOutput;
        public IOutputNode maxOutput;

        public BotNode(int number) => this.number = number;

        public bool TakeChip(int chip) {
            if (chipA == null) {
                chipA = chip;
                return false;
            }

            chipB = chip;
            return true;
        }
    }

    public class OutputNode : IOutputNode {
        public int chip;

        public bool TakeChip(int chip) {
            this.chip = chip;
            return false;
        }
    }
}
