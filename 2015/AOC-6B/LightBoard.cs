using System;
using System.Linq;

public class LightBoard {
    public const int SIZE = 1000;

    private int[,] _grid = new int[SIZE, SIZE];

    public int Brightness {
        get {
            int brightness = 0;
            for (int x = 0; x < SIZE; ++x) {
                for (int y = 0; y < SIZE; ++y) {
                    brightness += _grid[x, y];
                }
            }
            return brightness;
        }
    }

    public void TurnOn(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                ++_grid[x, y];
            }
        }
    }

    public void TurnOff(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                _grid[x, y] = Math.Max(0, _grid[x, y] - 1);
            }
        }
    }

    public void Toggle(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                _grid[x, y] += 2;
            }
        }
    }
}
