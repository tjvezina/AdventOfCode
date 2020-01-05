using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day23 {
    public class Computer {
        private class Register {
            public ulong value;
        }

        private Dictionary<string, Register> _registers = new Dictionary<string, Register> {
            { "a", new Register() }, { "b", new Register() }
        };

        public ulong this[string registerID] {
            get => _registers[registerID].value;
            set => _registers[registerID].value = value;
        }

        private Instruction[] _instructions;

        public Computer(string[] input) {
            _instructions = input.Select(i => new Instruction(i)).ToArray();
        }

        public void Run() {
            int instructionPtr = 0;

            while (0 <= instructionPtr && instructionPtr < _instructions.Length) {
                Instruction instruction = _instructions[instructionPtr];
                Register register = (instruction.register != null ? _registers[instruction.register] : null);

                int step = 1;

                switch (instruction.type) {
                    case Instruction.Type.Half:
                        register.value /= 2;
                        break;
                    case Instruction.Type.Triple:
                        register.value *= 3;
                        break;
                    case Instruction.Type.Increment:
                        register.value += 1;
                        break;
                    case Instruction.Type.Jump:
                        step = instruction.offset;
                        break;
                    case Instruction.Type.JumpIfEven:
                        if (register.value % 2 == 0) step = instruction.offset;
                        break;
                    case Instruction.Type.JumpIfOne:
                        if (register.value == 1) step = instruction.offset;
                        break;
                }

                instructionPtr += step;
            }
        }
    }
}
