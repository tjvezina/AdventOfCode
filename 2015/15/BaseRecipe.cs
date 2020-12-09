using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day15
{
    public abstract class BaseRecipe
    {
        protected const int Quantity = 100;
        protected const int Calories = 500;

        protected static readonly List<Ingredient> Ingredients = new List<Ingredient>();

        public static void Init(IEnumerable<string> input)
        {
            foreach (string data in input)
            {
                string[] parts = data.Split(':');
                string name = parts[0];

                string[] propData = parts[1].Split(',');
                int GetProperty(int index) => int.Parse(propData[index].Split(' ')[2]);

                int[] properties = new int[Ingredient.PropertyCount];
                for (int i = 0; i < properties.Length; i++)
                {
                    properties[i] = GetProperty(i);
                }

                int calories = GetProperty(4);

                Ingredients.Add(new Ingredient(name, properties, calories));
            }
        }

        public int bestScore { get; protected set; }

        public abstract void MaximizeScore();

        protected int CalculateScore(int[] quantities)
        {
            List<int> propScores = new List<int>(new int[Ingredient.PropertyCount]);

            for (int p = 0; p < propScores.Count; p++)
            {
                for (int i = 0; i < Ingredients.Count; i++)
                {
                    propScores[p] += (Ingredients[i].properties[p] * quantities[i]);
                }

                propScores[p] = Math.Max(0, propScores[p]);
            }

            return propScores.Aggregate((a, b) => a * b);
        }

        protected int CalculateCalories(int[] quantities)
        {
            return Ingredients.Select((ingredient, i) => (ingredient.calories * quantities[i])).Sum();
        }
    }
}
