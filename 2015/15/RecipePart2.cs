using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day15
{
    public class RecipePart2 : BaseRecipe
    {
        public override void MaximizeScore()
        {
            int[] recipe = new int[Ingredients.Count];
            int[] bestRecipe = new int[Ingredients.Count];

            bestScore = 0;

            RecipeLoop();

            Console.WriteLine($"Optimized: {bestScore}\n" +
                bestRecipe.Select((q, i) => $"- {q} {Ingredients[i].name}").Aggregate((a, b) => $"{a}\n{b}")
            );

            void RecipeLoop(int index = 0, int max = Quantity)
            {
                if (index == recipe.Length - 1)
                {
                    recipe[index] = max; // Use up remaining ingredients
                    if (CalculateCalories(recipe) == Calories)
                    {
                        int score = CalculateScore(recipe);
                        if (bestScore < score)
                        {
                            bestScore = score;
                            recipe.CopyTo(bestRecipe, 0);
                        }
                    }
                    return;
                }

                for (int i = max; i >= 0; i--)
                {
                    recipe[index] = i;
                    RecipeLoop(index + 1, max - i);
                }
            }
        }
    }
}
