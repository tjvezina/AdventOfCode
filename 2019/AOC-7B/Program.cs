using System;
using System.Linq;

public static class Program {
    private static int[] _phase;
    private static int[] _bestPhase;
    private static int _bestOutput;

    private static AmplifierManager _ampManager;

    private static void Main(string[] args) {
        Init();
        
        do {
            int output = _ampManager.Execute(_phase);

            if (output > _bestOutput) {
                _bestOutput = output;
                _phase.CopyTo(_bestPhase, 0);
            }
        } while (NextPermutation());

        string bestPhase = _bestPhase.Select(p => p.ToString()).Aggregate((a, b) => $"{a},{b}");
        Console.WriteLine($"Best output: {_bestOutput} ({bestPhase})");
    }

    private static void Init() {
        _ampManager = new AmplifierManager();

        _phase = new int[AmplifierManager.AMP_COUNT];
        _bestPhase = new int[AmplifierManager.AMP_COUNT];

        for (int i = 0; i < _phase.Length; ++i) {
            _phase[i] = 5 + i;
        }
    }

    private static bool NextPermutation() {
        // Find greatest index x, where p[x] < p[x+1]
        int x = -1;
        for (int i = _phase.Length - 2; i>= 0; --i) {
            if (_phase[i] < _phase[i+1]) {
                x = i;
                break;
            }
        }

        // Final permutation reached
        if (x == -1) {
            return false;
        }

        // Find greatest index y, where p[x] < p[y]
        int y = -1;
        for (int i = _phase.Length - 1; i >= x + 1; --i) {
            if (_phase[x] < _phase[i]) {
                y = i;
                break;
            }
        }
        
        // Swap p[x] and p[y]
        int hold = _phase[y];
        _phase[y] = _phase[x];
        _phase[x] = hold;

        // Reverse elements from p[x+1]..p[n]
        int[] reverse = new int[_phase.Length - 1 - x];
        for (int i = 0; i < reverse.Length; ++i) {
            reverse[i] = _phase[x + 1 + i];
        }
        for (int i = 0; i < reverse.Length; ++i) {
            _phase[_phase.Length - 1 - i] = reverse[i];
        }

        return true;
    }
}
