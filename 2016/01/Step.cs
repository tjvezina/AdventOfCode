using System;

namespace AdventOfCode.Year2016.Day01
{

    public struct Step
    {
        private enum Turn { Left, Right }

        private Turn _turn;
        public int distance;

        public Step(string data)
        {
            _turn = data[0] switch
            {
                'L' => Turn.Left,
                'R' => Turn.Right,
                _   => throw new Exception($"Failed to parse turn direction from char {data[0]}")
            };

            distance = int.Parse(data.Substring(1));
        }

        public void ApplyTurn(ref Direction dir)
        {
            dir = (_turn == Turn.Left ? dir.RotateCCW() : dir.RotateCW());
        }

        public void Apply(ref Point pos, ref Direction dir)
        {
            ApplyTurn(ref dir);
            pos += (Point)dir * distance;
        }
    }
}
