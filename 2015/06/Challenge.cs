using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day06
{
    public class Challenge : BaseChallenge
    {
        private enum InstructionType { TurnOn, TurnOff, Toggle }

        private const string TypeTurnOn = "turn on";
        private const string TypeTurnOff = "turn off";
        private const string TypeToggle = "toggle";
        private static readonly string[] Separators = { ",", " through " };

        private readonly struct Instruction
        {
            public readonly InstructionType type;
            public readonly Point start;
            public readonly Point end;

            public Instruction(string data)
            {
                if (data.StartsWith(TypeTurnOn))
                {
                    type = InstructionType.TurnOn;
                    data = data.Substring(TypeTurnOn.Length + 1);
                }
                else if (data.StartsWith(TypeTurnOff))
                {
                    type = InstructionType.TurnOff;
                    data = data.Substring(TypeTurnOff.Length + 1);
                }
                else if (data.StartsWith(TypeToggle))
                {
                    type = InstructionType.Toggle;
                    data = data.Substring(TypeToggle.Length + 1);
                }
                else
                {
                    throw new Exception("Failed to determine instruction type: " + data);
                }

                int[] parts = data.Split(Separators, StringSplitOptions.None).Select(int.Parse).ToArray();
                start = new Point(parts[0], parts[1]);
                end = new Point(parts[2], parts[3]);
            }
        }

        private readonly IReadOnlyList<Instruction> _instructions;

        public Challenge() => _instructions = inputList.Select(i => new Instruction(i)).ToList();

        public override object part1ExpectedAnswer => 377891;
        public override (string message, object answer) SolvePart1()
        {
            LightBoard board = new LightBoard(
                turnOn: x => 1,
                turnOff: x => 0,
                toggle: x => x == 0 ? 1 : 0
            );

            return ("Lit lights: ", RunInstructions(board));
        }
        
        public override object part2ExpectedAnswer => 14110788;
        public override (string message, object answer) SolvePart2()
        {
            LightBoard board = new LightBoard(
                turnOn: x => x + 1,
                turnOff: x => Math.Max(0, x - 1),
                toggle: x => x + 2
            );

            return ("Total brightness: ", RunInstructions(board));
        }

        private int RunInstructions(LightBoard board)
        {
            foreach (Instruction instruction in _instructions)
            {
                switch (instruction.type)
                {
                    case InstructionType.TurnOn:  board.TurnOn(instruction.start, instruction.end);  break;
                    case InstructionType.TurnOff: board.TurnOff(instruction.start, instruction.end); break;
                    case InstructionType.Toggle:  board.Toggle(instruction.start, instruction.end);  break;
                }
            }

            return board.brightness;
        }
    }
}
