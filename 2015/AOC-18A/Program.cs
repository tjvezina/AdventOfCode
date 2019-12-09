using System;

public static class Program {
    private static void Main(string[] args) {
        LightBoard board = new LightBoard();

        for (int i = 0; i < 100; ++i) {
            board.Update();
        }

        Console.WriteLine("Lights on: " + board.litCount);
    }
}
