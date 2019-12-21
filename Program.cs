using System;
using System.Linq;

namespace AdventOfCode {
    public static class Program {
        // Set to a specific year while working on puzzles from that year, or null for current year
        private static readonly int? ACTIVE_YEAR = null;
        private static int defaultYear {
            get {
                if (ACTIVE_YEAR.HasValue) return ACTIVE_YEAR.Value;
                return DateTime.Now.Year - (DateTime.Now.Month < 12 ? 1 : 0);
            }
        }
        private static int defaultDay => (DateTime.Now.Month == 12 ? DateTime.Now.Day : 25);

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
                year = defaultYear;
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
            int year = defaultYear;

            Type type = null;
            Enumerable.Range(1, defaultDay).Reverse().FirstOrDefault(d => (type = Challenge.GetType(year, d)) != null);

            if (type == null) {
                Console.WriteLine($"No challenges found in {year}");
            }

            return type;
        }
    }
}
