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

        public override string part1Answer => "1771";
        public override (string, object) SolvePart1() {
            _paintBot.Run(firstTileIsWhite:false);
            return ("Tiles painted: ", _paintBot.paintedCount);
        }
        
        public override string part2Answer => "HGEHJHUZ";
        public override (string, object) SolvePart2() {
            _paintBot.Run(firstTileIsWhite:true);
            _paintBot.PrintResults();
            return ("Image text: ", "HGEHJHUZ"); // Explicitly return answer, until image-to-text is implemented :)
        }
    }
}
