namespace AdventOfCode {
    public enum Direction {
        Right,
        Left,
        Up,
        Down
    }

    public static class DirectionExtensions {
        public static bool IsHorizontal(this Direction dir) => dir == Direction.Right || dir == Direction.Left;
        public static bool IsPositive(this Direction dir) => dir == Direction.Right || dir == Direction.Up;

        public static int Sign(this Direction dir) => (dir.IsPositive() ? 1 : -1);
    }
}
