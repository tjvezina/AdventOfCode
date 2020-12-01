using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2019.IntCodeV3
{
    public class IntCode
    {
        public enum State
        {
            Default,
            Waiting,
            Complete
        }

        private enum ParamMode
        {
            Position = 0,
            Immediate = 1
        }

        private struct Param
        {
            public int value;
            public ParamMode mode;
        }

        private struct Operator
        {
            public int paramCount;
            public Action<Param[]> action;
        }

        private readonly Dictionary<int, Operator> Operators;

        public event Action<int> OnOutput;

        public int length => _memory.Length;

        public State state { get; private set; }
        private int[] _memory;
        private int[] _initialMemory;
        private int _memoryPtr;
        private int _inputDest;

        public IntCode(string memory)
        {
            Operators = new Dictionary<int, Operator>
            {
                { 01, new Operator { paramCount = 3, action = OpAdd } },
                { 02, new Operator { paramCount = 3, action = OpMult } },
                { 03, new Operator { paramCount = 1, action = OpInput } },
                { 04, new Operator { paramCount = 1, action = OpOutput } },
                { 05, new Operator { paramCount = 2, action = OpJumpIfTrue } },
                { 06, new Operator { paramCount = 2, action = OpJumpIfFalse } },
                { 07, new Operator { paramCount = 3, action = OpLessThan } },
                { 08, new Operator { paramCount = 3, action = OpEqual } },
                { 99, new Operator { paramCount = 0, action = OpHalt } }
            };

            _initialMemory = memory.Split(',').Select(int.Parse).ToArray();
            _memory = new int[_initialMemory.Length];
            Reset();
        }

        public void Reset()
        {
            state = State.Default;
            _initialMemory.CopyTo(_memory, 0);
            _memoryPtr = 0;
            _inputDest = -1;
        }

        public void Begin()
        {
            Debug.Assert(state == State.Default, "Execution has already begun, do you need to reset first?");

            Continue();
        }

        public void Input(int input)
        {
            Debug.Assert(state == State.Waiting, "Received input, but not expecting any.");

            _memory[_inputDest] = input;
            _inputDest = -1;
            Continue();
        }

        private void Continue()
        {
            Debug.Assert(_memoryPtr < length, "End of program reached before halt opcode found.");
            state = State.Default;

            int ReadMemory() => _memory[_memoryPtr++];

            while (state == State.Default)
            {
                int opData = ReadMemory();
                int opCode = opData % 100;
                opData /= 100;

                Operator op = Operators[opCode];

                Param[] opParams = new Param[op.paramCount];
                for (int i = 0; i < op.paramCount; i++)
                {
                    ParamMode paramMode = (ParamMode)(opData % 10);
                    opData /= 10;

                    int param = ReadMemory();
                    opParams[i] = new Param { value = param, mode = paramMode };
                }

                op.action(opParams);
            }
        }

        private void OpHalt(Param[] opParams) => state = State.Complete;

        private void OpAdd(Param[] opParams) => OpMath(opParams, (a, b) => a + b);
        private void OpMult(Param[] opParams) => OpMath(opParams, (a, b) => a * b);
        private void OpMath(Param[] opParams, Func<int, int, int> op)
        {
            _memory[opParams[2].value] = op(ResolveParam(opParams[0]), ResolveParam(opParams[1]));
        }

        private void OpInput(Param[] opParams)
        {
            _inputDest = opParams[0].value;
            state = State.Waiting;
        }

        private void OpOutput(Param[] opParams) => OnOutput(ResolveParam(opParams[0]));

        private void OpJumpIfTrue(Param[] opParams) => OpJump(opParams, v => v != 0);
        private void OpJumpIfFalse(Param[] opParams) => OpJump(opParams, v => v == 0);
        private void OpJump(Param[] opParams, Func<int, bool> condition)
        {
            if (condition(ResolveParam(opParams[0])))
            {
                _memoryPtr = ResolveParam(opParams[1]);
            }
        }

        private void OpLessThan(Param[] opParams) => OpCondition(opParams, (a, b) => a < b);
        private void OpEqual(Param[] opParams) => OpCondition(opParams, (a, b) => a == b);
        private void OpCondition(Param[] opParams, Func<int, int, bool> condition)
        {
            int paramA = ResolveParam(opParams[0]);
            int paramB = ResolveParam(opParams[1]);
            _memory[opParams[2].value] = (condition(paramA, paramB) ? 1 : 0);
        }

        private int ResolveParam(Param param)
        {
            switch (param.mode)
            {
                case ParamMode.Immediate: return param.value;
                case ParamMode.Position:  return _memory[param.value];
                default:
                    throw new Exception("Unhandled param mode: " + param.mode);
            }
        }
    }
}