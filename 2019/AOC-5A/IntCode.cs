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

    private enum ParamMode {
        Position = 0,
        Immediate = 1
    }

    private struct Instruction {
        public int paramCount;
        public Action<int, ParamMode[]> action;
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
            { 01, new Instruction { paramCount = 3, action = InstructionAdd } },
            { 02, new Instruction { paramCount = 3, action = InstructionMult } },
            { 03, new Instruction { paramCount = 1, action = InstructionInput } },
            { 04, new Instruction { paramCount = 1, action = InstructionOutput } },
            { 99, new Instruction { paramCount = 0, action = InstructionHalt } }
        };
    }

    public void Reset() {
        _state = State.Loaded;
        _memory = new List<int>(_initialMemory);
    }

    public void Load(string input) {
        Debug.Assert(_state == State.Uninitialized, "Code has already been loaded!");

        _memory = input.Split(SEPARATOR).Select(int.Parse).ToList();
        _initialMemory = new List<int>(_memory);
        _state = State.Loaded;
    }

    public void Execute() {
        Debug.Assert(_state == State.Loaded, _state == State.Uninitialized
            ? "Unable to execute before loading, no program to run!"
            : "Already executed! Reset and load before executing again.");

        int address = 0;

        while (_state != State.Complete) {
            Debug.Assert(address < Length, "End of program reached before exit opcode found.");

            int opData = _memory[address];
            int opCode = opData % 100;
            Debug.Assert(s_Instructions.ContainsKey(opCode), "Unknown opCode: " + opCode);

            Instruction instruction = s_Instructions[opCode];
            Debug.Assert(address + instruction.paramCount < Length, "Unable to complete instruction, end of program reached.");

            opData /= 100;
            ParamMode[] paramModes = new ParamMode[instruction.paramCount];
            for (int i = 0; i < instruction.paramCount; ++i) {
                paramModes[i] = (ParamMode)(opData % 10);
                opData /= 10;
            }

            instruction.action(address, paramModes);

            address += 1 + instruction.paramCount;
        }
    }

    private void InstructionHalt(int address, ParamMode[] paramModes) => _state = State.Complete;

    private void InstructionAdd(int address, ParamMode[] paramModes)
        => InstructionMathOp(address, paramModes, (a, b) => a + b);
    private void InstructionMult(int address, ParamMode[] paramModes)
        => InstructionMathOp(address, paramModes, (a, b) => a * b);
    private void InstructionMathOp(int address, ParamMode[] paramModes, Func<int, int, int> op) {
        int[] opParams = new int[3];

        for (int i = 0; i < 3; ++i) {
            opParams[i] = _memory[address + i + 1];
        }

        int value0 = GetParamValue(opParams[0], paramModes[0]);
        int value1 = GetParamValue(opParams[1], paramModes[1]);

        _memory[opParams[2]] = op(value0, value1);
    }

    private void InstructionInput(int address, ParamMode[] paramModes) {
        int param = _memory[address + 1];
        
        Console.Write("Input : ");
        _memory[param] = int.Parse(Console.ReadLine());
    }

    private void InstructionOutput(int address, ParamMode[] paramModes) {
        int param = _memory[address + 1];

        Console.WriteLine("Output: " + GetParamValue(param, paramModes[0]));
    }

    private int GetParamValue(int param, ParamMode mode) {
        switch (mode) {
            case ParamMode.Immediate: return param;
            case ParamMode.Position: return _memory[param];
            default:
                throw new Exception("Unhandled param mode: " + mode);
        }
    }
}
