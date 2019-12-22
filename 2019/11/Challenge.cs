using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day11 {
     public class Challenge : BaseChallenge {
        private PaintBot _paintBot;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YUp;
            _paintBot = new PaintBot(input);
        }

        public override string SolvePart1() {
            _paintBot.Run(firstTileIsWhite:false);
            return $"Tiles painted: {_paintBot.paintedCount}";
        }
        
        public override string SolvePart2() {
            _paintBot.Run(firstTileIsWhite:true);
            _paintBot.PrintResults();
            return null;
        }
    }
}
