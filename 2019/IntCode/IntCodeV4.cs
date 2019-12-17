using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019.IntCodeV4 {
    public class IntCode {
        public enum State {
            Default,
            Waiting,
            Complete
        }

        private enum ParamMode {
            Position = 0,
            Immediate = 1,
            Relative = 2
        }

        private struct Param {
            public long value;
            public ParamMode mode;
        }

        private struct Operator {
            public int paramCount;
            public Action<Param[]> action;
        }

        private readonly Dictionary<int, Operator> OPERATORS;

        public event Action<long> OnOutput;

        public int length => _memory.Count;

        public State state { get; private set; }
        private List<long> _memory;
        private long[] _initialMemory;
        private int _memoryPtr;
        private Param? _inputDest;
        private int _relativeBase;

        public IntCode(string memory, Dictionary<int, long> substitutions = null) {
            OPERATORS = new Dictionary<int, Operator> {
                { 01, new Operator { paramCount = 3, action = OpAdd } },
                { 02, new Operator { paramCount = 3, action = OpMult } },
                { 03, new Operator { paramCount = 1, action = OpInput } },
                { 04, new Operator { paramCount = 1, action = OpOutput } },
                { 05, new Operator { paramCount = 2, action = OpJumpIfTrue } },
                { 06, new Operator { paramCount = 2, action = OpJumpIfFalse } },
                { 07, new Operator { paramCount = 3, action = OpLessThan } },
                { 08, new Operator { paramCount = 3, action = OpEqual } },
                { 09, new Operator { paramCount = 1, action = OpMoveRelativeBase } },
                { 99, new Operator { paramCount = 0, action = OpHalt } }
            };

            _initialMemory = memory.Split(',').Select(long.Parse).ToArray();

            if (substitutions != null) {
                foreach ((int index, long value) in substitutions) {
                    _initialMemory[index] = value;
                }
            }

            Reset();
        }

        public void Reset() {
            state = State.Default;
            _memory = new List<long>(_initialMemory);
            _memoryPtr = 0;
            _relativeBase = 0;
            _inputDest = null;
        }

        public void Begin() {
            Debug.Assert(state == State.Default, "Execution has already begun, do you need to reset first?");

            Continue();
        }

        public void Input(long input) {
            Debug.Assert(state == State.Waiting, "Received input, but not expecting any.");

            WriteToMemory(_inputDest.Value, input);
            _inputDest = null;
            Continue();
        }

        private void Continue() {
            Debug.Assert(_memoryPtr < length, "End of program reached before halt opcode found.");
            state = State.Default;

            long ReadMemory() => _memory[_memoryPtr++];

            while (state == State.Default) {
                long opData = ReadMemory();
                int opCode = (int)(opData % 100);
                opData /= 100;

                Operator op = OPERATORS[opCode];

                Param[] opParams = new Param[op.paramCount];
                for (int i = 0; i < op.paramCount; ++i) {
                    ParamMode paramMode = (ParamMode)(opData % 10);
                    opData /= 10;

                    long param = ReadMemory();
                    opParams[i] = new Param { value = param, mode = paramMode };
                }

                op.action(opParams);
            }
        }

        private void OpHalt(Param[] opParams) => state = State.Complete;

        private void OpAdd(Param[] opParams) => OpMath(opParams, (a, b) => a + b);
        private void OpMult(Param[] opParams) => OpMath(opParams, (a, b) => a * b);
        private void OpMath(Param[] opParams, Func<long, long, long> op) {
            WriteToMemory(opParams[2], op(ResolveParam(opParams[0]), ResolveParam(opParams[1])));
        }

        private void OpInput(Param[] opParams) {
            _inputDest = opParams[0];
            state = State.Waiting;
        }

        private void OpOutput(Param[] opParams) => OnOutput?.Invoke(ResolveParam(opParams[0]));

        private void OpJumpIfTrue(Param[] opParams) => OpJump(opParams, v => v != 0);
        private void OpJumpIfFalse(Param[] opParams) => OpJump(opParams, v => v == 0);
        private void OpJump(Param[] opParams, Func<long, bool> condition) {
            if (condition(ResolveParam(opParams[0]))) {
                _memoryPtr = (int)ResolveParam(opParams[1]);
            }
        }

        private void OpLessThan(Param[] opParams) => OpCondition(opParams, (a, b) => a < b);
        private void OpEqual(Param[] opParams) => OpCondition(opParams, (a, b) => a == b);
        private void OpCondition(Param[] opParams, Func<long, long, bool> condition) {
            long paramA = ResolveParam(opParams[0]);
            long paramB = ResolveParam(opParams[1]);
            WriteToMemory(opParams[2], condition(paramA, paramB) ? 1 : 0);
        }

        private void OpMoveRelativeBase(Param[] opParams) => _relativeBase += (int)ResolveParam(opParams[0]);

        private long ResolveParam(Param param) {
            long ReadMemory(int index) => (index < _memory.Count ? _memory[index] : 0);

            switch (param.mode) {
                case ParamMode.Immediate: return param.value;
                case ParamMode.Position:  return ReadMemory((int)param.value);
                case ParamMode.Relative:  return ReadMemory((int)param.value + _relativeBase);
                default:
                    throw new Exception("Unhandled param mode: " + param.mode);
            }
        }

        private void WriteToMemory(Param writeParam, long value) {
            int position = (int)writeParam.value;
            if (writeParam.mode == ParamMode.Relative) {
                position += _relativeBase;
            }

            Debug.Assert(position >= 0, "Invalid write address: " + position);

            while (_memory.Count <= position) {
                _memory.Add(0);
            }

            _memory[position] = value;
        }
    }
}
