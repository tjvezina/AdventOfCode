using System;

public static class Program {
    private static Recipe _recipe;

    private static void Main(string[] args) {
        _recipe = new Recipe();
        _recipe.MaximizeScore();
    }
}
