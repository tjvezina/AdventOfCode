// #define MANUAL_SOLVE
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019.Day25
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 8912902;
        public override (string message, object answer) SolvePart1()
        {
            Droid droid = new Droid(inputList[0]);

#if MANUAL_SOLVE
            while (!droid.isComplete)
            {
                droid.ReadInput();
            }
#else
            // Collect all items and return to the security room
            droid.LoadSave(LoadFileLines("SaveGame1.txt"));
            // Try every combination of items until the correct weight is found
            droid.TestAllItemCombinations();
#endif

            Match match = Regex.Match(droid.lastOutput, @"\d+");

            return ("Airlock password: ", match.Value);
        }
        
        public override object part2ExpectedAnswer => "The Sun";
        public override (string message, object answer) SolvePart2() => (null, part2ExpectedAnswer);
    }
}
