using System;

namespace AdventOfCode.Year2015.Day06
{
    public class LightBoard
    {
        private const int Size = 1000;

        private readonly int[,] _grid = new int[Size, Size];

        private readonly Func<int, int> _turnOn;
        private readonly Func<int, int> _turnOff;
        private readonly Func<int, int> _toggle;

        public LightBoard(Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle)
        {
            _turnOn = turnOn;
            _turnOff = turnOff;
            _toggle = toggle;
        }

        public int brightness
        {
            get
            {
                int value = 0;
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        value += _grid[x, y];
                    }
                }
                return value;
            }
        }

        public void TurnOn(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = _turnOn(_grid[x, y]);
                }
            }
        }

        public void TurnOff(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = _turnOff(_grid[x, y]);
                }
            }
        }

        public void Toggle(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = _toggle(_grid[x, y]);
                }
            }
        }
    }
}
