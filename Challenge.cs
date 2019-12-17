using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public abstract class Challenge {
        public static Type GetType(int year, int day) {
            return Type.GetType($"{nameof(AdventOfCode)}.Year{year}.Day{day:00}");
        }

        public static void Run(Type type) {
            Challenge challenge = (Challenge)type.GetConstructor(Type.EmptyTypes).Invoke(null);

            Console.WriteLine(string.Empty);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" <<< Advent of Code {challenge.year} Day {challenge.day} >>> ");
            Console.ForegroundColor = ConsoleColor.Gray;

            challenge.Init();

            void RunPart(int part, Func<string> partFunc) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[Part {part}]");
                Console.ForegroundColor = ConsoleColor.Gray;
                string solution = partFunc.Invoke();
                if (!string.IsNullOrWhiteSpace(solution)) {
                    Console.WriteLine($"{solution}");
                }
                Console.WriteLine(string.Empty);
            }

            RunPart(1, challenge.SolvePart1);
            challenge.Reset();
            RunPart(2, challenge.SolvePart2);
        }

        public int year { get; }
        public int day { get; }

        protected Challenge() {
            string yearStr = GetType().Namespace?.Split('.').Last();
            string dayStr = GetType().Name;
            Debug.Assert(Regex.IsMatch(yearStr, @"Year20\d{2}"), $"Invalid namespace, expected 'Year20XX': {yearStr}");
            Debug.Assert(Regex.IsMatch(dayStr, @"Day\d{2}"), $"Invalid challenge type name, expected 'DayXX`: {dayStr}");
            year = int.Parse(yearStr.Substring(4));
            day = int.Parse(dayStr.Substring(3));
        }

        private void Init() {
            string inputPath = $"{year}/{day:00}/input.txt";
            string[] input = null;

            if (File.Exists(inputPath)) {
                input = File.ReadAllLines(inputPath);
            }

            MethodInfo initMethod = GetType().GetMethod(nameof(Init), BindingFlags.NonPublic | BindingFlags.Instance);

            if (initMethod == null) {
                Debug.Assert(input == null, "Input data found, but no Init function found to send it to!");
                return;
            }

            ParameterInfo[] initParams = initMethod.GetParameters();

            if (initParams.Length == 0) {
                initMethod.Invoke(this, null);
                return;
            }

            Debug.Assert(initParams.Length == 1, "Found Init function, but it takes too many parameters");

            if (initParams[0].ParameterType == typeof(string)) {
                initMethod.Invoke(this, new[] { input[0] });
                return;
            }

            if (initParams[0].ParameterType == typeof(string[])) {
                initMethod.Invoke(this, new[] { input });
                return;
            }

            throw new Exception($"Found Init function, but unknown parameter type: {initParams[0]}");
        }

        protected virtual void Reset() { }

        protected abstract string SolvePart1();
        protected abstract string SolvePart2();
    }
}
