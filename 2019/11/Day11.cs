using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day11 : Challenge {
        private PaintBot _paintBot;

        private void Init(string input) {
            _paintBot = new PaintBot(input);
        }

        protected override string SolvePart1() {
            _paintBot.Run(firstTileIsWhite:false);
            return $"Tiles painted: {_paintBot.paintedCount}";
        }
        
        protected override string SolvePart2() {
            _paintBot.Run(firstTileIsWhite:true);
            _paintBot.PrintResults();
            return null;
        }
    }
}
