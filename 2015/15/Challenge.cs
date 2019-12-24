using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
     public class Challenge : BaseChallenge {
        public override void InitPart1() {
            Recipe.Init(inputSet);
        }

        public override string part1Answer => "21367368";
        public override (string, object) SolvePart1() {
            Recipe recipe = new RecipePart1();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
        
        public override string part2Answer => "1766400";
        public override (string, object) SolvePart2() {
            Recipe recipe = new RecipePart2();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
    }
}
