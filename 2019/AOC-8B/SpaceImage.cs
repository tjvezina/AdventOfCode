using System;
using System.Diagnostics;

public class SpaceImage {
    int[,,] _data;

    public int width => _data.GetLength(0);
    public int height => _data.GetLength(1);
    public int depth => _data.GetLength(2);

    public int this[int x, int y, int z] => _data[x, y, z];

    public SpaceImage(int width, int height, string rawData) {
        Debug.Assert(rawData.Length % (width * height) == 0, "Invalid data for size, failed to load image");

        int depth = rawData.Length / (width * height);
        _data = new int[width, height, depth];

        int index = 0;
        for (int z = 0; z < depth; ++z) {
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    _data[x, y, z] = rawData[index++] - '0'; // Char digit to int
                }
            }
        }
    }

    public void Draw() {
        for (int y = 0; y < height; ++y) {
            for (int x = 0; x < width; ++x) {
                int color = 0;
                for (int z = 0; z < depth; ++z) {
                    int value = _data[x, y, z];
                    if (value != 2) {
                        color = value;
                        break;
                    }
                }
                Console.BackgroundColor = (color == 0 ? ConsoleColor.Black : ConsoleColor.White);
                Console.Write(" ");
            }
            Console.WriteLine("");
        }
        Console.BackgroundColor = ConsoleColor.Black;
    }
}
