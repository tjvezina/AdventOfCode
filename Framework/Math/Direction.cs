using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace AdventOfCode
{
    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    [PublicAPI]
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

        public static Direction GetOpposite(this Direction direction)
        {
            return direction switch
            {
                Direction.Right => Direction.Left,
                Direction.Left =>  Direction.Right,
                Direction.Up =>    Direction.Down,
                Direction.Down =>  Direction.Up,
                _ => throw new Exception($"Unknown direction {direction}")
            };
        }

        public static Direction RotateCW(this Direction direction)
        {
            return direction switch
            {
                Direction.Up =>    Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down =>  Direction.Left,
                Direction.Left =>  Direction.Up,
                _ => throw new Exception($"Unknown direction {direction}")
            };
        }

        public static Direction RotateCCW(this Direction direction)
        {
            return direction switch
            {
                Direction.Up =>    Direction.Left,
                Direction.Left =>  Direction.Down,
                Direction.Down =>  Direction.Right,
                Direction.Right => Direction.Up,
                _ => throw new Exception($"Unknown direction {direction}")
            };
        }
    }
}
