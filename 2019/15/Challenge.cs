using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day15 {
     public class Challenge : BaseChallenge {
        private RepairBot _bot;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YUp;
        }

        public override string part1Answer => "374";
        public override (string, object) SolvePart1() {
            _bot = new RepairBot(input);
            return ($"Oxygen system is {{0}} steps away at {_bot.goalPos.Value}.", _bot.goalDist);
        }
        
        public override string part2Answer => "482";
        public override (string, object) SolvePart2() {
            return ("It will take {0} minutes to fill the region with oxygen.", _bot.timeToFill);
        }
    }
}
