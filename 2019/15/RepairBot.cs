using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day15
{
    public class RepairBot
    {
        private enum Tile
        {
            Wall = 0,
            Empty = 1,
            Goal = 2
        }

        private static readonly Dictionary<Direction, long> InputMap = new Dictionary<Direction, long>
        {
            { Direction.Up,    1 },
            { Direction.Down,  2 },
            { Direction.Right, 3 },
            { Direction.Left,  4 }
        };

        private IntCode _intCode;

        private Point _position = Point.zero;
        private HashSet<Point> _unknown = new HashSet<Point>();
        private Queue<Direction> _moves = new Queue<Direction>();
        private Point _pendingPosition;

        public Point? goalPos { get; private set; }
        public int goalDist { get; private set; }
        public int timeToFill { get; private set; }

        private Dictionary<Point, Tile> _map = new Dictionary<Point, Tile>();
        
        public RepairBot(string intCodeMemory)
        {
            _map[_position] = Tile.Empty;
            AddUnknownNeighbors();

            _intCode = new IntCode(intCodeMemory);
            _intCode.Begin();

            while (_unknown.Count > 0)
            {
                BuildPathToNearestUnknown();
                ExecuteMoves();
            }

            if (goalPos == null)
            {
                Console.WriteLine("Failed to find oxygen system, no where left to search!");
                return;
            }

            goalDist = FindPath(Point.zero, goalPos.Value).Count;
            timeToFill = _map
                .Where(p => p.Value == Tile.Empty)
                .Select(p => FindPath(goalPos.Value, p.Key).Count)
                .OrderBy(c => c)
                .Last();

            DrawMap();
        }

        private void BuildPathToNearestUnknown()
        {
            Point target = _unknown.OrderBy(p => Point.TaxiDist(p, _position)).First();

            Stack<Point> path = FindPath(_position, target);

            Point prev = _position;
            while (path.Count > 0)
            {
                Point next = path.Pop();
                _moves.Enqueue(next - prev);
                prev = next;
            }
        }

        private void ExecuteMoves()
        {
            Direction direction;

            while (_moves.Count > 1)
            {
                direction = _moves.Dequeue();
                _position += direction;
                _intCode.Input(InputMap[direction]);
            }

            direction = _moves.Dequeue();
            _pendingPosition = _position + direction;

            _intCode.OnOutput += HandleUnknownMove;
            _intCode.Input(InputMap[direction]);
            _intCode.OnOutput -= HandleUnknownMove;
        }

        private void HandleUnknownMove(long output)
        {
            Tile tile = (Tile)output;
            _map[_pendingPosition] = tile;

            _unknown.Remove(_pendingPosition);

            if (tile == Tile.Wall) return;

            _position = _pendingPosition;

            if (tile == Tile.Goal)
            {
                goalPos = _position;
                return;
            }

            AddUnknownNeighbors();
        }

        private void AddUnknownNeighbors()
        {
            Point[] neighbors = EnumUtil.GetValues<Direction>().Select(d => _position + d).ToArray();

            foreach (Point neighbor in neighbors.Where(n => !_map.ContainsKey(n)))
            {
                _unknown.Add(neighbor);
            }
        }

        private Stack<Point> FindPath(Point start, Point end)
        {
            bool IsValid(Point p) => (_map.ContainsKey(p) && _map[p] != Tile.Wall) || (p == end && !_map.ContainsKey(p));
            return Pathfinder.FindPathInGrid(start, end, IsValid);
        }

        private void DrawMap()
        {
            Stack<Point> path = FindPath(Point.zero, goalPos.Value);

            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);

            foreach (Point p in _map.Keys)
            {
                min.x = Math.Min(min.x, p.x);
                min.y = Math.Min(min.y, p.y);
                max.x = Math.Max(max.x, p.x);
                max.y = Math.Max(max.y, p.y);
            }

            ConsoleColor prevBackground = Console.BackgroundColor;
            for (int y = max.y; y >= min.y; y--)
            {
                for (int x = min.x; x <= max.x; x++)
                {
                    Point p = new Point(x, y);
                    ConsoleColor color = ConsoleColor.DarkGray;
                    if (_map.ContainsKey(p))
                    {
                        switch (_map[p])
                        {
                            case Tile.Goal:
                                color = ConsoleColor.Green;
                                break;
                            case Tile.Empty:
                                if (p == Point.zero) color = ConsoleColor.DarkRed;
                                else if (path.Contains(p)) color = ConsoleColor.DarkYellow;
                                else color = ConsoleColor.Black;
                                break;
                        }
                    }
                    Console.BackgroundColor = color;
                    Console.Write("  ");
                }
                Console.BackgroundColor = prevBackground;
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
