using System;
using System.Collections.Generic;

public class Amplifier {
    public event Action<int> OnOutput;

    private IntCode _intCode;

    private Queue<int> _inputs;

    public Amplifier(string intCodeInput) {
        _intCode = new IntCode();
        _intCode.Load(intCodeInput);
        _intCode.OnInput += HandleOnInput;
        _intCode.OnOutput += HandleOnOutput;
    }

    public void Execute(int phaseSetting, int input) {
        _inputs = new Queue<int>();
        _inputs.Enqueue(phaseSetting);
        _inputs.Enqueue(input);

        _intCode.Reset();
        _intCode.Execute();
    }

    private int HandleOnInput() => _inputs.Dequeue();

    private void HandleOnOutput(int output) => OnOutput?.Invoke(output);
}
