using System;
using System.Collections.Generic;
using System.IO;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day13
{
    public class Arcade
    {
        public enum Tile
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            Paddle = 3,
            Ball = 4
        }

        public enum State
        {
            PosX,
            PosY,
            Type
        }

        private static readonly Point ScorePos = new Point(-1, 0);

        public State state { get; private set; } = State.PosX;

        private IntCode _intCode;
        private Tile[,] _screen = new Tile[0, 0];
        private Point _nextPos = Point.zero;

        public int blockCount { get; private set; }

        public int score { get; private set; }
        private int _paddleX;
        private int _ballX;

        public Arcade(string intCodeMemory, bool insertQuarters = false)
        {
            Dictionary<int, long> substitutions = new Dictionary<int, long>();
            if (insertQuarters)
            {
                substitutions[0] = 2;
            }

            _intCode = new IntCode(intCodeMemory, substitutions);
            _intCode.OnOutput += HandleOnOutput;
            _intCode.Begin();

            while (_intCode.state == IntCode.State.Waiting)
            {
                int delta = _ballX - _paddleX;
                delta = delta == 0 ? 0 : delta / Math.Abs(delta); // -1, 0, 1
                _intCode.Input(delta);
            }

            if (insertQuarters)
            {
                Console.WriteLine("Final score: " + score);
            } else
            {
                RunDiagnosticCheck();
            }
        }

        private void RunDiagnosticCheck()
        {
            for (int y = 0; y < _screen.GetLength(1); y++)
            {
                for (int x = 0; x < _screen.GetLength(0); x++)
                {
                    blockCount += (_screen[x, y] == Tile.Block ? 1 : 0);
                }
            }
        }

        private void HandleOnOutput(long output)
        {
            if (state == State.PosX)
            {
                _nextPos.x = (int)output;
            }
            else if (state == State.PosY)
            {
                _nextPos.y = (int)output;
            }
            else if (state == State.Type)
            {
                if (_nextPos == ScorePos)
                {
                    score = (int)output;
                } else
                {
                    ResizeScreen();
                    Tile tile = (Tile)output;
                    _screen[_nextPos.x, _nextPos.y] = tile;
                    if (tile == Tile.Ball) _ballX = _nextPos.x;
                    if (tile == Tile.Paddle) _paddleX = _nextPos.x;
                }
            }

            state = state + 1;
            if (!Enum.IsDefined(typeof(State), state)) state = (State)0;
        }

        private void ResizeScreen()
        {
            if (_nextPos.x < _screen.GetLength(0) && _nextPos.y < _screen.GetLength(1)) return;

            int width = Math.Max(_nextPos.x + 1, _screen.GetLength(0));
            int height = Math.Max(_nextPos.y + 1, _screen.GetLength(1));

            Tile[,] newScreen = new Tile[width, height];

            for (int y = 0; y < _screen.GetLength(1); y++)
            {
                for (int x = 0; x < _screen.GetLength(0); x++)
                {
                    newScreen[x, y] = _screen[x, y];
                }
            }

            _screen = newScreen;
        }
    }
}
