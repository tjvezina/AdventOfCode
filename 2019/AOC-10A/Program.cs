using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private static int?[,] _map;
    private static int _width;
    private static int _height;

    private static void Main(string[] args) {
        LoadMap();
        ProcessMap();
        PrintBestLocation();
    }

    private static void LoadMap() {
        string[] input = File.ReadAllLines("input.txt");

        _width = input[0].Length;
        _height = input.Length;
        _map = new int?[_width, _height];
        
        for (int y = 0; y < _height; ++y) {
            string line = input[y];
            for (int x = 0; x < _width; ++x) {
                if (line[x] == '#') {
                    _map[x, y] = 0;
                }
            }
        }
    }

    private static void ProcessMap() {
        for (int y = 0; y < _height; ++y) {
            for (int x = 0; x < _width; ++x) {
                if (_map[x, y] != null) {
                    _map[x, y] = CountVisible(new Point(x, y));
                }
            }
        }
    }

    private static void PrintBestLocation() {
        int maxVisible = 0;
        Point coord = new Point(-1, -1);
        for (int y = 0; y < _height; ++y) {
            for (int x = 0; x < _width; ++x) {
                if (_map[x, y] != null) {
                    if (_map[x, y].Value > maxVisible) {
                        maxVisible = _map[x, y].Value;
                        coord = new Point(x, y);
                    }
                }
            }
        }
        Console.WriteLine($"Max visible asteroids: {maxVisible} {coord}");
    }

    private static int CountVisible(Point p1) {
        int visible = 0;
        for (int y = 0; y < _height; ++y) {
            for (int x = 0; x < _width; ++x) {
                Point p2 = new Point(x, y);
                if (_map[x, y] == null || p1 == p2) continue;

                if (CanSee(p1, p2)) ++visible;
            }
        }
        return visible;
    }

    private static bool CanSee(Point p1, Point p2) {
        Point d = p2 - p1;
        int gcd = GCD(d.x, d.y);

        Point step = d / gcd;
        Point p3 = p1;
        for (int i = 1; i < gcd; ++i) {
            p3 += step;
            if (_map[p3.x, p3.y] != null) return false;
        }

        return true;
    }

    private static int GCD(int a, int b) {
        a = Math.Abs(a);
        b = Math.Abs(b);

        if (a < b) {
            int hold = a;
            a = b;
            b = hold;
        }

        while (b > 0) {
            int r = a % b;
            a = b;
            b = r;
        }

        return a;
    }
}
