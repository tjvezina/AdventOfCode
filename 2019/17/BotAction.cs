namespace AdventOfCode.Year2019.Day17 {
    public enum Turn { Left, Right }

    public struct BotAction {
        public Turn turn;
        public int steps;

        public BotAction(Turn turn, int steps) {
            this.turn = turn;
            this.steps = steps;
        }

        public int charCount => ToString().Length;

        public override string ToString() => $"{turn.ToString()[0]},{steps}";

        public bool Equals(BotAction a) => a.turn == turn && a.steps == steps;
        public override bool Equals(object obj) => obj is BotAction && Equals((BotAction)obj);
        public override int GetHashCode() => steps * (turn == Turn.Left ? 1 : -1);
        public static bool operator==(BotAction a, BotAction b) => a.Equals(b);
        public static bool operator!=(BotAction a, BotAction b) => !(a == b);
    }
}
