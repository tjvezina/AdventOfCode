using System;
using System.Diagnostics;

namespace AdventOfCode.Year2019.Day08
{
    public class SpaceImage
    {
        private const int Black = 0;
        private const int White = 1;
        private const int Transparent = 2;

        int[,,] _data;

        public int width => _data.GetLength(0);
        public int height => _data.GetLength(1);
        public int depth => _data.GetLength(2);

        public int this[int x, int y, int z] => _data[x, y, z];

        public SpaceImage(int width, int height, string rawData)
        {
            Debug.Assert(rawData.Length % (width * height) == 0, "Invalid data for size, failed to load image");

            int depth = rawData.Length / (width * height);
            _data = new int[width, height, depth];

            int index = 0;
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        _data[x, y, z] = rawData[index++] - '0'; // Char digit to int
                    }
                }
            }
        }

        public bool[,] Flatten()
        {
            bool[,] pixels = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        int color = _data[x, y, z];
                        if (color != Transparent)
                        {
                            pixels[x, y] = (color == White);
                            break;
                        }
                    }
                }
            }

            return pixels;
        }
    }
}
