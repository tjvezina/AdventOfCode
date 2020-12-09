namespace AdventOfCode.Year2015.Day15
{
    public class Ingredient
    {
        public const int PropertyCount = 4;

        public string name { get; }
        public int[] properties { get; }
        public int calories { get; }

        public Ingredient(string name, int[] properties, int calories)
        {
            this.name = name;
            this.calories = calories;
            this.properties = new int[PropertyCount];
            properties.CopyTo(this.properties, 0);
        }
    }
}
