using System;

public static class Program {
    private const int DIGITS = 6;
    private const int RANGE_MIN = 146810;
    private const int RANGE_MAX = 612564;

    private static void Main(string[] args) {
        int matchCount = 0;
        for (int password = RANGE_MIN; password <= RANGE_MAX; ++password) {
            if (IsMatch(password)) {
                ++matchCount;
            }
        }

        Console.WriteLine(matchCount);
    }

    private static bool IsMatch(int password) {
        bool containsDouble = false;

        int lastDigit = password % 10;
        for (int i = 1; i < DIGITS; ++i) {
            password /= 10;
            int nextDigit = password % 10;
            
            if (nextDigit > lastDigit) return false;

            if (nextDigit == lastDigit) containsDouble = true;

            lastDigit = nextDigit;
        }

        return containsDouble;
    }
}
