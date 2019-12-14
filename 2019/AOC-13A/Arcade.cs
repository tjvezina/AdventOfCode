using System;
using System.Collections.Generic;
using System.IO;

public class Arcade {
    public enum Tile {
        Empty = 0,
        Wall = 1,
        Block = 2,
        Paddle = 3,
        Ball = 4
    }

    public enum State {
        PosX,
        PosY,
        Type
    }

    public State state { get; private set; } = State.PosX;

    private IntCode _intCode;
    private Tile[,] _screen;
    private Point _nextPos = Point.zero;

    public Arcade() {
        _screen = new Tile[0,0];

        _intCode = new IntCode(File.ReadAllLines("input.txt")[0]);
        _intCode.OnOutput += HandleOnOutput;
        _intCode.Begin();

        int blocks = 0;
        for (int y = 0; y < _screen.GetLength(1); ++y) {
            for (int x = 0; x < _screen.GetLength(0); ++x) {
                blocks += (_screen[x, y] == Tile.Block ? 1 : 0);
            }
        }

        Console.WriteLine($"{blocks} blocks on screen");
    }

    private void HandleOnOutput(long output) {
        if (state == State.PosX) {
            _nextPos.x = (int)output;
            state = State.PosY;
        }
        else if (state == State.PosY) {
            _nextPos.y = (int)output;
            state = State.Type;
        }
        else if (state == State.Type) {
            ResizeScreen();
            _screen[_nextPos.x, _nextPos.y] = (Tile)output;
            state = State.PosX;
        }
    }

    private void ResizeScreen() {
        if (_nextPos.x < _screen.GetLength(0) && _nextPos.y < _screen.GetLength(1)) return;

        int width = Math.Max(_nextPos.x + 1, _screen.GetLength(0));
        int height = Math.Max(_nextPos.y + 1, _screen.GetLength(1));

        Tile[,] newScreen = new Tile[width, height];

        for (int y = 0; y < _screen.GetLength(1); ++y) {
            for (int x = 0; x < _screen.GetLength(0); ++x) {
                newScreen[x, y] = _screen[x, y];
            }
        }

        _screen = newScreen;
    }
}
