using System;
using System.Linq;

public class LightBoard {
    public const int SIZE = 1000;

    private bool[,] _grid = new bool[SIZE, SIZE];

    public int LitCount {
        get {
            int count = 0;
            for (int x = 0; x < SIZE; ++x) {
                for (int y = 0; y < SIZE; ++y) {
                    if (_grid[x, y]) ++count;
                }
            }
            return count;
        }
    }

    public void TurnOn(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                _grid[x, y] = true;
            }
        }
    }

    public void TurnOff(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                _grid[x, y] = false;
            }
        }
    }

    public void Toggle(Point a, Point b) {
        for (int x = a.x; x <= b.x; ++x) {
            for (int y = a.y; y <= b.y; ++y) {
                _grid[x, y] = !_grid[x, y];
            }
        }
    }
}
