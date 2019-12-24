using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day20 {
     public class Challenge : BaseChallenge {
        public struct Node {
            public Point pos;
            public int depth;

            public Node(Point pos, int depth = 0) {
                this.pos = pos;
                this.depth = depth;
            }

            public bool Equals(Node n) => n.pos == pos && n.depth == depth;
            public override bool Equals(object obj) => obj is Node && Equals((Node)obj);
            public override int GetHashCode() => pos.GetHashCode() << depth;
            public static bool operator==(Node a, Node b) => a.Equals(b);
            public static bool operator!=(Node a, Node b) => !(a == b);
        }

        private CharMap _map;

        private Point _start;
        private Point _end;
        private PairMap<Point> _portalMap = new PairMap<Point>();

        private bool IsPortalID(char c) => c >= 'A' && c <= 'Z';

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YDown;
            _map = new CharMap(inputSet);

            Dictionary<string, (Point a, Point b)> portals = new Dictionary<string, (Point, Point)>();

            void CheckPortal(char c, Point pos, Point dir) {
                char c2 = _map.GetCharOrDefault(pos + dir);
                if (!IsPortalID(c2)) return;

                Point portal = _map.GetCharOrDefault(pos - dir) == '.' ? pos - dir : pos + dir*2;
                string id = $"{c}{c2}";

                switch (id) {
                    case "AA":
                        _map[portal] = 's';
                        _start = portal;
                        break;
                    case "ZZ":
                        _map[portal] = 'e';
                        _end = portal;
                        break;
                    default:
                        _map[portal] = IsOuterPortal(portal) ? '@' : '$';
                        if (!portals.ContainsKey(id)) portals[id] = (portal, Point.zero);
                        else portals[id] = (portals[id].a, portal);
                        break;
                }
            }

            foreach ((int x, int y, char c) in _map.Enumerate()) {
                if (IsPortalID(c)) {
                    CheckPortal(c, new Point(x, y), Direction.Right);
                    CheckPortal(c, new Point(x, y), Direction.Down);
                }
            }

            foreach ((Point a, Point b) in portals.Values) _portalMap.Add(a, b);
        }

        private bool IsOuterPortal(Point p) {
            return p.x == 2 || p.x == _map.width - 3 ||
                   p.y == 2 || p.y == _map.height - 3;
        }

        public override string part1Answer => "484";
        public override (string, object) SolvePart1() => ("Shortest path: {0} steps", FindShortestPath());
        
        public override string part2Answer => "5754";
        public override (string, object) SolvePart2() => ("Shortest path: {0} steps", FindShortestPathRecursive());

        private int FindShortestPath() {
            int GetH(Point p, Point end) => 0; // No heuristic (Dijsktra's/BFS)

            bool IsValid(Point p) => ".se@$".Contains(_map.GetCharOrDefault(p));

            HashSet<Point> GetNeighbors(Point p) {
                HashSet<Point> neighbors = new HashSet<Point> {
                    p + new Point( 1, 0), p + new Point(0, 1),
                    p + new Point(-1, 0), p + new Point(0,-1)
                };
                if ("@$".Contains(_map[p])) neighbors.Add(_portalMap[p]);
                return neighbors;
            }

            return Pathfinder.FindPath(_start, _end, IsValid, GetH, GetNeighbors).Count;
        }

        private int FindShortestPathRecursive() {
            int GetH(Node n, Node end) => n.depth; // No heuristic (Dijkstra's/BFS)

            bool IsValid(Node node) {
                switch (_map.GetCharOrDefault(node.pos)) {
                    case '.':
                    case '$':
                        return true;
                    case 's':
                    case 'e':
                        return node.depth == 0;
                    case '@':
                        return node.depth > 0;
                }
                
                return false;
            }

            HashSet<Node> GetNeighbors(Node node) {
                HashSet<Node> neighbors = new HashSet<Node> {
                    new Node(node.pos + new Point( 1, 0), node.depth),
                    new Node(node.pos + new Point( 0, 1), node.depth),
                    new Node(node.pos + new Point(-1, 0), node.depth),
                    new Node(node.pos + new Point( 0,-1), node.depth)
                };

                if (_map[node.pos] == '$') neighbors.Add(new Node(_portalMap[node.pos], node.depth + 1));
                if (_map[node.pos] == '@') neighbors.Add(new Node(_portalMap[node.pos], node.depth - 1));

                return neighbors;
            }

            return Pathfinder.FindPath(new Node(_start), new Node(_end), IsValid, GetH, GetNeighbors).Count;
        }
    }
}
