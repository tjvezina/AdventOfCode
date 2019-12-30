// #define MANUAL_SOLVE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2019.Day25 {
    public class Challenge : BaseChallenge {
        public override string part1Answer => "8912902";
        public override (string, object) SolvePart1() {
            Droid droid = new Droid(input);

#if MANUAL_SOLVE
            while (!droid.isComplete) {
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
        
        public override string part2Answer => "The Sun";
        public override (string, object) SolvePart2() {
            return (null, "The Sun");
        }
    }
}
