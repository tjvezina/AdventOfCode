public class Amplifier {
    public bool IsComplete => _intCode.state == IntCode.State.Complete;

    private IntCode _intCode;
    private int _output;

    public Amplifier(string intCodeMemory) {
        _intCode = new IntCode(intCodeMemory);
        _intCode.OnOutput += (o => _output = o);
    }

    // Resets and begins the intCode, passing the phase setting as the first input
    public void Begin(int phase) {
        _intCode.Reset();
        _intCode.Begin();
        _intCode.Input(phase);
    }

    // Passes the next input, which triggers an output, and return the output
    public int Step(int input) {
         _intCode.Input(input);
         return _output;
    }
}
