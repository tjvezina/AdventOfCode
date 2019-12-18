using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day15 : Challenge {
        private RepairBot _bot;

        private void Init(string input) {
            SpaceUtil.system = CoordSystem.YUp;
            _bot = new RepairBot(input);
        }

        protected override string SolvePart1() {
            return $"Oxygen system is {_bot.goalDist} steps away at {_bot.goalPos.Value}.";
        }
        
        protected override string SolvePart2() {
            return $"It will take {_bot.timeToFill} minutes to fill the region with oxygen.";
        }
    }
}
