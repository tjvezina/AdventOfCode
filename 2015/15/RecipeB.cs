using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
    public class RecipeB : AbstractRecipe {
        public override void MaximizeScore() {
            int[] recipe = new int[_ingredients.Count];

            int bestScore = int.MinValue;

            void RecipeLoop(int index = 0, int max = QUANTITY) {
                if (index == recipe.Length - 1) {
                    recipe[index] = max; // Use up remaining ingredients
                    if (CalculateCalories(recipe) == CALORIES) {
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
