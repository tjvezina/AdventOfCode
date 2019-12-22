using System;
using System.IO;

namespace AdventOfCode {
    public static class Program {
        // Set to a specific year while working on puzzles from that year, or null for current year
        private static readonly int? ACTIVE_YEAR = null;
        private static int GetDefaultYear() => ACTIVE_YEAR ?? DateTime.Now.Year - (DateTime.Now.Month < 12 ? 1 : 0);
        private static int GetDefaultDay() => (DateTime.Now.Month == 12 ? DateTime.Now.Day : 25);

        private static void Main(string[] args) {
            if (args.Length == 1 && args[0] == "testall") {
                TestAll();
                return;
            }

            Type type = args.Length switch {
                0 => GetMostRecentChallenge(),
                1 => ParseArgs(args[0]),
                _ => throw new Exception("Too many args, 0 or 1 expected")
            };

            if (type != null) {
                ChallengeManager.Run(type);
            }
        }

        private static Type ParseArgs(string data) {
            int year = 0;
            int day = 0;
            
            string[] parts = data.Split('.');

            if (parts.Length == 1) {
                year = GetDefaultYear();
                int.TryParse(parts[0], out day);
            } else {
                int.TryParse(parts[0], out year);
                int.TryParse(parts[1], out day);
            }

            Type type = ChallengeManager.GetType(year, day);

            if (type == null) {
                Console.WriteLine($"Challenge not found for {year}.{day}");
            }

            return type;
        }

        private static Type GetMostRecentChallenge() {
            int year = GetDefaultYear();

            for (int day = GetDefaultDay(); day >= 1; --day) {
                Type type = ChallengeManager.GetType(year, day);
                if (type != null) return type;
            }

            Console.WriteLine($"No challenges found in {year}");
            return null;
        }

        private static void TestAll() {
            for (int year = 2015; year <= DateTime.Now.Year; ++year) {
                for (int day = 1; day <= 25; ++day) {
                    Type type = ChallengeManager.GetType(year, day);
                    if (type != null) {
                        ChallengeManager.Run(type, test:true);
                    }
                }
            }
        }
    }
}
