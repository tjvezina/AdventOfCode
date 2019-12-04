using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program {
    static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        List<int> massList = new List<int>(input.Select(i => int.Parse(i)));

        Console.WriteLine("Total fuel cost: " + massList.Select(GetFuelCost).Sum());
    }

    private static int GetFuelCost(int mass) {
        int fuel = Math.Max(0, mass / 3 - 2);

        if (fuel > 0)
        {
            fuel += GetFuelCost(fuel);
        }

        return fuel;
    }
}
