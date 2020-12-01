using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    public static class DirectionExtensions
    {
        public static bool IsHorizontal(this Direction dir) => dir == Direction.Right || dir == Direction.Left;
        public static bool IsPositive(this Direction dir) => dir == Direction.Right || dir == Direction.Up;

        public static int Sign(this Direction dir) => (dir.IsPositive() ? 1 : -1);

        public static IEnumerable<Direction> GetOrthogonal(this Direction dir)
        {
            yield return dir.RotateCW();
            yield return dir.RotateCCW();
        }

        public static Direction GetOpposite(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Right: return Direction.Left;
                case Direction.Left:  return Direction.Right;
                case Direction.Up:    return Direction.Down;
                case Direction.Down:  return Direction.Up;
            }
            throw new Exception($"Unknown direction {dir}");
        }

        public static Direction RotateCW(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:    return Direction.Right;
                case Direction.Right: return Direction.Down;
                case Direction.Down:  return Direction.Left;
                case Direction.Left:  return Direction.Up;
            }
            throw new Exception($"Unknown direction {dir}");
        }

        public static Direction RotateCCW(this Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:    return Direction.Left;
                case Direction.Left:  return Direction.Down;
                case Direction.Down:  return Direction.Right;
                case Direction.Right: return Direction.Up;
            }
            throw new Exception($"Unknown direction {dir}");
        }
    }
}
