using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day18 {
     public class Challenge : BaseChallenge {
        public override string SolvePart1() {
            LightBoard board = new LightBoard(inputSet);

            for (int i = 0; i < 100; ++i) {
                board.Update();
            }

            return $"Lights on: {board.litCount}";
        }
        
        public override string SolvePart2() {
            LightBoard board = new LightBoard(inputSet, cornersAlwaysOn:true);

            for (int i = 0; i < 100; ++i) {
                board.Update();
            }

            return $"Lights on: {board.litCount}";
        }
    }
}
