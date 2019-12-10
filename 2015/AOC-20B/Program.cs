using System;

public static class Program {
    private const int TARGET = 29000000;

    private static void Main(string[] args) {
        CountPresents(10000);

        for (int i = 1; ; ++i) {
            if (CountPresents(i) >= TARGET) {
                Console.WriteLine("House " + i);
                break;
            }
        }
    }

    private static int CountPresents(int h) {
        const int M = 50; // Max houses/elf
        int p = 0;
        int min = (int)Math.Ceiling((double)h / M);
        for (int i = 1; i <= Math.Sqrt(h); ++i) {
            if (h % i == 0) {
                int j = h / i;
                if (i >= min) p += i;
                if (j >= min && j != i) p += j;
            }
        }
        return p * 11;
    }
}
