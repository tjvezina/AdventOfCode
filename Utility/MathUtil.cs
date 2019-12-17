using System;

namespace AdventOfCode {
    public static class MathUtil {
        public static int Mod(int value, int divisor) {
            return (divisor == 0 ? 0 : value - (divisor * (int)Math.Floor((double)value / divisor)));
        }
        
        public static int Wrap(int value, int a, int b) => Wrap(value, new Range(a, b));
        public static int Wrap(int value, Range range) => range.Wrap(value);

        // Lowest common multiple
        public static int LCM(int a, int b) => (a * b) / GCD(a, b);
        public static long LCM(long a, long b) => (a * b) / GCD(a, b);

        // Greatest common divisor
        public static int GCD(int a, int b) => (int)GCD((long)a, (long)b);
        public static long GCD(long a, long b) {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a < b) {
                long hold = a;
                a = b;
                b = hold;
            }

            while (b > 0) {
                long r = a % b;
                a = b;
                b = r;
            }

            return a;
        }
    }
}
