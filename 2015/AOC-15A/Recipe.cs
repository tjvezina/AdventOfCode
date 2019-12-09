using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Recipe {
    private class Ingredient {
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

    public const int MAX_QUANTITY = 100;

    private static List<Ingredient> _ingredients = new List<Ingredient>();

    static Recipe() {
        string[] input = File.ReadAllLines("input.txt");

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

    public int[] _quantities;

    public Recipe() {
        _quantities = new int[_ingredients.Count];

        for (int i = 0; i < _quantities.Length; ++i) {
            _quantities[i] = MAX_QUANTITY / _ingredients.Count;
        }
    }

    public void MaximizeScore() {
        while (TryImprove()) {
            int score = CalculateScore(_quantities);
            string ingredients = _quantities.Select(i => $"{i}").Aggregate((a, b) => $"{a} {b}");
            Console.WriteLine($"Improved: {ingredients} = {score}");
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

    public int GetScore() => Math.Max(0, CalculateScore(_quantities));

    private int CalculateScore(int[] quantities) {
        List<int> propScores = new List<int>(new int[Ingredient.PROPERTY_COUNT]);

        for (int p = 0; p < propScores.Count; ++p) {
            for (int i = 0; i < _ingredients.Count; i++) {
                propScores[p] += (_ingredients[i].properties[p] * quantities[i]);
            }
        }

        return propScores.Aggregate((a, b) => a * b);
    }
}
