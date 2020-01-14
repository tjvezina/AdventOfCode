using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019.IntCodeV2 {
    public class IntCode {
        private enum State {
            Uninitialized,
            Loaded,
            Complete
        }

        private enum ParamMode {
            Position = 0,
            Immediate = 1
        }

        private struct Param {
            public int value;
            public ParamMode mode;
        }

        private struct Instruction {
            public int paramCount;
            public Action<Param[]> action;
        }

        private const char Separator = ',';

        private readonly Dictionary<int, Instruction> s_Instructions;

        private State _state = State.Uninitialized;
        private List<int> _memory;
        private List<int> _initialMemory;
        private int _memoryPtr;

        public int Length => _memory.Count;
        public int this[int i] => _memory[i];

        public event Action<int> OnOutput;
        public event Func<int> OnInput;

        public IntCode() {
            s_Instructions = new Dictionary<int, Instruction> {
                { 01, new Instruction { paramCount = 3, action = InstructionAdd } },
                { 02, new Instruction { paramCount = 3, action = InstructionMult } },
                { 03, new Instruction { paramCount = 1, action = InstructionInput } },
                { 04, new Instruction { paramCount = 1, action = InstructionOutput } },
                { 05, new Instruction { paramCount = 2, action = InstructionJumpIfTrue } },
                { 06, new Instruction { paramCount = 2, action = InstructionJumpIfFalse } },
                { 07, new Instruction { paramCount = 3, action = InstructionLessThan } },
                { 08, new Instruction { paramCount = 3, action = InstructionEqual } },
                { 99, new Instruction { paramCount = 0, action = InstructionHalt } }
            };
        }

        public void Reset() {
            _state = State.Loaded;
            _memory = new List<int>(_initialMemory);
            _memoryPtr = 0;
        }

        public void Load(string input) {
            Debug.Assert(_state == State.Uninitialized, "Code has already been loaded!");

            _memory = input.Split(Separator).Select(int.Parse).ToList();
            _initialMemory = new List<int>(_memory);
            _state = State.Loaded;
        }

        public void Execute() {
            Debug.Assert(_state == State.Loaded, _state == State.Uninitialized
                ? "Unable to execute before loading, no program to run!"
                : "Already executed! Reset and load before executing again.");

            while (_state != State.Complete) {
                Debug.Assert(_memoryPtr < Length, "End of program reached before halt opcode found.");

                int opData = _memory[_memoryPtr++];
                int opCode = opData % 100;
                opData /= 100;
                Debug.Assert(s_Instructions.ContainsKey(opCode), "Unknown opCode: " + opCode);

                Instruction instruction = s_Instructions[opCode];

                Param[] opParams = new Param[instruction.paramCount];
                for (int i = 0; i < instruction.paramCount; ++i) {
                    ParamMode paramMode = (ParamMode)(opData % 10);
                    opData /= 10;

                    int param = _memory[_memoryPtr++];
                    opParams[i] = new Param { value = param, mode = paramMode };
                }

                instruction.action(opParams);
            }
        }

        private void InstructionHalt(Param[] opParams) => _state = State.Complete;

        private void InstructionAdd(Param[] opParams) => InstructionMathOp(opParams, (a, b) => a + b);
        private void InstructionMult(Param[] opParams) => InstructionMathOp(opParams, (a, b) => a * b);
        private void InstructionMathOp(Param[] opParams, Func<int, int, int> op) {
            _memory[opParams[2].value] = op(ResolveParam(opParams[0]), ResolveParam(opParams[1]));
        }

        private void InstructionInput(Param[] opParams) {
            int input = OnInput();
            _memory[opParams[0].value] = input;
        }

        private void InstructionOutput(Param[] opParams) {
            OnOutput(ResolveParam(opParams[0]));
        }

        private void InstructionJumpIfTrue(Param[] opParams) => InstructionJump(opParams, v => v != 0);
        private void InstructionJumpIfFalse(Param[] opParams) => InstructionJump(opParams, v => v == 0);
        private void InstructionJump(Param[] opParams, Func<int, bool> condition) {
            if (condition(ResolveParam(opParams[0]))) {
                _memoryPtr = ResolveParam(opParams[1]);
            }
        }

        private void InstructionLessThan(Param[] opParams) => InstructionCondition(opParams, (a, b) => a < b);
        private void InstructionEqual(Param[] opParams) => InstructionCondition(opParams, (a, b) => a == b);
        private void InstructionCondition(Param[] opParams, Func<int, int, bool> condition) {
            _memory[opParams[2].value] = (condition(ResolveParam(opParams[0]), ResolveParam(opParams[1])) ? 1 : 0);
        }

        private int ResolveParam(Param param) {
            switch (param.mode) {
                case ParamMode.Immediate: return param.value;
                case ParamMode.Position:  return _memory[param.value];
                default:
                    throw new Exception("Unhandled param mode: " + param.mode);
            }
        }
    }
}
