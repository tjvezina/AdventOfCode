using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode {
    public class Pathfinder {
        public static Stack<Point> FindPath(Point start, Point end, Func<Point, bool> isValid) {
            if (!TryFindPath(start, end, isValid, out Stack<Point> path)) {
                throw new Exception($"Failed to find path from {start} to {end}");
            }
            return path;
        }

        public static bool TryFindPath(Point start, Point end, Func<Point, bool> isValid, out Stack<Point> path) {
            int GetH(Point p) => Point.TaxiDist(p, end);

            HashSet<Point> openSet = new HashSet<Point> { start };
            Dictionary<Point, Point> parentMap = new Dictionary<Point, Point>();

            Dictionary<Point, int> gMap = new Dictionary<Point, int> { { start, 0 } };
            Dictionary<Point, int> fMap = new Dictionary<Point, int> { { start, GetH(start) } };

            int GetG(Point p) => gMap.ContainsKey(p) ? gMap[p] : int.MaxValue;
            int GetF(Point p) => fMap.ContainsKey(p) ? fMap[p] : int.MaxValue;

            while (openSet.Count > 0) {
                Point current = openSet.OrderBy(GetF).First();

                if (current == end) {
                    path = new Stack<Point>();
                    while (parentMap.ContainsKey(current)) {
                        path.Push(current);
                        current = parentMap[current];
                    }
                    return true;
                }

                openSet.Remove(current);

                List<Point> neighbors = EnumUtil.GetValues<Direction>().Select(d => current + d).ToList();

                if (parentMap.ContainsKey(current)) {
                    neighbors.Remove(parentMap[current]);
                }

                int gNext = GetG(current) + 1;
                foreach (Point neighbor in neighbors) {
                    if (isValid(neighbor) && gNext < GetG(neighbor)) {
                        gMap[neighbor] = gNext;
                        fMap[neighbor] = gNext + GetH(neighbor);
                        parentMap[neighbor] = current;
                        openSet.Add(neighbor);
                    }
                }
            }

            path = null;
            return false;
        }
    }
}
