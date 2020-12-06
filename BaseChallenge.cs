using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public abstract class BaseChallenge
    {
        public int year { get; }
        public int day { get; }

        public abstract string part1ExpectedAnswer { get; }
        public abstract string part2ExpectedAnswer { get; }

        public virtual CoordSystem? coordSystem => null;

        protected string inputFile { get; }
        protected IReadOnlyList<string> inputList { get; }

        protected BaseChallenge()
        {
            Match match = Regex.Match(GetType().FullName ?? string.Empty, @"AdventOfCode\.Year(\d{4})\.Day(\d{2})\.Challenge");
            Debug.Assert(match.Success, $"Challenge type name doesn't match expected pattern: {GetType().FullName}");
            year = int.Parse(match.Groups[1].Value);
            day = int.Parse(match.Groups[2].Value);

            string inputFilePath = GetFilePath("input.txt");
            if (File.Exists(inputFilePath))
            {
                inputList = Array.AsReadOnly(File.ReadAllLines(inputFilePath));
                inputFile = inputList.Aggregate((a, b) => $"{a}\n{b}");
            }

            CoordUtil.system = coordSystem;
        }

        protected string GetFilePath(string fileName) => $"{this.GetPath()}/{fileName}";

        protected string LoadFile(string fileName)
        {
            string filePath = GetFilePath(fileName);
            return (File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty);
        }

        protected string[] LoadFileLines(string fileName)
        {
            string filePath = GetFilePath(fileName);
            return (File.Exists(filePath) ? File.ReadAllLines(filePath) : new string[0]);
        }

        protected void SaveFile(string fileName, string contents)
        {
            File.WriteAllText(GetFilePath(fileName), contents);
        }

        protected void SaveFile(string fileName, IEnumerable<string> lines)
        {
            File.WriteAllLines(GetFilePath(fileName), lines);
        }

        public virtual void Reset() { }

        public abstract (string message, object answer) SolvePart1();
        public abstract (string message, object answer) SolvePart2();
    }
}
