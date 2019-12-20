using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day18 : Challenge {
        private class NodeData : Dictionary<char, (int, string)> { }
        private class RouteData : Dictionary<(char, string), int> { }

        private bool IsWall(char c) => c == '#';
        private bool IsStart(char c) => "@1234".Contains(c);
        private bool IsKey(char c) => c >= 'a' && c <= 'z';
        private bool IsDoor(char c) => c >= 'A' && c <= 'Z';

        private char[,] _map;
        private int _width;
        private int _height;

        private Dictionary<char, NodeData> _nodeMap = new Dictionary<char, NodeData>();

        private void Init(string[] data) {
            SpaceUtil.system = CoordSystem.YDown;

            _height = data.Length;
            _width = data[0].Length;
            _map = new char[_width, _height];

            for (int y = 0; y < _height; ++y) {
                string line = data[y];
                for (int x = 0; x < _width; ++x) {
                    _map[x, y] = line[x];
                }
            }
        }

        protected override string SolvePart1() {
            BuildGraph();
            return $"Shortest path to all keys: {FindShortestPath()}";
        }

        protected override string SolvePart2() => null;

        private void BuildGraph() {
            foreach ((int x, int y, char c) in _map.GetElements()) {
                if (IsKey(c) || IsStart(c)) {
                    _nodeMap[c] = BuildNodeData(new Point(x, y));
                }
            }
        }

        private NodeData BuildNodeData(Point start) {
            NodeData data = new NodeData();
            HashSet<Point> visited = new HashSet<Point>();
            Queue<(Point, int, string)> queue = new Queue<(Point, int, string)>();
            queue.Enqueue((start, 0, string.Empty));

            while (queue.Count > 0) {
                (Point pos, int dist, string route) = queue.Dequeue();
                char c = _map[pos.x, pos.y];
                if (dist > 0 && (IsKey(c) || IsDoor(c))) {
                    data[c] = (dist, route);
                    route += c;
                }
                visited.Add(pos);

                Point[] neighbors = new[] {
                    pos + new Point(0,  1), pos + new Point( 1, 0),
                    pos + new Point(0, -1), pos + new Point(-1, 0)
                };

                foreach (Point neighbor in neighbors) {
                    if (!IsWall(_map[neighbor.x, neighbor.y]) && !visited.Contains(neighbor)) {
                        queue.Enqueue((neighbor, dist + 1, route));
                    }
                }
            }

            return data;
        }

        private int FindShortestPath() {
            IEnumerable<char> allKeys = _nodeMap.Keys.Except(new[] { '@' });

            RouteData data = new RouteData { { ('@', ""), 0 } };
            for (int i = 0; i < allKeys.Count(); ++i) {
                RouteData nextData = new RouteData();
                foreach (((char start, string keys), int dist) in data) {
                    foreach (char end in allKeys.Where(k => !keys.Contains(k))) {
                        (int distToEnd, string route) = _nodeMap[start][end];
                        if (route.All(c => keys.Contains(char.ToLower(c)))) {
                            int newDist = dist + distToEnd;
                            (char, string) nextNode = (end, new string(keys.Append(end).OrderBy(k => k).ToArray()));
                            if (!nextData.ContainsKey(nextNode) || newDist < nextData[nextNode]) {
                                nextData[nextNode] = newDist;
                            }
                        }
                    }
                }
                data = nextData;
            }

            return data.Values.OrderBy(d => d).First();
        }
    }
}
