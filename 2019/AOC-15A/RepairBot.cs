using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class RepairBot {
    private enum Direction {
        North = 1,
        South = 2,
        West = 3,
        East = 4
    }

    private enum Tile {
        Wall = 0,
        Empty = 1,
        Goal = 2
    }

    private static int ManhattanDistance(Point a, Point b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    private static readonly Dictionary<Direction, Point> DIRECTION_TO_POINT = new Dictionary<Direction, Point> {
        { Direction.North, Point.up    },
        { Direction.South, Point.down  },
        { Direction.East,  Point.right },
        { Direction.West,  Point.left  }
    };

    private IntCode _intCode;

    private Point _position = Point.zero;
    private HashSet<Point> _unknown = new HashSet<Point>();
    private Queue<Direction> _moves = new Queue<Direction>();
    private Point _pendingPosition;
    private Point? _goal;

    private Dictionary<Point, Tile> _map = new Dictionary<Point, Tile>();
    
    public RepairBot() {
        _map[_position] = Tile.Empty;
        AddUnknownNeighbors();

        _intCode = new IntCode(File.ReadAllLines("input.txt")[0]);
        _intCode.Begin();

        while (_goal == null && _unknown.Count > 0) {
            BuildPathToNearestUnknown();
            ExecuteMoves();
        }

        if (_goal == null) {
            Console.WriteLine("Failed to find oxygen system, no where left to search!");
            return;
        }

        Console.WriteLine($"Oxygen system is {FindPath(Point.zero, _goal.Value).Count} steps away at {_goal.Value}.");
        DrawMap();
    }

    private void BuildPathToNearestUnknown() {
        Point target = _unknown.OrderBy(p => ManhattanDistance(p, _position)).First();

        Stack<Point> path = FindPath(_position, target);

        Point prev = _position;
        while (path.Count > 0) {
            Point next = path.Pop();
            _moves.Enqueue(DIRECTION_TO_POINT.First(p => p.Value == next - prev).Key);
            prev = next;
        }
    }

    private void ExecuteMoves() {
        Direction direction;

        while (_moves.Count > 1) {
            direction = _moves.Dequeue();
            _position += DIRECTION_TO_POINT[direction];
            _intCode.Input((long)direction);
        }

        direction = _moves.Dequeue();
        _pendingPosition = _position + DIRECTION_TO_POINT[direction];

        _intCode.OnOutput += HandleUnknownMove;
        _intCode.Input((long)direction);
        _intCode.OnOutput -= HandleUnknownMove;
    }

    private void HandleUnknownMove(long output) {
        Tile tile = (Tile)output;
        _map[_pendingPosition] = tile;

        _unknown.Remove(_pendingPosition);

        if (tile == Tile.Wall) return;

        _position = _pendingPosition;

        if (tile == Tile.Goal) {
            _goal = _position;
            return;
        }

        AddUnknownNeighbors();
    }

    private void AddUnknownNeighbors() {
        Point[] neighbors = new[] {
            _position + Point.up,
            _position + Point.right,
            _position + Point.down,
            _position + Point.left,
        };

        foreach (Point neighbor in neighbors.Where(n => !_map.ContainsKey(n))) {
            _unknown.Add(neighbor);
        }
    }

    private Stack<Point> FindPath(Point start, Point end) {
        int GetH(Point p) => ManhattanDistance(p, end);

        Debug.Assert(_map.ContainsKey(start), $"Pathfinding failed, invalid start {start}");

        HashSet<Point> openSet = new HashSet<Point> { start };
        Dictionary<Point, Point> parentMap = new Dictionary<Point, Point>();
        Dictionary<Point, int> gMap = new Dictionary<Point, int> { { start, 0 } };
        Dictionary<Point, int> fMap = new Dictionary<Point, int> { { start, GetH(start) } };

        int GetG(Point p) => gMap.ContainsKey(p) ? gMap[p] : int.MaxValue;
        int GetF(Point p) => fMap.ContainsKey(p) ? fMap[p] : int.MaxValue;

        while (openSet.Count > 0) {
            Point current = openSet.OrderBy(GetF).First();

            if (current == end) {
                Stack<Point> path = new Stack<Point>();
                while (parentMap.ContainsKey(current)) {
                    path.Push(current);
                    current = parentMap[current];
                }
                return path;
            }

            openSet.Remove(current);

            List<Point> neighbors = new List<Point> {
                current + Point.up,
                current + Point.right,
                current + Point.down,
                current + Point.left,
            };

            if (parentMap.ContainsKey(current)) {
                neighbors.Remove(parentMap[current]);
            }

            int gNext = GetG(current) + 1;
            foreach (Point neighbor in neighbors) {
                if (neighbor != end && (!_map.ContainsKey(neighbor) || _map[neighbor] == Tile.Wall)) continue;

                if (gNext < GetG(neighbor)) {
                    gMap[neighbor] = gNext;
                    fMap[neighbor] = gNext + GetH(neighbor);
                    parentMap[neighbor] = current;
                    openSet.Add(neighbor);
                }
            }
        }

        throw new Exception($"Failed to find path from {start} to {end}");
    }

    private void DrawMap() {
        Stack<Point> path = FindPath(Point.zero, _goal.Value);

        Point min = new Point(int.MaxValue, int.MaxValue);
        Point max = new Point(int.MinValue, int.MinValue);

        foreach (Point p in _map.Keys) {
            min.x = Math.Min(min.x, p.x);
            min.y = Math.Min(min.y, p.y);
            max.x = Math.Max(max.x, p.x);
            max.y = Math.Max(max.y, p.y);
        }

        for (int y = max.y; y >= min.y; --y) {
            for (int x = min.x; x <= max.x; ++x) {
                Point p = new Point(x, y);
                Console.BackgroundColor = ConsoleColor.Black;
                if (_map.ContainsKey(p)) {
                    if (path.Contains(p)) Console.BackgroundColor = ConsoleColor.DarkGreen;
                    if (p == Point.zero) Console.BackgroundColor = ConsoleColor.DarkRed;
                    switch (_map[p]) {
                        case Tile.Empty: Console.Write("-"); break;
                        case Tile.Wall:  Console.Write("#"); break;
                        case Tile.Goal:  Console.Write("O"); break;
                    }
                } else {
                    Console.Write(" ");
                }
            }
            Console.WriteLine(string.Empty);
        }
        Console.BackgroundColor = ConsoleColor.Black;
    }
}
