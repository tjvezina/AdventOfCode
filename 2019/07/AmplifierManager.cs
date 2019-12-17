using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class AmplifierManager {
        public const int AMP_COUNT = 5;

        private List<Amplifier> _amps = new List<Amplifier>();

        public AmplifierManager(string intCodeMemory) {
            for (int i = 0; i < AMP_COUNT; ++i) {
                _amps.Add(new Amplifier(intCodeMemory));
            }
        }

        public int Execute(int[] phaseSettings) {
            for (int i = 0; i < _amps.Count; ++i) {
                _amps[i].Begin(phaseSettings[i]);
            }

            int nextInput = 0;

            while (!_amps[0].IsComplete) {
                _amps.ForEach(amp => nextInput = amp.Step(nextInput));
            }

            return nextInput;
        }
    }
}
