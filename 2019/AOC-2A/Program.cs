using System;
using System.IO;

class Program {
    private const int NOUN = 12;
    private const int VERB = 02;

    private static IntCode _intCode;

    private static void Main(string[] args) {
        _intCode = new IntCode();
        _intCode.Load(File.ReadAllLines("input.txt")[0]);
        _intCode.Execute(NOUN, VERB);

        Console.WriteLine(_intCode[0]);
    }
}
