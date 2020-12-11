using System;
using System.Numerics;
using JetBrains.Annotations;

namespace AdventOfCode
{
    [PublicAPI]
    public static class MathUtil
    {
        public static int Mod(int value, int divisor)
        {
            return (divisor == 0 ? 0 : value - (divisor * (int)Math.Floor((decimal)value / divisor)));
        }
        public static long Mod(long value, long divisor)
        {
            return (divisor == 0 ? 0 : value - (divisor * (long)Math.Floor((decimal)value / divisor)));
        }
        
        public static int Wrap(int value, int a, int b) => Mod(value - a, b - a) + a;
        public static long Wrap(long value, long a, long b) => Mod(value - a, b - a) + a;

        public static int FloorTo(int value, int multiple) => (int)Math.Floor((decimal)value / multiple) * multiple;
        public static int RoundTo(int value, int multiple) => (int)Math.Round((decimal)value / multiple) * multiple;
        public static int CeilingTo(int value, int multiple) => (int)Math.Ceiling((decimal)value / multiple) * multiple;

        // Lowest common multiple
        public static int LCM(int a, int b) => (a * b) / GCD(a, b);
        public static long LCM(long a, long b) => (a * b) / GCD(a, b);

        // Greatest common divisor
        public static int GCD(int a, int b) => (int)GCD((long)a, b);
        public static long GCD(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            if (a < b) DataUtil.Swap(ref a, ref b);

            while (b > 0)
            {
                long r = a % b;
                a = b;
                b = r;
            }

            return a;
        }

        public static int Power(int b, int e) => (int)Power((long)b, e);
        public static long Power(long b, long e)
        {
            long result = 1;
            while (e > 0)
            {
                if ((e & 1) != 0)
                {
                    result *= b;
                }
                b *= b;
                e >>= 1;
            }
            return result;
        }

        public static int ModPower(int b, int e, int m) => (int)ModPower((BigInteger)b, e, m);
        public static long ModPower(long b, long e, long m) => (long)ModPower((BigInteger)b, e, m);
        public static BigInteger ModPower(BigInteger b, BigInteger e, BigInteger m)
        {
            BigInteger result = 1;
            while (e > 0)
            {
                if ((e & 1) != 0)
                {
                    result = (result * b) % m;
                }
                b = (b * b) % m;
                e >>= 1;
            }
            return result;
        }
    }
}
