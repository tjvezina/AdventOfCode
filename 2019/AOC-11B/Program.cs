using System;

public static class Program {
    private static void Main(string[] args) {
        PaintBot bot = new PaintBot();

        while (bot.Update());
    }
}
