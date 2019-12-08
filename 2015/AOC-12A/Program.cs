using System;
using System.IO;
using System.Linq;
using TinyJSON;

public static class Program {
    private static void Main(string[] args) {
        Variant root = JSON.Load(File.ReadAllLines("input.txt")[0]);

        Console.WriteLine("Sum of all numbers: " + SumNumbers(root));
    }

    private static int SumNumbers(Variant variant) {
        if (variant is ProxyNumber number) {
            return (int)Convert.ChangeType(number, typeof(int));
        }

        if (variant is ProxyArray array) {
            return array.Sum(SumNumbers);
        }

        if (variant is ProxyObject obj) {
            return obj.Sum(pair => SumNumbers(pair.Value));
        }

        return 0;
    }
}
