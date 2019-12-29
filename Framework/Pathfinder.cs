using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode {
    public class Pathfinder {
        public static Stack<Point> FindPathInGrid(Point start, Point end, Func<Point, bool> isValid) {
            return FindPath(start, end, isValid, GridHeuristic, GetGridNeighbors);
        }
        
        public static Stack<T> FindPath<T>(T start, T end, Func<T, bool> isValid,
            Func<T, T, int> getH, Func<T, HashSet<T>> getNeighbors
        ) {
            
            if (!TryFindPath(start, end, isValid, getH, getNeighbors, out Stack<T> path)) {
                throw new Exception($"Failed to find path from {start} to {end}");
            }
            return path;
        }

        public static bool TryFindPathInGrid(Point start, Point end, Func<Point, bool> isValid, out Stack<Point> path) {
            return TryFindPath(start, end, isValid, GridHeuristic, GetGridNeighbors, out path);
        }

        public static bool TryFindPath<T>(
            T start, T end,
            Func<T, bool> isValid,
            Func<T, T, int> getH,
            Func<T, HashSet<T>> getNeighbors,
            out Stack<T> path
        ) {
            HashSet<T> openSet = new HashSet<T> { start };
            Dictionary<T, T> parentMap = new Dictionary<T, T>();

            Dictionary<T, int> gMap = new Dictionary<T, int> { { start, 0 } };
            Dictionary<T, int> fMap = new Dictionary<T, int> { { start, getH(start, end) } };

            int GetG(T p) => gMap.ContainsKey(p) ? gMap[p] : int.MaxValue;
            int GetF(T p) => fMap.ContainsKey(p) ? fMap[p] : int.MaxValue;

            while (openSet.Count > 0) {
                T current = openSet.OrderBy(GetF).First();

                if (current.Equals(end)) {
                    path = new Stack<T>();
                    while (parentMap.ContainsKey(current)) {
                        path.Push(current);
                        current = parentMap[current];
                    }
                    return true;
                }

                openSet.Remove(current);

                HashSet<T> neighbors = getNeighbors(current);

                if (parentMap.ContainsKey(current)) {
                    neighbors.Remove(parentMap[current]);
                }

                int gNext = GetG(current) + 1;
                foreach (T neighbor in neighbors) {
                    if (isValid(neighbor) && gNext < GetG(neighbor)) {
                        gMap[neighbor] = gNext;
                        fMap[neighbor] = gNext + getH(neighbor, end);
                        parentMap[neighbor] = current;
                        openSet.Add(neighbor);
                    }
                }
            }

            path = null;
            return false;
        }

        private static HashSet<Point> GetGridNeighbors(Point p) => new HashSet<Point> {
            p + new Point( 1, 0), p + new Point(0, 1),
            p + new Point(-1, 0), p + new Point(0,-1)
        };

        private static int GridHeuristic(Point p, Point end) => Point.TaxiDist(p, end);
    }
}
