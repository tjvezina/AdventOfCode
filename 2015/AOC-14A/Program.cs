using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        List<Reindeer> reindeer = new List<Reindeer>();

        foreach (string data in input) {
            string[] parts = data.Split(' ');
            int speed = int.Parse(parts[3]);
            int flyTime = int.Parse(parts[6]);
            int restTime = int.Parse(parts[13]);

            reindeer.Add(new Reindeer(speed, flyTime, restTime));
        }

        Console.WriteLine(reindeer.Max(r => r.GetDistance(2503)));
    }
}
