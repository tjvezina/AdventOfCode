using System;
using System.Collections.Generic;
using System.IO;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day11
{
    public class PaintBot
    {
        private enum State
        {
            Paint,
            Move
        }

        private IntCode _intCode;
        private State _state;
        private Point _pos;
        private Point _dir;

        private HashSet<Point> _whiteTiles = new HashSet<Point>();
        private HashSet<Point> _paintedTiles = new HashSet<Point>();

        public int paintedCount => _paintedTiles.Count;

        public PaintBot(string intCodeMemory)
        {
            _intCode = new IntCode(intCodeMemory);
            _intCode.OnOutput += HandleOnOutput;
        }

        public void Run(bool firstTileIsWhite)
        {
            _whiteTiles.Clear();
            _paintedTiles.Clear();
            _state = State.Paint;
            _pos = Point.zero;
            _dir = Direction.Up;
            if (firstTileIsWhite)
            {
                _whiteTiles.Add(_pos);
            }

            _intCode.Reset();
            _intCode.Begin();

            while (Update());
        }

        private bool Update()
        {
            if (_intCode.state == IntCode.State.Waiting)
            {
                _intCode.Input(_whiteTiles.Contains(_pos) ? 1 : 0);
            }

            return _intCode.state != IntCode.State.Complete;
        }

        private void HandleOnOutput(long output)
        {
            switch (_state)
            {
                case State.Paint:
                    _paintedTiles.Add(_pos);
                    if (output == 1)
                    {
                        _whiteTiles.Add(_pos);
                    } else
                    {
                        _whiteTiles.Remove(_pos);
                    }
                    _state = State.Move;
                    break;
                case State.Move:
                    if (output == 0)
                    {
                        _dir = _dir.RotateCCW();
                    } else
                    {
                        _dir = _dir.RotateCW();
                    }
                    _pos += _dir;
                    _state = State.Paint;
                    break;
            }
        }

        public bool[,] GetImage()
        {
            Point min = new Point(int.MaxValue, int.MaxValue);
            Point max = new Point(int.MinValue, int.MinValue);

            foreach (Point p in _whiteTiles)
            {
                min.x = Math.Min(min.x, p.x);
                min.y = Math.Min(min.y, p.y);
                max.x = Math.Max(max.x, p.x);
                max.y = Math.Max(max.y, p.y);
            }

            bool[,] pixels = new bool[max.x - min.x + 1, max.y - min.y + 1];

            for (int y = max.y; y >= min.y; y--)
            {
                for (int x = min.x; x <= max.x; x++)
                {
                    int xPixel = x - min.x;
                    int yPixel = max.y - y;
                    pixels[xPixel, yPixel] = _whiteTiles.Contains(new Point(x, y));
                }
            }

            return pixels;
        }
    }
}
