using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day15 {
     public class Challenge : BaseChallenge {
        private RepairBot _bot;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YUp;
        }

        public override string SolvePart1() {
            _bot = new RepairBot(input);
            return $"Oxygen system is {_bot.goalDist} steps away at {_bot.goalPos.Value}.";
        }
        
        public override string SolvePart2() {
            return $"It will take {_bot.timeToFill} minutes to fill the region with oxygen.";
        }
    }
}
