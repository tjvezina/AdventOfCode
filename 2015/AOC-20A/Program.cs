using System;

public static class Program {
    private const int TARGET = 29000000;

    private static void Main(string[] args) {
        for (int i = 1; ; ++i) {
            if (CountPresents(i) >= TARGET) {
                Console.WriteLine("House " + i);
                break;
            }
        }
    }

    private static int CountPresents(int house) {
        int presents = 0;
        for (int i = 1; i <= Math.Sqrt(house); ++i) {
            if (house % i == 0) {
                presents += i;
                if (house / i != i) {
                    presents += house / i;
                }
            }
        }
        return presents * 10;
    }
}
