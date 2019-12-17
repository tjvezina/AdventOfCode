namespace AdventOfCode.Year2019 {
    public abstract class BotAction {
        public int charCount => ToString().Length;
    }

    public class Move : BotAction {
        public int units;

        public override string ToString() => $"{units}";
    }

    public class Turn : BotAction {
        public bool left;

        public override string ToString() => (left ? "L" : "R");
    }
}
