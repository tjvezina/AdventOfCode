using System;
using System.Linq;

namespace AdventOfCode {
    public static class Program {
        // Set to a specific year while working on puzzles from that year
        public static readonly int DEFAULT_YEAR = DateTime.Now.Year;

        private static void Main(string[] args) {
            Type type;

            if (args.Length > 0) {
                type = ParseChallengeData(args[0]);
            } else {
                type = GetMostRecentChallenge();
            }
            
            if (type != null) {
                Challenge.Run(type);
            }
        }

        private static Type ParseChallengeData(string data) {
            int year = 0;
            int day = 0;
            
            string[] parts = data.Split('.');

            if (parts.Length == 1) {
                year = DEFAULT_YEAR;
                int.TryParse(parts[0], out day);
            } else {
                int.TryParse(parts[0], out year);
                int.TryParse(parts[1], out day);
            }

            Type type = Challenge.GetType(year, day);

            if (type == null) {
                Console.WriteLine($"Failed to find challenge for {year}.{day}");
            }

            return type;
        }

        private static Type GetMostRecentChallenge() {
            int year;
            int day;

            if (DateTime.Now.Month == 12) {
                year = DateTime.Now.Year;
                day = DateTime.Now.Day;
            }
            else {
                year = DateTime.Now.Year - 1;
                day = 25;
            }

            Type type = null;
            Enumerable.Range(1, day).Reverse().FirstOrDefault(d => (type = Challenge.GetType(year, d)) != null);

            if (type == null) {
                Console.WriteLine($"No challenges found in {year}");
            }

            return type;
        }
    }
}
