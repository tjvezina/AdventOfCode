using System;
using System.IO;

public static class Program {
    private static void Main(string[] args) {
        IntCode intCode = new IntCode();
        intCode.Load(File.ReadAllLines("input.txt")[0]);
        intCode.Execute();
    }
}
