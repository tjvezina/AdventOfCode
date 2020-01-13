using System;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day08 {
    public abstract class Operation {
        protected class FillOperation : Operation {
            private int _width;
            private int _height;

            public FillOperation(string data) {
                Match match = Regex.Match(data, @"(\d+)x(\d+)");
                _width = int.Parse(match.Groups[1].Value);
                _height = int.Parse(match.Groups[2].Value);
            }

            public override void Execute(bool[,] board) {
                for (int y = 0; y < _height; y++) {
                    for (int x = 0; x < _width; x++) {
                        board[x, y] = true;
                    }
                }
            }
        }

        protected class RotateOperation : Operation {
            private int _lineAxis;
            private int _line;
            private int _offset;

            public RotateOperation(string data) {
                Match match = Regex.Match(data, @"(x|y)=(\d+) by (\d+)");
                _lineAxis = (match.Groups[1].Value == "x" ? 0 : 1);
                _line = int.Parse(match.Groups[2].Value);
                _offset = int.Parse(match.Groups[3].Value);
            }

            public override void Execute(bool[,] board) {
                int stepAxis = (_lineAxis == 0 ? 1 : 0);
                Point p = Point.zero;
                p[_lineAxis] = _line;

                bool[] line = new bool[board.GetLength(stepAxis)];
                for (int i = 0; i < line.Length; i++) {
                    p[stepAxis] = i;
                    line[i] = board[p.x, p.y];
                }
                for (int i = 0; i < line.Length; i++) {
                    p[stepAxis] = (i + _offset) % line.Length;
                    board[p.x, p.y] = line[i];
                }
            }
        }

        public static Operation Parse(string data) {
            if (data.StartsWith("rect")) return new FillOperation(data);
            if (data.StartsWith("rotate")) return new RotateOperation(data);
            throw new Exception($"Failed to parse operation data: {data}");
        }

        public abstract void Execute(bool[,] board);
    }
}
