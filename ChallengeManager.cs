using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode {
    public static class ChallengeManager {
        private enum ChallengePart { Part1, Part2 }

        private static readonly Type ChallengeType = typeof(BaseChallenge);
        private static readonly Dictionary<ChallengePart, MethodInfo> InitMethods = new Dictionary<ChallengePart, MethodInfo> {
            { ChallengePart.Part1, ChallengeType.GetMethod(nameof(BaseChallenge.InitPart1)) },
            { ChallengePart.Part2, ChallengeType.GetMethod(nameof(BaseChallenge.InitPart2)) }
        };
        private static readonly Dictionary<ChallengePart, MethodInfo> SolveMethods = new Dictionary<ChallengePart, MethodInfo> {
            { ChallengePart.Part1, ChallengeType.GetMethod(nameof(BaseChallenge.SolvePart1)) },
            { ChallengePart.Part2, ChallengeType.GetMethod(nameof(BaseChallenge.SolvePart2)) }
        };
        private static readonly Dictionary<ChallengePart, PropertyInfo> AnswerProps = new Dictionary<ChallengePart, PropertyInfo> {
            { ChallengePart.Part1, ChallengeType.GetProperty(nameof(BaseChallenge.part1Answer)) },
            { ChallengePart.Part2, ChallengeType.GetProperty(nameof(BaseChallenge.part2Answer)) }
        };

        private static Stopwatch _stopwatch = new Stopwatch();

        public static Type GetType(int year, int day) => Type.GetType($"AdventOfCode.Year{year}.Day{day:00}.Challenge");

        public static void Run(Type type) {
            BaseChallenge challenge = (BaseChallenge)type.GetConstructor(Type.EmptyTypes).Invoke(null);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" <<< Advent of Code {challenge.year} Day {challenge.day} >>> ");
            Console.ForegroundColor = ConsoleColor.Gray;

            RunPart(challenge, ChallengePart.Part1);
            RunPart(challenge, ChallengePart.Part2);

            Console.WriteLine();
        }

        private static void RunPart(BaseChallenge challenge, ChallengePart part) {
            DelayedWriter partWriter = new DelayedWriter();
            Console.SetOut(partWriter);
            (string format, string answer, string expected) = Execute(challenge, part);
            ConsoleUtil.RestoreDefaultOutput();

            Console.Write($"[Part {(part == ChallengePart.Part1 ? 1 : 2)}]");
            Console.ResetColor();
            Console.Write(" ");

            string answerStr = string.Empty;
            string[] formatParts = (format ?? string.Empty).Split("{0}");
            if (formatParts.Length > 0) {
                Console.Write(formatParts[0]);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(answer);
            Console.ResetColor();
            if (formatParts.Length > 1) {
                Console.Write(formatParts[1]);
            }
            Console.WriteLine();
            
            if (!partWriter.isEmpty) {
                partWriter.Flush();
                Console.WriteLine();
            }
        }

        public static void Test(Type type) {
            BaseChallenge challenge = (BaseChallenge)type.GetConstructor(Type.EmptyTypes).Invoke(null);

            TestPart(challenge, ChallengePart.Part1);
            TestPart(challenge, ChallengePart.Part2);
        }

        private static void TestPart(BaseChallenge challenge, ChallengePart part) {
            Console.SetOut(new StringWriter());
            (string format, string answer, string expected) = Execute(challenge, part, fullStackTrace:false);
            ConsoleUtil.RestoreDefaultOutput();

            Console.Write($"[{challenge.day:00} {(part == ChallengePart.Part1 ? 'A' : 'B')}]");
            Console.ResetColor();
            Console.Write(" ");

            WriteBenchmark();

            Console.ResetColor();
            Console.WriteLine(answer);
        }

        private static (string format, string answer, string expected) Execute(
            BaseChallenge challenge,
            ChallengePart part,
            bool fullStackTrace = true
        ) {
            (string format, string answer) = (null, null);
            string expected = (string)AnswerProps[part].GetValue(challenge);

            try {
                _stopwatch.Restart();
                InitMethods[part].Invoke(challenge, null);
                ValueTuple<string, object> result = (ValueTuple<string, object>)SolveMethods[part].Invoke(challenge, null);
                _stopwatch.Stop();
                format = result.Item1;
                answer = result.Item2?.ToString();

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                if (!string.IsNullOrEmpty(expected)) {
                    Console.ForegroundColor = (answer == expected ? ConsoleColor.Green : ConsoleColor.Red);
                } else if (string.IsNullOrEmpty(answer)) {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    _stopwatch.Reset();
                }
            } catch (TargetInvocationException ex) {
                _stopwatch.Reset();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.DarkRed;
                format = ex.InnerException.Message;
                if (fullStackTrace) {
                    format += "\n" + FormatStackTrace(ex.InnerException.StackTrace);
                }
            }

            return (format, answer, expected);
        }

        private static void WriteBenchmark() {
            if (_stopwatch.ElapsedTicks == 0) return;

            double elapsed = _stopwatch.Elapsed.TotalSeconds;

            string elapsedStr = elapsed switch {
                var e when e < 10   => $"{elapsed:0.000}",
                var e when e < 100  => $"{elapsed:00.00}",
                var e when e < 1000 => $"{elapsed:000.0}",
                _                   => ">1000"
            };

            Console.ForegroundColor = elapsed switch {
                var e when e >= 5.0 => ConsoleColor.DarkRed,
                var e when e >= 1.0 => ConsoleColor.DarkYellow,
                _                   => ConsoleColor.DarkGray
            };

            Console.Write($"({elapsedStr}s) ");
        }

        private static string FormatStackTrace(string stackTrace) {
            string[] lines = stackTrace.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; ++i) {
                Match match = Regex.Match(lines[i], @" in (.*:line \d+)");
                lines[i] = match.Success ? match.Groups[1].Value : lines[i];
            }
            return "Stack trace:\n" + lines.Where(l => !string.IsNullOrWhiteSpace(l)).Aggregate((a, b) => $"{a}\n{b}");
        }
    }
}
