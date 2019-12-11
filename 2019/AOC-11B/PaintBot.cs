using System;
using System.Collections.Generic;
using System.IO;

public class PaintBot {
    private enum State {
        Paint,
        Move
    }

    private IntCode _intCode;
    private State _state = State.Paint;
    private Point _pos = Point.zero;
    private Point _dir = Point.up;

    private HashSet<Point> _whiteTiles = new HashSet<Point> { Point.zero };

    public PaintBot() {
        _intCode = new IntCode(File.ReadAllLines("input.txt")[0]);
        _intCode.OnOutput += HandleOnOutput;
        _intCode.Begin();
    }

    public bool Update() {
        if (_intCode.state == IntCode.State.Waiting) {
            _intCode.Input(_whiteTiles.Contains(_pos) ? 1 : 0);
        }

        if (_intCode.state == IntCode.State.Complete) {
            PrintResults();
        }

        return _intCode.state != IntCode.State.Complete;
    }

    private void HandleOnOutput(long output) {
        switch (_state) {
            case State.Paint:
                if (output == 1) {
                    _whiteTiles.Add(_pos);
                } else {
                    _whiteTiles.Remove(_pos);
                }
                _state = State.Move;
                break;
            case State.Move:
                if (output == 0) {
                    _dir = new Point(-_dir.y, _dir.x);
                } else {
                    _dir = new Point(_dir.y, -_dir.x);
                }
                _pos += _dir;
                _state = State.Paint;
                break;
        }
    }

    private void PrintResults() {
        Point min = new Point(int.MaxValue, int.MaxValue);
        Point max = new Point(int.MinValue, int.MinValue);

        foreach (Point p in _whiteTiles) {
            min.x = Math.Min(min.x, p.x);
            min.y = Math.Min(min.y, p.y);
            max.x = Math.Max(max.x, p.x);
            max.y = Math.Max(max.y, p.y);
        }

        for (int y = max.y; y >= min.y; --y) {
            for (int x = min.x; x <= max.x; ++x) {
                Console.BackgroundColor = (_whiteTiles.Contains(new Point(x, y)) ? ConsoleColor.White : ConsoleColor.Black);
                Console.Write("  ");
            }
            Console.WriteLine(string.Empty);
        }

        Console.BackgroundColor = ConsoleColor.Black;
    }
}
