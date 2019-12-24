using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day15 {
    public abstract class Recipe {
        protected class Ingredient {
            public const int PROPERTY_COUNT = 4;

            public string name { get; }
            public int[] properties { get; }
            public int calories { get; }

            public Ingredient(string name, int[] properties, int calories) {
                this.name = name;
                this.calories = calories;
                this.properties = new int[PROPERTY_COUNT];
                properties.CopyTo(this.properties, 0);
            }
        }

        public const int QUANTITY = 100;
        public const int CALORIES = 500;

        public int bestScore { get; protected set; }

        protected static List<Ingredient> _ingredients = new List<Ingredient>();

        public static void Init(string[] input) {
            foreach (string data in input) {
                string[] parts = data.Split(':');
                string name = parts[0];

                string[] propData = parts[1].Split(',');
                int GetProperty(int index) => int.Parse(propData[index].Split(' ')[2]);

                int[] properties = new int[Ingredient.PROPERTY_COUNT];
                for (int i = 0; i < properties.Length; ++i) {
                    properties[i] = GetProperty(i);
                }

                int calories = GetProperty(4);

                _ingredients.Add(new Ingredient(name, properties, calories));
            }
        }

        public int[] _quantities = new int[_ingredients.Count];

        public int GetScore() => CalculateScore(_quantities);

        protected int CalculateScore(int[] quantities) {
            List<int> propScores = new List<int>(new int[Ingredient.PROPERTY_COUNT]);

            for (int p = 0; p < propScores.Count; ++p) {
                for (int i = 0; i < _ingredients.Count; i++) {
                    propScores[p] += (_ingredients[i].properties[p] * quantities[i]);
                }

                propScores[p] = Math.Max(0, propScores[p]);
            }

            return propScores.Aggregate((a, b) => a * b);
        }

        protected int CalculateCalories(int[] quantities) {
            int calories = 0;

            for (int i = 0; i < _ingredients.Count; ++i) {
                calories += (_ingredients[i].calories * quantities[i]);
            }

            return calories;
        }

        public abstract void MaximizeScore();
    }
}
