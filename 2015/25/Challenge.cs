using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day25 {
    public class Challenge : BaseChallenge {
        private const long FirstCode = 20_151_125;
        private const long Factor = 252_533;
        private const long Mod = 33_554_393;

        private int _codeRow;
        private int _codeCol;

        public override void InitPart1() {
            Match match = Regex.Match(input, @"row (\d+), column (\d+)");
            _codeRow = int.Parse(match.Groups[1].Value);
            _codeCol = int.Parse(match.Groups[2].Value);
        }

        public override string part1Answer => "2650453";
        public override (string, object) SolvePart1() {
            int codeIndex = GetCodeIndex(_codeRow, _codeCol);

            long code = FirstCode;
            for (int i = 0; i < codeIndex; ++i) {
                code = (code * Factor) % Mod;
            }

            return ($"Code at ({_codeRow}, {_codeCol}): ", code);
        }
        
        public override string part2Answer => "Weather Machine";
        public override (string, object) SolvePart2() => (null, part2Answer);

        private int GetCodeIndex(int row, int col) {
            int diagonal = row + (col - 1);
            int prevRowsSum = ((diagonal - 1) * diagonal) / 2;
            return prevRowsSum + col - 1;
        }
    }
}
