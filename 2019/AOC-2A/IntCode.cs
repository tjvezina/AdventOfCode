using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class IntCode {
    private enum State {
        Uninitialized,
        Loaded,
        Complete
    }

    private struct Instruction {
        public int paramCount;
        public Action<int> action;
    }

    private const char SEPARATOR = ',';

    private readonly Dictionary<int, Instruction> s_Instructions;

    private State _state = State.Uninitialized;
    private List<int> _memory;
    private List<int> _initialMemory;

    public int Length => _memory.Count;
    public int this[int i] => _memory[i];

    public IntCode() {
        s_Instructions = new Dictionary<int, Instruction> {
            { 1, new Instruction { paramCount = 3, action = InstructionAdd } },
            { 2, new Instruction { paramCount = 3, action = InstructionMult } },
            { 99, new Instruction { paramCount = 0, action = InstructionHalt } }
        };
    }

    public void Reset() {
        _state = State.Uninitialized;
        _memory = new List<int>(_initialMemory);
    }

    public void Load(string input) {
        Debug.Assert(_state == State.Uninitialized, "Code has already been loaded!");

        _memory = input.Split(SEPARATOR).Select(int.Parse).ToList();
        _initialMemory = new List<int>(_memory);
        _state = State.Loaded;
    }

    public void Execute(int noun, int verb) {
        Debug.Assert(_state == State.Loaded, _state == State.Uninitialized
            ? "Unable to execute before loading, no program to run!"
            : "Already executed! Reset and load before executing again.");

        _memory[1] = noun;
        _memory[2] = verb;

        int address = 0;

        while (_state != State.Complete) {
            Debug.Assert(address < Length, "End of program reached before exit opcode found.");

            int opCode = _memory[address];
            Debug.Assert(s_Instructions.ContainsKey(opCode), "Unknown opCode: " + opCode);

            Instruction instruction = s_Instructions[opCode];
            Debug.Assert(address + instruction.paramCount < Length, "Unable to complete instruction, end of program reached.");

            instruction.action(address);

            address += 1 + instruction.paramCount;
        }
    }

    private void InstructionHalt(int address) => _state = State.Complete;
    private void InstructionAdd(int address) => InstructionMathOp(address, (a, b) => a + b);
    private void InstructionMult(int address) => InstructionMathOp(address, (a, b) => a * b);
    private void InstructionMathOp(int address, Func<int, int, int> op) {
        int[] opParams = new int[3];

        // Read and validate the instruction's parameters
        for (int i = 0; i < 3; ++i) {
            int opIndex = _memory[address + i + 1];
            Debug.Assert(opIndex >= 0 && opIndex < Length, $"Instruction failed, index {i} is invalid: {opIndex}");
            opParams[i] = opIndex;
        }

        // Read the values at index 0 and 1, perform the given operation, then write to index 2
        _memory[opParams[2]] = op(_memory[opParams[0]], _memory[opParams[1]]);
    }
}
