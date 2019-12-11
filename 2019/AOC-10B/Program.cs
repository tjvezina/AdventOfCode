using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    private struct Asteroid {
        public Point position;
        public double angle;

        public Asteroid(Point position) {
            this.position = position;
            angle = GetAngle(position - STATION);
        }
    }

    private static readonly Point STATION = new Point(22, 19);

    private static bool[,] _map;
    private static int _width;
    private static int _height;

    private static void Main(string[] args) {
        LoadMap();
        FindAsteroid(200);
    }

    private static void LoadMap() {
        string[] input = File.ReadAllLines("input.txt");

        _width = input[0].Length;
        _height = input.Length;
        _map = new bool[_width, _height];
        
        for (int y = 0; y < _height; ++y) {
            string line = input[y];
            for (int x = 0; x < _width; ++x) {
                _map[x, y] = (line[x] == '#');
            }
        }
    }

    private static void FindAsteroid(int number) {
        List<Asteroid> asteroids = new List<Asteroid>();
        for (int y = 0; y < _height; ++y) {
            for (int x = 0; x < _width; ++x) {
                Point p = new Point(x, y);
                if (!_map[x, y] || STATION == p) continue;

                if (CanSee(p)) {
                    asteroids.Add(new Asteroid(p));
                }
            }
        }
        
        asteroids.Sort((a, b) => a.angle.CompareTo(b.angle));

        Console.WriteLine($"#{number} = {asteroids[number-1].position}");
    }

    private static bool CanSee(Point p) {
        Point d = p - STATION;
        int gcd = GCD(d.x, d.y);

        Point step = d / gcd;
        Point p3 = STATION;
        for (int i = 1; i < gcd; ++i) {
            p3 += step;
            if (_map[p3.x, p3.y]) return false;
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

    private static double GetAngle(Point p) => (Math.Atan2(-p.x, p.y) + Math.PI) % (2 * Math.PI);
}
