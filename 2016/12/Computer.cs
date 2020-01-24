using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day12 {
    public class Computer {
        private Dictionary<string, int> _registers = new Dictionary<string, int> {
            { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }
        };

        public int this[string registerID] {
            get => _registers[registerID];
            set => _registers[registerID] = value;
        }

        private Instruction[] _instructions;

        public Computer(string[] input) {
            _instructions = input.Select(i => new Instruction(i)).ToArray();
        }

        public void Run() {
            int instructionPtr = 0;

            int ResolveArg(object arg) => arg is int value ? value : _registers[(string)arg];

            while (instructionPtr < _instructions.Length) {
                Instruction instruction = _instructions[instructionPtr];

                int step = 1;

                switch (instruction.type) {
                    case Instruction.Type.Increment:
                        _registers[(string)instruction.args[0]]++;
                        break;
                    case Instruction.Type.Decrement:
                        _registers[(string)instruction.args[0]]--;
                        break;
                    case Instruction.Type.Copy:
                        _registers[(string)instruction.args[1]] = ResolveArg(instruction.args[0]);
                        break;
                    case Instruction.Type.JumpIfNonZero:
                        if (ResolveArg(instruction.args[0]) != 0) step = (int)instruction.args[1];
                        break;
                }

                instructionPtr += step;
            }
        }
    }
}
