using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019 {
    public class Day17 : Challenge {
        private IntCode _intCode;

        private List<char> _output = new List<char>();
        private char[,] _map;
        private int _width;
        private int _height;

        private Point _botPos;
        private Direction _botDir;
        private BotAction[] _actions;

        private void Init(string input) {
            _intCode = new IntCode(input);
        }

        protected override string SolvePart1() {
            List<char> map = new List<char>();

            _intCode.OnOutput += HandleOnOutput;
            _intCode.Begin();

            ProcessOutput();

            int alignParams = 0;
            foreach (Point intersection in GetIntersections()) {
                alignParams += (intersection.x * intersection.y);
            }

            return $"Sum of alignment parameters: {alignParams}";
        }
        
        protected override string SolvePart2() {
            BuildActionsList();
            return null;
        }

        private void HandleOnOutput(long output) => _output.Add((char)output);

        private void ProcessOutput() {
            _width = _output.IndexOf('\n');
            _height = (_output.Count + 1) / (_width + 1); // Account for newline chars on all but the last line

            _map = new char[_width, _height];

            Point? bot = null;
            List<char> botChars = new List<char> { '>', '<', '^', 'v' }; // Maps to Direction enum values
            for (int y = 0; y < _height; ++y) {
                for (int x = 0; x < _width; ++x) {
                    _map[x, y] = _output[x + y * (_width + 1)];
                    if (bot == null && botChars.Contains(_map[x, y])) {
                        bot = new Point(x, y);
                    }
                }
            }

            _botPos = bot.Value;
            _botDir = (Direction)botChars.IndexOf(_map[_botPos.x, _botPos.y]);
        }

        private IEnumerable<Point> GetIntersections() {
            for (int y = 1; y < _height - 1; ++y) {
                for (int x = 1; x < _width - 1; ++x) {
                    // Checking a point + 3 sides is sufficient to confirm intersection
                    if (_map[x, y] == '#' &&
                        _map[x+1, y] == '#' &&
                        _map[x, y+1] == '#' &&
                        _map[x-1, y] == '#') {
                        yield return new Point(x, y);
                    }
                }
            }
        }

        private void BuildActionsList() {
            // TODO
        }
    }
}
