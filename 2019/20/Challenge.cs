using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day20
{
     public class Challenge : BaseChallenge
     {
        public struct Node
        {
            public Point pos;
            public int depth;

            public Node(Point pos, int depth = 0)
            {
                this.pos = pos;
                this.depth = depth;
            }

            public bool Equals(Node n) => n.pos == pos && n.depth == depth;
            public override bool Equals(object obj) => obj is Node && Equals((Node)obj);
            public override int GetHashCode() => (pos.x << 16) + (pos.y << 8) + depth;
            public static bool operator==(Node a, Node b) => a.Equals(b);
            public static bool operator!=(Node a, Node b) => !(a == b);
        }

        public override CoordSystem? coordSystem => CoordSystem.YDown;

        private CharMap _map;

        private Point _start;
        private Point _end;
        private PairMap<Point> _portalMap = new PairMap<Point>();

        private bool IsPortalID(char c) => c >= 'A' && c <= 'Z';

        public Challenge()
        {
            _map = new CharMap(inputList.ToArray());

            Dictionary<string, (Point a, Point b)> portals = new Dictionary<string, (Point, Point)>();

            void CheckPortal(char c, Point pos, Point dir)
            {
                char c2 = _map.GetCharOrDefault(pos + dir);
                if (!IsPortalID(c2)) return;

                Point portal = _map.GetCharOrDefault(pos - dir) == '.' ? pos - dir : pos + dir*2;
                string id = $"{c}{c2}";

                switch (id)
                {
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

            foreach ((int x, int y, char c) in _map)
            {
                if (IsPortalID(c))
                {
                    CheckPortal(c, new Point(x, y), Direction.Right);
                    CheckPortal(c, new Point(x, y), Direction.Down);
                }
            }

            foreach ((Point a, Point b) in portals.Values) _portalMap.Add(a, b);
        }

        private bool IsOuterPortal(Point p)
        {
            return p.x == 2 || p.x == _map.width - 3 ||
                   p.y == 2 || p.y == _map.height - 3;
        }

        public override string part1ExpectedAnswer => "484";
        public override (string message, object answer) SolvePart1()
        {
            return ("Shortest path: {0} steps", FindShortestPath());
        }
        
        public override string part2ExpectedAnswer => "5754";
        public override (string message, object answer) SolvePart2()
        {
            return ("Shortest path: {0} steps", FindShortestPathRecursive());
        }

        private int FindShortestPath()
        {
            int GetH(Point p, Point end) => 0; // No heuristic (Dijsktra's/BFS)

            bool IsValid(Point p) => ".se@$".Contains(_map.GetCharOrDefault(p));

            HashSet<Point> GetNeighbors(Point p)
            {
                HashSet<Point> neighbors = new HashSet<Point>
                {
                    p + new Point( 1, 0), p + new Point(0, 1),
                    p + new Point(-1, 0), p + new Point(0,-1)
                };
                if ("@$".Contains(_map[p])) neighbors.Add(_portalMap[p]);
                return neighbors;
            }

            return Pathfinder.FindPath(_start, _end, IsValid, GetH, GetNeighbors).Count;
        }

        private int FindShortestPathRecursive()
        {
            Queue<(Point pos, int depth, int dist)> queue = new Queue<(Point, int, int)>();
            queue.Enqueue((_start, 0, 0));

            HashSet<Node> visited = new HashSet<Node> { new Node(_start) };

            while (queue.Count > 0)
            {
                (Point pos, int depth, int dist) = queue.Dequeue();
                char c = _map[pos];

                foreach ((int dx, int dy) in new[] { (1, 0), (0, 1), (-1, 0), (0, -1) })
                {
                    Point next = new Point(pos.x + dx, pos.y + dy);
                    char nextChar = _map[next];
                    int nextDepth = depth;

                    if (nextChar == 'e' && depth == 0)
                    {
                        return dist + 1;
                    }

                    if ((nextChar == '#') || // Walls
                        (depth == 0 && nextChar == '@') || // Outer portals on top level
                        (depth == 60 && nextChar == '$') || // Inner portals on bottom level
                        (depth > 0 && (nextChar == 's' || nextChar == 'e')) // Start/end on inner levels
                    )
                    {
                        continue;
                    }

                    if (IsPortalID(nextChar))
                    {
                        if (c == 's') continue;

                        nextDepth += (c == '$' ? 1 : -1);
                        next = _portalMap[pos];
                    }

                    if (visited.Add(new Node(next, nextDepth)))
                    {
                        queue.Enqueue((next, nextDepth, dist + 1));
                    }
                }
            }

            throw new Exception("Failed to reach end of maze");
        }
    }
}
