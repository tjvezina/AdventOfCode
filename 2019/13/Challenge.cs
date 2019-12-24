using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day13 {
     public class Challenge : BaseChallenge {
        private string _intCodeMemory;

        public override void InitPart1() {
            _intCodeMemory = input;
        }

        public override string part1Answer => "452";
        public override (string, object) SolvePart1() {
            Arcade arcade = new Arcade(_intCodeMemory);
            return ("{0} blocks on screen", arcade.blockCount);
        }
        
        public override string part2Answer => "21415";
        public override (string, object) SolvePart2() {
            Arcade arcade = new Arcade(_intCodeMemory, insertQuarters:true);
            return ("Final score: ", arcade.score);
        }
    }
}
