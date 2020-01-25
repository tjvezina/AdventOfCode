using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public abstract class BaseChallenge {
        public int year { get; }
        public int day { get; }

        public abstract string part1ExpectedAnswer { get; }
        public abstract string part2ExpectedAnswer { get; }

        public virtual CoordSystem? coordSystem => null;

        protected IReadOnlyList<string> inputList { get; }

        protected BaseChallenge() {
            Match match = Regex.Match(GetType().FullName, @"AdventOfCode\.Year(\d{4})\.Day(\d{2})\.Challenge");
            Debug.Assert(match.Success, $"Challenge type name doesn't match expected pattern: {GetType().FullName}");
            year = int.Parse(match.Groups[1].Value);
            day = int.Parse(match.Groups[2].Value);

            string inputFilePath = GetFilePath("input.txt");
            if (File.Exists(inputFilePath)) {
                inputList = Array.AsReadOnly(File.ReadAllLines(inputFilePath));
            }

            CoordUtil.system = coordSystem;
        }

        protected string GetFilePath(string fileName) => $"{this.GetPath()}/{fileName}";
        protected string[] LoadFileLines(string fileName) => File.ReadAllLines(GetFilePath(fileName));
        protected string LoadFile(string fileName) => File.ReadAllText(GetFilePath(fileName));

        public virtual void Reset() { }

        public abstract (string message, object answer) SolvePart1();
        public abstract (string message, object answer) SolvePart2();
    }
}
