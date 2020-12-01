using System;

namespace AdventOfCode.Year2015.Day06
{
    public class LightBoard
    {
        public const int Size = 1000;

        private int[,] _grid = new int[Size, Size];

        private Func<int, int> turnOn;
        private Func<int, int> turnOff;
        private Func<int, int> toggle;

        public LightBoard(Func<int, int> turnOn, Func<int, int> turnOff, Func<int, int> toggle)
        {
            this.turnOn = turnOn;
            this.turnOff = turnOff;
            this.toggle = toggle;
        }

        public int Brightness
        {
            get
            {
                int brightness = 0;
                for (int x = 0; x < Size; x++)
                {
                    for (int y = 0; y < Size; y++)
                    {
                        brightness += _grid[x, y];
                    }
                }
                return brightness;
            }
        }

        public void TurnOn(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = turnOn(_grid[x, y]);
                }
            }
        }

        public void TurnOff(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = turnOff(_grid[x, y]);
                }
            }
        }

        public void Toggle(Point a, Point b)
        {
            for (int x = a.x; x <= b.x; x++)
            {
                for (int y = a.y; y <= b.y; y++)
                {
                    _grid[x, y] = toggle(_grid[x, y]);
                }
            }
        }
    }
}
