using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Year2019.IntCodeV4;

namespace AdventOfCode.Year2019 {
    public class Day21 : Challenge {
        private IntCode _intCode;

        private void Init(string input) {
            _intCode = new IntCode(input);
            _intCode.OnOutput += HandleOutput;
        }

        protected override string SolvePart1() {
            RunSpringBot(new[] {
                "NOT A J", // If A is hole
                "NOT B T",
                "OR T J",  // Or B is hole
                "NOT C T",
                "OR T J",  // Or C is hole
                "AND D J", // And D is ground
                "WALK"
            });

            return null;
        }
        
        protected override string SolvePart2() {
            RunSpringBot(new[] {
                "OR E T", // (1) If E is ground (can walk after jump)
                "OR H T", // (1) Or H is ground (can jump after jump)
                "OR A J", // (2) If A is hole
                "AND B J", // (2) Or B is hole
                "AND C J", // (2) Or C is hole
                "NOT J J",
                "AND D J", // (2) And D is ground
                "AND T J", // If (1) and (2)
                "RUN"
            });

            return null;
        }

        private void RunSpringBot(string[] instructions) {
            Queue<char> input = new Queue<char>(instructions.Aggregate((a, b) => $"{a}\n{b}") + "\n");

            _intCode.Reset();
            _intCode.Begin();

            while (_intCode.state != IntCode.State.Complete && input.Count > 0) {
                _intCode.Input((long)input.Dequeue());
            }
        }

        private void HandleOutput(long output) {
            if (output <= char.MaxValue) {
                Console.Write((char)output);
            } else {
                Console.WriteLine($"Output: {output}");
            }
        }
    }
}
