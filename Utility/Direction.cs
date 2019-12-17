using System;

namespace AdventOfCode {
    public enum Direction { Right, Left, Up, Down }

    public static class DirectionExtensions {
        public static bool IsHorizontal(this Direction dir) => dir == Direction.Right || dir == Direction.Left;
        public static bool IsPositive(this Direction dir) => dir == Direction.Right || dir == Direction.Up;

        public static int Sign(this Direction dir) => (dir.IsPositive() ? 1 : -1);

        public static Point ToPoint(this Direction dir) {
            switch (dir) {
                case Direction.Right: return Point.right;
                case Direction.Left: return Point.left;
                case Direction.Up: return Point.up;
                case Direction.Down: return Point.down;
                default:
                    throw new Exception("Failed to convert unknown direction to point: " + dir);
            }
        }
    }
}
