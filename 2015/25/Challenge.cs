using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day25
{
    public class Challenge : BaseChallenge
    {
        private const long FirstCode = 20_151_125;
        private const long Factor = 252_533;
        private const long Mod = 33_554_393;

        private readonly int _codeRow;
        private readonly int _codeCol;

        public Challenge()
        {
            Match match = Regex.Match(inputList[0], @"row (\d+), column (\d+)");
            _codeRow = int.Parse(match.Groups[1].Value);
            _codeCol = int.Parse(match.Groups[2].Value);
        }

        public override object part1ExpectedAnswer => 2650453;
        public override (string message, object answer) SolvePart1()
        {
            int codeIndex = GetCodeIndex(_codeRow, _codeCol);

            long code = FirstCode;
            for (int i = 0; i < codeIndex; i++)
            {
                code = (code * Factor) % Mod;
            }

            return ($"Code at ({_codeRow}, {_codeCol}): ", code);
        }
        
        public override object part2ExpectedAnswer => "Weather Machine";
        public override (string message, object answer) SolvePart2() => (null, part2ExpectedAnswer);

        private int GetCodeIndex(int row, int col)
        {
            int diagonal = row + (col - 1);
            int prevRowsSum = ((diagonal - 1) * diagonal) / 2;
            return prevRowsSum + col - 1;
        }
    }
}
