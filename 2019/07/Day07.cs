using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day07 : Challenge {
        private AmplifierManager _ampManager;

        private void Init(string input) {
            _ampManager = new AmplifierManager(input);
        }

        protected override string SolvePart1() {
            return TestAllPermutations(Enumerable.Range(0, AmplifierManager.AMP_COUNT).ToArray());
        }
        
        protected override string SolvePart2() {
            return TestAllPermutations(Enumerable.Range(5, AmplifierManager.AMP_COUNT).ToArray());
        }

        private string TestAllPermutations(int[] _phase) {
            int[] bestPhase = new int[_phase.Length];
            int bestOutput = 0;

            do {
                int output = _ampManager.Execute(_phase);

                if (output > bestOutput) {
                    bestOutput = output;
                    _phase.CopyTo(bestPhase, 0);
                }
            } while (DataUtil.NextPermutation(_phase));

            string bestPhaseStr = bestPhase.Select(p => p.ToString()).Aggregate((a, b) => $"{a},{b}");
            return $"Best output: {bestOutput} ({bestPhaseStr})";
        }
    }
}
