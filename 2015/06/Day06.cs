using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015 {
    public class Day06 : Challenge {
        private enum InstructionType { TurnOn, TurnOff, Toggle }

        private const string TYPE_TURN_ON = "turn on";
        private const string TYPE_TURN_OFF = "turn off";
        private const string TYPE_TOGGLE = "toggle";
        private static readonly string[] SEPARATORS = new[] { ",", " through " };

        private struct Instruction {
            public InstructionType type;
            public Point start;
            public Point end;

            public Instruction(string data) {
                if (data.StartsWith(TYPE_TURN_ON)) {
                    type = InstructionType.TurnOn;
                    data = data.Substring(TYPE_TURN_ON.Length + 1);
                }
                else if (data.StartsWith(TYPE_TURN_OFF)) {
                    type = InstructionType.TurnOff;
                    data = data.Substring(TYPE_TURN_OFF.Length + 1);
                }
                else if (data.StartsWith(TYPE_TOGGLE)) {
                    type = InstructionType.Toggle;
                    data = data.Substring(TYPE_TOGGLE.Length + 1);
                }
                else {
                    throw new Exception("Failed to determine instruction type: " + data);
                }

                int[] parts = data.Split(SEPARATORS, StringSplitOptions.None).Select(int.Parse).ToArray();
                start = new Point(parts[0], parts[1]);
                end = new Point(parts[2], parts[3]);
            }
        }

        private IEnumerable<Instruction> _instructions;

        private void Init(string[] input) {
            _instructions = input.Select(i => new Instruction(i));
        }

        protected override string SolvePart1() {
            LightBoard board = new LightBoard(
                turnOn:(x) => 1,
                turnOff:(x) => 0,
                toggle:(x) => x == 0 ? 1 : 0
            );

            return $"Lit lights: {RunInstructions(board)}";
        }
        
        protected override string SolvePart2() {
            LightBoard board = new LightBoard(
                turnOn:(x) => x + 1,
                turnOff:(x) => Math.Min(0, x - 1),
                toggle:(x) => x + 2
            );

            return $"Total brightness: {RunInstructions(board)}";
        }

        private int RunInstructions(LightBoard board) {

            foreach (Instruction instruction in _instructions) {
                switch (instruction.type) {
                    case InstructionType.TurnOn:  board.TurnOn(instruction.start, instruction.end);  break;
                    case InstructionType.TurnOff: board.TurnOff(instruction.start, instruction.end); break;
                    case InstructionType.Toggle:  board.Toggle(instruction.start, instruction.end);  break;
                }
            }

            return board.Brightness;
        }
    }
}
