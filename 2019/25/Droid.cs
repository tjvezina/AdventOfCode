using System;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019.Day25 {
    public class Droid {
        private IntCode _intCode;

        public bool isComplete => _intCode.state == IntCode.State.Complete;

        public string lastOutput { get; private set; }

        public Droid(string intCodeMemory) {
            _intCode = new IntCode(intCodeMemory);
            _intCode.OnOutput += HandleOnOutput;
            _intCode.Begin();
        }

        public void LoadSave(string[] moves) {
            foreach (string input in moves) {
                Input(input);
            }
        }

        public void ReadInput() {
            Console.Write("> ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Input(Console.ReadLine(), print:false);
        }

        public void TestAllItemCombinations() {
            string[] items = new[] {
                "semiconductor",
                "mutex",
                "sand",
                "asterisk",
                "dark matter",
                "ornament",
                "wreath",
                "loom"
            };

            int itemMask = 0b_1111_1111;

            do {
                foreach (string item in items) {
                    Input($"drop {item}");
                }
                int mask = 1;
                for (int i = 0; i < items.Length; ++i) {
                    if ((itemMask & mask) != 0) {
                        Input($"take {items[i]}");
                    }
                    mask <<= 1;
                }
                Input("east");
            } while (--itemMask > 0 && !isComplete);
        }

        private void Input(string input, bool print = true) {
            lastOutput = string.Empty;
            
            if (print) {
                Console.Write("> ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(input);
            }

            foreach (char c in input) {
                _intCode.Input((long)c);
            }

            Console.ResetColor();
            _intCode.Input('\n');
        }

        private void HandleOnOutput(long output) {
            char c = (char)output;
            lastOutput += c;
            Console.Write(c);
        }
    }
}
