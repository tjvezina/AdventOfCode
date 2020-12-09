using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day08
{
    public class Challenge : BaseChallenge
    {
        private enum InstructionType
        {
            Accumulator,
            Jump,
            NoOp
        }

        private class Instruction
        {
            public static Instruction Parse(string data)
            {
                InstructionType type;

                string[] parts = data.Split(' ');
                switch (parts[0])
                {
                    case "acc": type = InstructionType.Accumulator; break;
                    case "jmp": type = InstructionType.Jump; break;
                    case "nop": type = InstructionType.NoOp; break;
                    default:
                        throw new Exception("Unrecognized instruction type: " + parts[0]);
                }

                int value = int.Parse(parts[1]);

                return new Instruction(type, value);
            }

            public InstructionType type { get; private set; }
            public int value { get; }

            private Instruction(InstructionType type, int value)
            {
                this.type = type;
                this.value = value;
            }

            public bool Uncorrupt()
            {
                switch (type)
                {
                    case InstructionType.Jump:
                        type = InstructionType.NoOp;
                        return true;
                    case InstructionType.NoOp:
                        type = InstructionType.Jump;
                        return true;
                    default:
                        return false;
                }
            }
        }

        private readonly List<Instruction> _program;

        public Challenge()
        {
            _program = inputList.Select(Instruction.Parse).ToList();
        }

        public override object part1ExpectedAnswer => 1867;
        public override (string message, object answer) SolvePart1()
        {
            (int pointer, int accumulator) = Execute();

            if (pointer >= _program.Count)
            {
                throw new Exception("No infinite loop found in program, failed to fail!");
            }

            return ($"Program repeated instruction {pointer}! Accumulator: ", accumulator);
        }

        public override object part2ExpectedAnswer => 1303;
        public override (string message, object answer) SolvePart2()
        {
            for (int i = 0; i < _program.Count; i++)
            {
                Instruction instruction = _program[i];

                if (instruction.Uncorrupt())
                {
                    (int pointer, int accumulator) = Execute();

                    if (pointer == _program.Count)
                    {
                        return ($"Program success! Swapped #{i} to {instruction.type}. Accumulator: ", accumulator);
                    }

                    instruction.Uncorrupt();
                }
            }

            throw new Exception("Failed to resolve program corruption");
        }

        private (int pointer, int accumulator) Execute()
        {
            int pointer = 0;
            int accumulator = 0;

            bool[] executions = new bool[_program.Count];

            while (0 <= pointer && pointer < _program.Count)
            {
                // Detect infinite loops and abort
                if (executions[pointer])
                {
                    break;
                }

                executions[pointer] = true;

                Instruction instruction = _program[pointer];

                switch (instruction.type)
                {
                    case InstructionType.Accumulator:
                        accumulator += instruction.value;
                        pointer++;
                        break;
                    case InstructionType.Jump:
                        pointer += instruction.value;
                        break;
                    case InstructionType.NoOp:
                        pointer++;
                        break;
                }
            }

            return (pointer, accumulator);
        }
    }
}
