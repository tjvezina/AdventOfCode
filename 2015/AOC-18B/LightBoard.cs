using System;
using System.IO;

public class LightBoard {
    private bool[,] _board;
    private int _width;
    private int _height;

    public int litCount {
        get {
            int count = 0;
            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    count += (_board[x, y] ? 1 : 0);
                }
            }
            return count;
        }
    }

    public LightBoard(string[] input) {
        _width = input[0].Length;
        _height = input.Length;

        _board = new bool[_width, _height];
        for (int y = 0; y < _height; ++y) {
            string line = input[y];
            for (int x = 0; x < _width; ++x) {
                _board[x, y] = (line[x] == '#');
            }
        }

        // Corners are always on
        _board[0, 0] = true;
        _board[_width-1, 0] = true;
        _board[0, _height-1] = true;
        _board[_width-1, _height-1] = true;
    }

    public void Update() {
        bool[,] nextBoard = new bool[_width, _height];

        for (int y = 0; y < _height; ++y) {
            for (int x = 0; x < _width; ++x) {
                // Skip corners (always on)
                if ((x == 0 && y == 0) || (x == _width-1 && y == 0) ||
                    (x == 0 && y == _height-1) || (x == _width-1 && y == _height-1)) {
                    nextBoard[x, y] = true;
                    continue;
                }

                int litNeighbors = 0;
                for (int y2 = -1; y2 <= 1; ++y2) {
                    for (int x2 = -1; x2 <= 1; ++x2) {
                        // Skip self and out-of-bounds
                        if ((x2 == 0 && y2 == 0) || 
                            (x + x2 < 0 || x + x2 >= _width) ||
                            (y + y2 < 0 || y + y2 >= _height)) {
                            continue;
                        }

                        if (_board[x + x2, y + y2]) {
                            ++litNeighbors;
                        }
                    }
                }
                nextBoard[x, y] =
                    (_board[x, y] && (litNeighbors >= 2 && litNeighbors <= 3)) ||
                    (!_board[x, y] && litNeighbors == 3);
            }
        }

        Array.Copy(nextBoard, _board, nextBoard.Length);
    }
}
