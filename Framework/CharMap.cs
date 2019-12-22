using System;
using System.Collections.Generic;

namespace AdventOfCode {
    public class CharMap {
        private char[,] _map;
        public int width { get; }
        public int height { get; }

        public char this[int x, int y] {
            get => _map[x, y];
            set => _map[x, y] = value;
        }

        public char this[Point p] {
            get => _map[p.x, p.y];
            set => _map[p.x, p.y] = value;
        }

        public CharMap(int width, int height) {
            this.width = width;
            this.height = height;
            _map = new char[width, height];
        }

        public CharMap(string[] input) {
            height = input.Length;
            width = input[0].Length;
            _map = new char[width, height];

            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    _map[x, y] = input[y][x];
                }
            }
        }

        public char GetCharOrDefault(Point p) => GetCharOrDefault(p.x, p.y);
        public char GetCharOrDefault(int x, int y) {
            if (x >= 0 && y >= 0 && x < width && y < height) return _map[x, y];
            return default;
        }

        public IEnumerable<char> GetElements() {
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    yield return _map[x, y];
                }
            }
        }

        public IEnumerable<(int x, int y, char c)> Enumerate() {
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    yield return (x, y, _map[x, y]);
                }
            }
        }

        public void Draw() {
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    Console.Write(_map[x, y]);
                }
                Console.WriteLine();
            }
        }
    }
}
