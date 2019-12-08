using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private const int AMP_COUNT = 5;

    private static int[] _phase;
    private static int[] _bestPhase;
    private static int _bestOutput;

    private static List<Amplifier> _amps = new List<Amplifier>();
    private static int _ampIndex = 0;

    private static void Main(string[] args) {
        Init();
        BeginNextCycle();
    }

    private static void Init() {
        string intCodeInput = File.ReadAllLines("input.txt")[0];

        for (int i = 0; i < AMP_COUNT; ++i) {
            Amplifier amp = new Amplifier(intCodeInput);
            amp.OnOutput += ExecuteNextAmp;
            _amps.Add(amp);
        }

        _phase = new int[AMP_COUNT];
        _bestPhase = new int[AMP_COUNT];

        for (int i = 0; i < _phase.Length; ++i) {
            _phase[i] = i;
        }
    }

    private static void BeginNextCycle() {
        _ampIndex = 0;
        ExecuteNextAmp(0);
    }

    private static void ExecuteNextAmp(int output) {
        if (_ampIndex == AMP_COUNT) {
            EndCycle(output);
            return;
        }

        Amplifier amp = _amps[_ampIndex];
        int phaseSetting = _phase[_ampIndex];
        ++_ampIndex;

        amp.Execute(phaseSetting, output);
    }

    private static void EndCycle(int output) {
        if (output > _bestOutput) {
            _bestOutput = output;
            for (int i = 0; i < _phase.Length; ++i) {
                _bestPhase[i] = _phase[i];
            }
        }

        if (NextPermutation()) {
            BeginNextCycle();
        } else {
            HandleAllCyclesComplete();
        }
    }

    private static void HandleAllCyclesComplete() {
        string bestPhase = _bestPhase.Select(p => p.ToString()).Aggregate((a, b) => $"{a},{b}");

        Console.WriteLine($"Best output: {_bestOutput} ({bestPhase})");
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
