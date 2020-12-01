namespace AdventOfCode.Year2015.Day15
{
     public class Challenge : BaseChallenge
     {
        public Challenge() => Recipe.Init(inputList);

        public override string part1ExpectedAnswer => "21367368";
        public override (string message, object answer) SolvePart1()
        {
            Recipe recipe = new RecipePart1();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
        
        public override string part2ExpectedAnswer => "1766400";
        public override (string message, object answer) SolvePart2()
        {
            Recipe recipe = new RecipePart2();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
    }
}
