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

    private HashSet<Point> _whiteTiles = new HashSet<Point>();
    private HashSet<Point> _paintedTiles = new HashSet<Point>();

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
                _paintedTiles.Add(_pos);
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
        Console.WriteLine($"Painted tiles: {_paintedTiles.Count}");
    }
}
