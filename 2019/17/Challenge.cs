using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day17 {
     public class Challenge : BaseChallenge {
        private const int MAX_BOT_FUNC_LENGTH = 20;
        private const int MAX_ACTIONS_PER_FUNC = (MAX_BOT_FUNC_LENGTH + 1) / 4;

        private string _intCodeMemory;

        private List<char> _output = new List<char>();
        private CharMap _map;

        private Point _botPos;
        private Direction _botDir;
        private BotAction[] _actions;

        private char[] _mainRoutine;
        private char[][] _botFuncs;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YDown;
            _intCodeMemory = input;
        }

        public override string SolvePart1() {
            List<char> map = new List<char>();

            IntCode intCode = new IntCode(_intCodeMemory);
            intCode.OnOutput += HandleOnOutput;
            intCode.Begin();

            ProcessOutput();

            int alignParams = 0;
            foreach (Point intersection in GetIntersections()) {
                alignParams += (intersection.x * intersection.y);
            }

            return $"Sum of alignment parameters: {alignParams}";
        }

        public override string SolvePart2() {
            BuildActionsList();
            DefineBotFunctions();

            Dictionary<int, long> substitutions = new Dictionary<int, long> { { 0, 2 } };
            IntCode intCode = new IntCode(_intCodeMemory, substitutions);
            intCode.Begin();
            intCode.OnOutput += (o) => { if (o > char.MaxValue) Console.WriteLine($"Output: {o}"); };

            IEnumerable<char> inputBuffer = _mainRoutine.Append('\n');

            foreach (char[] func in _botFuncs) {
                inputBuffer = inputBuffer.Concat(func.Append('\n'));
            }

            // Continuous video feed?
            inputBuffer = inputBuffer.Concat(new [] { 'n', '\n' });

            Queue<char> input = new Queue<char>(inputBuffer);
            while (input.Count > 0) intCode.Input((long)input.Dequeue());

            return null;
        }

        private void HandleOnOutput(long output) => _output.Add((char)output);

        private void ProcessOutput() {
            int width = _output.IndexOf('\n');
            int height = (_output.Count + 1) / (width + 1); // Account for newline chars on all but the last line

            _map = new CharMap(width, height);

            Point? bot = null;
            List<char> botChars = new List<char> { '>', '<', '^', 'v' }; // Maps to Direction enum values
            for (int y = 0; y < height; ++y) {
                for (int x = 0; x < width; ++x) {
                    _map[x, y] = _output[x + y * (width + 1)];
                    if (bot == null && botChars.Contains(_map[x, y])) {
                        bot = new Point(x, y);
                    }
                }
            }

            _botPos = bot.Value;
            _botDir = (Direction)botChars.IndexOf(_map[_botPos.x, _botPos.y]);
            _map[_botPos.x, _botPos.y] = '#';
        }

        private IEnumerable<Point> GetIntersections() {
            foreach ((int x, int y, char c) in _map.Enumerate()) {
                Point p = new Point(x, y);
                // Checking a point + 3 sides is sufficient to confirm intersection
                if (IsPath(p) && IsPath(p + Direction.Up) && IsPath(p + Direction.Down) & IsPath(p + Direction.Left)) {
                    yield return p;
                }
            }
        }

        private void BuildActionsList() {
            List<BotAction> actions = new List<BotAction>();

            Point pos = _botPos;
            Direction lastDir = _botDir;

            while (true) {
                Turn turn = Turn.Right;
                Direction nextDir = lastDir.RotateCW();
                if (!IsPath(pos + nextDir)) {
                    turn = Turn.Left;
                    nextDir = lastDir.RotateCCW();
                    if (!IsPath(pos + nextDir)) {
                        break;
                    }
                }

                int steps = 0;
                do {
                    ++steps;
                    pos += nextDir;
                } while (IsPath(pos + nextDir));

                actions.Add(new BotAction(turn, steps));

                lastDir = nextDir;
            }

            _actions = actions.ToArray();
        }

        private void DefineBotFunctions() {
            // Convert each unique action into an identifier
            Dictionary<BotAction, char> actionToID = new Dictionary<BotAction, char>();
            Dictionary<char, string> idToAction = new Dictionary<char, string>();
            char nextID = '1';
            string actionIDs = string.Empty;
            foreach (BotAction action in _actions) {
                if (!actionToID.ContainsKey(action)) {
                    actionToID[action] = nextID;
                    idToAction[nextID] = action.ToString();
                    ++nextID;
                }
                actionIDs += actionToID[action];
            }

            string a = null;
            string b = null;
            string c = null;

            void TestAllPatterns() {
                for (int lenA = MAX_ACTIONS_PER_FUNC; lenA > 0; --lenA) {
                    for (int lenB = MAX_ACTIONS_PER_FUNC; lenB > 0; --lenB) {
                        for (int lenC = MAX_ACTIONS_PER_FUNC; lenC > 0; --lenC) {
                            if (TestPatterns(actionIDs, lenA, lenB, lenC, out a, out b, out c)) {
                                return;
                            }
                        }
                    }
                }
            }

            TestAllPatterns();

            BuildMainRoutine(actionIDs, a, b, c);

            char[] BuildFunc(string pattern) => pattern.Select(id => idToAction[id]).Aggregate((a, b) => $"{a},{b}").ToArray();

            _botFuncs = new char[][] {
                BuildFunc(a),
                BuildFunc(b),
                BuildFunc(c)
            };
        }

        private bool TestPatterns(string input, int lenA, int lenB, int lenC, out string a, out string b, out string c)
        {
            string[] inputs = new[] { input };
            a = b = c = null;

            if (inputs.Length == 0 || inputs[0].Length < lenA) return false;
            string A = inputs[0].Substring(0, lenA);
            inputs = inputs.SelectMany(i => i.Split(A, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            if (inputs.Length == 0 || inputs[0].Length < lenB) return false;
            string B = inputs[0].Substring(0, lenB);
            inputs = inputs.SelectMany(i => i.Split(B, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            if (inputs.Length == 0 || inputs[0].Length < lenC) return false;
            string C = inputs[0].Substring(0, lenC);
            inputs = inputs.SelectMany(i => i.Split(C, StringSplitOptions.RemoveEmptyEntries)).ToArray();

            if (inputs.Length > 0) return false;

            a = A;
            b = B;
            c = C;

            return true;
        }

        private void BuildMainRoutine(string actionIDs, string a, string b, string c) {
            actionIDs = actionIDs.Replace(a, "A");
            actionIDs = actionIDs.Replace(b, "B");
            actionIDs = actionIDs.Replace(c, "C");

            _mainRoutine = actionIDs.ToCharArray().Select(c => $"{c}").Aggregate((a, b) => $"{a},{b}").ToArray();
        }

        private bool IsPath(Point p) => _map.GetCharOrDefault(p) == '#';

        private int Occurances(string input, string pattern) {
            int count = 0;
            int i = 0;
            while ((i = input.IndexOf(pattern, i)) > 0) {
                ++count;
                i += pattern.Length;
            }
            return count;
        }
    }
}
