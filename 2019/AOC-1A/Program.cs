using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program {
    static void Main(string[] args) {
        int GetFuelCost(int mass) => (int)Math.Max(0, mass / 3 - 2);

        string[] input = File.ReadAllLines("input.txt");

        List<int> massList = new List<int>(input.Select(i => int.Parse(i)));

        Console.WriteLine("Total fuel cost: " + massList.Select(GetFuelCost).Sum());
    }
}
