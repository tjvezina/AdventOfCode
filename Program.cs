using System;

namespace AdventOfCode {
    /// <summary>
    /// USAGE
    /// dotnet run - run the most recent challenge in the active year
    /// dotnet run create - create folder structure and challenge file for the next incomplete challenge
    /// dotnet run test - run the most recent challenge in test mode to validate functionality
    /// dotnet run test all - run every challenge in test mode chronologically
    /// NOTES
    /// Most commands can take an optional date to specify a challenge, in any of these formats:
    /// dd       (ex. 5)        - December 5th of active year
    /// yyyy.dd  (ex. 2019.23)  - December 23rd 2019
    /// </summary>
    public static class Program {
        private const int FirstYear = 2015;
        private static Range AllYears => new Range(FirstYear, DateTime.Now.Year);
        private static Range AllDays => new Range(1, 25);

        // Set to a specific year while working on puzzles from that year, or null for current year
        private static readonly int? ActiveYear = 2020;
        private static int GetDefaultYear() => ActiveYear ?? DateTime.Now.Year - (DateTime.Now.Month < 12 ? 1 : 0);
        private static int GetDefaultDay() => (DateTime.Now.Month == 12 ? DateTime.Now.Day : 25);

        private static void Main(string[] args) {
            string arg0 = args.Length > 0 ? args[0] : null;
            string arg1 = args.Length > 1 ? args[1] : null;

            switch (arg0) {
                default:
                    RunMode(arg0);
                    break;
                case "create":
                    CreateNextChallenge(arg1);
                    break;
                case "test":
                    TestMode(arg1);
                    break;
            }
        }

        private static void CreateNextChallenge(string yearStr) {
            if (!int.TryParse(yearStr, out int year)) year = GetDefaultYear();

            int day = 1;
            while (ChallengeManager.GetType(year, day) != null) day++;

            if (day > 25) {
                Console.WriteLine($"All {year} challenges already exist");
                return;
            }

            ChallengeManager.Create(year, day);
        }

        private static void RunMode(string date) {
            Type type = (date == null ? GetMostRecentChallengeType() : GetChallengeTypeForDate(date));

            if (type != null) {
                ChallengeManager.Run(type);
            }
        }

        private static void TestMode(string date) {
            if (date == null) {
                TestDay(GetDefaultYear(), GetMostRecentChallengeDay());
                return;
            }

            if (date == "all") {
                TestAll();
                return;
            }

            string[] parts = date.Split('.');
            int year;
            int day;

            if (parts.Length == 1) {
                if (int.TryParse(parts[0], out year)) {
                    TestYear(year);
                }
                return;
            }

            if (int.TryParse(parts[0], out year) && int.TryParse(parts[1], out day)) {
                TestDay(year, day);
            }
        }

        private static Type GetChallengeTypeForDate(string date) {
            int year = 0;
            int day = 0;
            
            string[] parts = date.Split('.');

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

        private static Type GetMostRecentChallengeType() {
            int year = GetDefaultYear();
            for (int day = GetDefaultDay(); day >= 1; day--) {
                Type type = ChallengeManager.GetType(year, day);
                if (type != null) return type;
            }

            Console.WriteLine($"No challenges found in {year}");
            return null;
        }

        private static int GetMostRecentChallengeDay() {
            int year = GetDefaultYear();
            for (int day = GetDefaultDay(); day >= 1; day--) {
                Type type = ChallengeManager.GetType(year, day);
                if (type != null) return day;
            }

            Console.WriteLine($"No challenges found in {year}");
            return 0;
        }

        private static void TestAll() => TestRange(AllYears, AllDays);
        private static void TestYear(int year) => TestRange(new Range(year, year), AllDays);
        private static void TestDay(int year, int day) => TestRange(new Range(year, year), new Range(day, day));
        private static void TestRange(Range years, Range days) {
            bool wasYearHeaderDrawn;
            for (int year = years.min; year <= years.max; year++) {
                wasYearHeaderDrawn = false;
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int day = days.min; day <= days.max; day++) {
                    Type type = ChallengeManager.GetType(year, day);
                    if (type == null) continue;

                    if (!wasYearHeaderDrawn) {
                        wasYearHeaderDrawn = true;
                        Console.WriteLine($"-- {year} --");
                    }
                    
                    ChallengeManager.Test(type);
                }
            }
        }
    }
}
