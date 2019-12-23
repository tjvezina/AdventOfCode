using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
     public class Challenge : BaseChallenge {
        public override void InitPart1() {
            AbstractRecipe.Init(inputSet);
        }

        public override string SolvePart1() {
            new RecipeA().MaximizeScore();
            return null;
        }
        
        public override string SolvePart2() {
            new RecipeB().MaximizeScore();
            return null;
        }
    }
}
