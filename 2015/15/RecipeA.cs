using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
    public class RecipePart1 : Recipe {
        public RecipePart1() {
            for (int i = 0; i < _quantities.Length; ++i) {
                _quantities[i] = QUANTITY / _ingredients.Count;
            }
            _quantities[0] += QUANTITY % _ingredients.Count; // Drop leftovers in the first ingredient
        }

        public override void MaximizeScore() {
            while (TryImprove()) {
                bestScore = CalculateScore(_quantities);
                string ingredients = _quantities.Select(i => $"{i}").Aggregate((a, b) => $"{a} {b}");
                Console.WriteLine($"Improved: {ingredients} = {bestScore}");
            }

            Console.WriteLine("Unable to further improve recipe.");
        }

        private bool TryImprove() {
            int baseScore = GetScore();
            int[] nextRecipe = new int[_ingredients.Count];
            
            void Reset() => _quantities.CopyTo(nextRecipe, 0);

            for (int i = 0; i < _quantities.Length; ++i) {
                for (int j = 0; j < _quantities.Length; ++j) {
                    if (i == j) continue;

                    Reset();
                    
                    if (nextRecipe[i] == 100 || nextRecipe[j] == 0) continue;

                    ++nextRecipe[i];
                    --nextRecipe[j];

                    int score = CalculateScore(nextRecipe);
                    if (score > baseScore) {
                        nextRecipe.CopyTo(_quantities, 0);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
