using System;
using System.IO;

class Program {
    private const int TARGET_OUTPUT = 19690720;

    private static IntCode intCode;

    private static void Main(string[] args) {
        intCode = new IntCode();
        intCode.Load(File.ReadAllLines("input.txt")[0]);

        for (int noun = 0; noun <= 99; ++noun) {
            for (int verb = 0; verb <= 99; ++verb) {
                intCode.Execute(noun, verb);

                if (intCode[0] == TARGET_OUTPUT) {
                    Console.WriteLine("Input found: " + (100 * noun + verb));
                    return;
                }

                intCode.Reset();
            }
        }

        Console.WriteLine("Failed to find input to produce target output.");
    }
}
