using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public abstract class BaseChallenge {
        public int year { get; }
        public int day { get; }

        public abstract string part1Answer { get; }
        public abstract string part2Answer { get; }

        protected string[] inputSet { get; }
        protected string input { get; }

        protected BaseChallenge() {
            Match match = Regex.Match(GetType().FullName, @"AdventOfCode\.Year(\d{4})\.Day(\d{2})\.Challenge");
            Debug.Assert(match.Success, $"Challenge type name doesn't match expected pattern: {GetType().FullName}");
            year = int.Parse(match.Groups[1].Value);
            day = int.Parse(match.Groups[2].Value);

            string inputFilePath = GetFilePath("input.txt");
            if (File.Exists(inputFilePath)) {
                inputSet = File.ReadAllLines(inputFilePath);
                input = inputSet[0];
            }
        }

        protected string GetFilePath(string fileName) => $"{year}/{day:00}/{fileName}";
        protected string[] LoadFileLines(string fileName) => File.ReadAllLines(GetFilePath(fileName));
        protected string LoadFile(string fileName) => File.ReadAllText(GetFilePath(fileName));

        public virtual void InitPart1() { }
        public abstract (string format, object answer) SolvePart1();

        public virtual void InitPart2() { }
        public abstract (string format, object answer) SolvePart2();
    }
}
