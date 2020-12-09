namespace AdventOfCode.Year2015.Day15
{
    public class Challenge : BaseChallenge
    {
        public Challenge() => BaseRecipe.Init(inputList);

        public override object part1ExpectedAnswer => 21367368;
        public override (string message, object answer) SolvePart1()
        {
            BaseRecipe recipe = new RecipePart1();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
        
        public override object part2ExpectedAnswer => 1766400;
        public override (string message, object answer) SolvePart2()
        {
            BaseRecipe recipe = new RecipePart2();
            recipe.MaximizeScore();
            return ("Best recipe score: ", recipe.bestScore);
        }
    }
}
