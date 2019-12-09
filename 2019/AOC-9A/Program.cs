using System;
using System.IO;

public static class Program {
    private static void Main(string[] args) {
        IntCode intCode = new IntCode(File.ReadAllLines("input.txt")[0]);
        intCode.OnOutput += (o => Console.WriteLine("Output: " + o));
        intCode.Begin();

        while (intCode.state == IntCode.State.Waiting) {
            Console.Write("Input : ");
            intCode.Input(long.Parse(Console.ReadLine()));
        }
    }
}
