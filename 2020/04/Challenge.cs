using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day04
{
    public class Challenge : BaseChallenge
    {
        private readonly List<Passport> _passportList;

        public Challenge()
        {
            _passportList = LoadFile("input.txt").Split("\n\n").Select(Passport.Parse).ToList();
        }

        public override string part1ExpectedAnswer => "226";
        public override (string message, object answer) SolvePart1()
        {
            int validCount = _passportList.Count(x => x.ValidateFields());

            return ($"{{0}} out of {_passportList.Count} passports have the required fields", validCount);
        }
        
        public override string part2ExpectedAnswer => "160";
        public override (string message, object answer) SolvePart2()
        {
            int validCount = _passportList.Count(x => x.ValidateFields() && x.ValidateData());

            return ($"{{0}} out of {_passportList.Count} passports have valid data", validCount);
        }
    }
}
