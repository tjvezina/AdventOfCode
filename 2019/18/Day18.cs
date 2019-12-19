using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day18 : Challenge {
        public enum Tile { Empty, Wall }

        public struct Key {
            public char id;
            public Point pos;
        }

        private Tile[,] _map;
        private int _width;
        private int _height;

        private Point _startPos;
        // Map key positions to door positions (if any)
        private List<Key> _keyList = new List<Key>();
        // Map door positions to whether or not they are unlocked
        private Dictionary<Point, Key> _doorMap = new Dictionary<Point, Key>();

        private void Init(string[] input) {
            SpaceUtil.system = CoordSystem.YDown;

            _width = input[0].Length;
            _height = input.Length;
            _map = new Tile[_width, _height];

            Dictionary<char, Point> keysAndDoors = new Dictionary<char, Point>();

            for (int y = 0; y < _height; ++y) {
                string line = input[y];
                for (int x = 0; x < _width; ++x) {
                    char c = line[x];
                    if (c == '#') {
                        _map[x, y] = Tile.Wall;
                        continue;
                    }
                    _map[x, y] = Tile.Empty;

                    if (c == '@') {
                        _startPos = new Point(x, y);
                        continue;
                    }
                    
                    keysAndDoors[c] = new Point(x, y);
                }
            }

            char keyID = 'a';
            while (keysAndDoors.ContainsKey(keyID)) {
                Key key = new Key { id = keyID, pos = keysAndDoors[keyID] };
                _keyList.Add(key);

                char doorID = (char)(keyID + ('A' - 'a')); // To uppercase
                if (keysAndDoors.ContainsKey(doorID)) {
                    _doorMap[keysAndDoors[doorID]] = key;
                }

                ++keyID;
            }
        }

        protected override string SolvePart1() {
            int dist = DistanceToCollectKeys(_startPos, _keyList);
            return $"Shortest distance to collect all keys: {dist}";
        }
        
        protected override string SolvePart2() => null;

        private int DistanceToCollectKeys(Point start, List<Key> keys, Dictionary<string, int> cache = null) {
            bool IsValid(Point p, Point end) {
                return _map[p.x, p.y] != Tile.Wall && // Not a wall
                    (p == end || !keys.Any(k => k.pos == p)) && // Not a key, except target
                    (!_doorMap.ContainsKey(p) || !keys.Contains(_doorMap[p])); // Not a door, unless unlocked
            }

            if (keys.Count == 0) return 0;

            string cacheKey = $"{start}{keys.Select(k => $"{k.id}").Aggregate((a, b) => $"{a}{b}")}";
            cache = cache ?? new Dictionary<string, int>();
            if (!cache.ContainsKey(cacheKey)) {
                int minDist = int.MaxValue;
                foreach (Key key in keys) {
                    if (Pathfinder.TryFindPath(start, key.pos, (p) => IsValid(p, key.pos), out Stack<Point> path)) {
                        List<Key> keysLeft = new List<Key>(keys);
                        keysLeft.Remove(key);
                        minDist = Math.Min(minDist, path.Count + DistanceToCollectKeys(key.pos, keysLeft, cache));
                    }
                }

                cache[cacheKey] = minDist;
            }

            return cache[cacheKey];
        }
    }
}
