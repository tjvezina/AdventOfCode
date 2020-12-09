using System;
using System.Linq;

namespace AdventOfCode.Year2015.Day15
{
    public class RecipePart1 : BaseRecipe
    {
        private readonly int[] _quantities = new int[Ingredients.Count];

        public RecipePart1()
        {
            for (int i = 0; i < _quantities.Length; i++)
            {
                _quantities[i] = Quantity / Ingredients.Count;
            }
            _quantities[0] += Quantity % Ingredients.Count; // Drop leftovers in the first ingredient
        }

        public override void MaximizeScore()
        {
            while (TryImprove())
            {
                bestScore = CalculateScore(_quantities);
                string ingredients = _quantities.Select(i => $"{i}").Aggregate((a, b) => $"{a} {b}");
                Console.WriteLine($"Improved: {ingredients} = {bestScore}");
            }

            Console.WriteLine("Unable to further improve recipe:\n" +
                _quantities.Select((q, i) => $"- {q} {Ingredients[i].name}").Aggregate((a, b) => $"{a}\n{b}")
            );
        }

        private bool TryImprove()
        {
            int baseScore = CalculateScore(_quantities);
            int[] nextRecipe = new int[Ingredients.Count];

            for (int i = 0; i < _quantities.Length; i++)
            {
                for (int j = 0; j < _quantities.Length; j++)
                {
                    if (i == j) continue;

                    _quantities.CopyTo(nextRecipe, 0);
                    
                    if (nextRecipe[i] == 100 || nextRecipe[j] == 0) continue;

                    nextRecipe[i]++;
                    nextRecipe[j]--;

                    int score = CalculateScore(nextRecipe);
                    if (score > baseScore)
                    {
                        nextRecipe.CopyTo(_quantities, 0);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
