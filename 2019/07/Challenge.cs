using System.Linq;

namespace AdventOfCode.Year2019.Day07 {
     public class Challenge : BaseChallenge {
        private readonly AmplifierManager _ampManager;

        public Challenge() => _ampManager = new AmplifierManager(inputList[0]);

        public override string part1ExpectedAnswer => "20413";
        public override (string message, object answer) SolvePart1() {
            return TestAllPermutations(Enumerable.Range(0, AmplifierManager.AmpCount).ToArray());
        }
        
        public override string part2ExpectedAnswer => "3321777";
        public override (string message, object answer) SolvePart2() {
            return TestAllPermutations(Enumerable.Range(5, AmplifierManager.AmpCount).ToArray());
        }

        private (string format, int bestOutput) TestAllPermutations(int[] _phase) {
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
            return ($"Best output: {{0}} ({bestPhaseStr})", bestOutput);
        }
    }
}
