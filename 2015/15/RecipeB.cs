using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
    public class RecipePart2 : Recipe {
        public override void MaximizeScore() {
            int[] recipe = new int[_ingredients.Count];

            bestScore = 0;

            void RecipeLoop(int index = 0, int max = Quantity) {
                if (index == recipe.Length - 1) {
                    recipe[index] = max; // Use up remaining ingredients
                    if (CalculateCalories(recipe) == Calories) {
                        int score = CalculateScore(recipe);
                        if (bestScore < score) {
                            bestScore = score;
                            recipe.CopyTo(_quantities, 0);
                        }
                    }
                    return;
                }

                for (int i = max; i >= 0; --i) {
                    recipe[index] = i;
                    RecipeLoop(index + 1, max - i);
                }
            }

            RecipeLoop();

            string ingredients = _quantities.Select(i => $"{i}").Aggregate((a, b) => $"{a} {b}");
            Console.WriteLine($"Optimized: {ingredients} = {bestScore}");
        }
    }
}
