using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    public class Reindeer {
        public int speed { get; }
        public int flyTime { get; }
        public int cycleTime { get; }

        public Reindeer(int speed, int flyTime, int restTime) {
            this.speed = speed;
            this.flyTime = flyTime;
            cycleTime = flyTime + restTime;
        }
    }

    private const int RACE_DURATION = 2503;

    private static List<Reindeer> _reindeer = new List<Reindeer>();
    
    private static void Main(string[] args) {
        Load();
        RunRace();
    }

    private static void Load() {
        string[] input = File.ReadAllLines("input.txt");

        foreach (string data in input) {
            string[] parts = data.Split(' ');
            int speed = int.Parse(parts[3]);
            int flyTime = int.Parse(parts[6]);
            int restTime = int.Parse(parts[13]);

            _reindeer.Add(new Reindeer(speed, flyTime, restTime));
        }
    }

    private static void RunRace() {
        List<int> distances = new List<int>(new int[_reindeer.Count]);
        List<int> scores = new List<int>(new int[_reindeer.Count]);

        for (int t = 1; t <= RACE_DURATION; ++t) {
            // Calculate distances for the next second of the race
            for (int i = 0; i < _reindeer.Count; ++i) {
                Reindeer r = _reindeer[i];
                if ((t - 1) % r.cycleTime < r.flyTime) {
                    distances[i] += r.speed;
                }
            }

            // Award leaders 1 point
            int lead = distances.Max();
            for (int i = 0; i < distances.Count; ++i) {
                if (distances[i] == lead) {
                    ++scores[i];
                }
            }
        }

        Console.WriteLine("Winning score: " + scores.Max());
    }
}
