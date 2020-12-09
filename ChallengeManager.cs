using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public static class ChallengeManager
    {
        private enum ChallengePart { Part1 = 1, Part2 = 2 }

        private enum ResultStatus
        {
            Development, // No answer given; hasn't been started, or isn't complete yet
            Candidate,   // Answer given, expected answer unknown; to be submitted to AoC
            WrongAnswer, // Given answer does not match expected
            Success,     // Given answer matches expected
            Exception    // Unhandled exception during execution
        }

        private struct Results
        {
            public ResultStatus status;
            public string givenAnswer;
            public string message;

            public void SetStatusColor()
            {
                Console.ResetColor();
                if (status == ResultStatus.Exception)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                Console.ForegroundColor = status switch
                {
                    ResultStatus.Development => ConsoleColor.DarkGray,
                    ResultStatus.Candidate   => ConsoleColor.Cyan,
                    ResultStatus.WrongAnswer => ConsoleColor.Red,
                    ResultStatus.Success     => ConsoleColor.Green,
                    ResultStatus.Exception   => ConsoleColor.Black,
                    _ => throw new Exception($"Unhandled result status: {status}")
                };
            }
        }

        private static readonly Type ChallengeType = typeof(BaseChallenge);
        private static readonly MethodInfo ResetMethod = ChallengeType.GetMethod(nameof(BaseChallenge.Reset));
        private static readonly Dictionary<ChallengePart, MethodInfo> SolvePartMethods = new Dictionary<ChallengePart, MethodInfo>
        {
            [ChallengePart.Part1] = ChallengeType.GetMethod(nameof(BaseChallenge.SolvePart1)),
            [ChallengePart.Part2] = ChallengeType.GetMethod(nameof(BaseChallenge.SolvePart2))
        };
        private static readonly Dictionary<ChallengePart, PropertyInfo> ExpectedAnswerProps = new Dictionary<ChallengePart, PropertyInfo>
        {
            [ChallengePart.Part1] = ChallengeType.GetProperty(nameof(BaseChallenge.part1ExpectedAnswer)),
            [ChallengePart.Part2] = ChallengeType.GetProperty(nameof(BaseChallenge.part2ExpectedAnswer))
        };

        private static readonly Stopwatch Stopwatch = new Stopwatch();

        private static string GetPath(int year, int day) => $"{year}/{day:00}";
        public static string GetPath(this BaseChallenge challenge) => GetPath(challenge.year, challenge.day);

        public static Type GetType(int year, int day) => Type.GetType($"AdventOfCode.Year{year}.Day{day:00}.Challenge");

        public static void Create(int year, int day)
        {
            Debug.Assert(GetType(year, day) == null, $"Challenge {year}.{day} already exists");
            
            string path = GetPath(year, day);
            string filePath = $"{path}/Challenge.cs";

            Debug.Assert(!File.Exists(filePath), $"A challenge file already exists at {filePath}");

            Directory.CreateDirectory(GetPath(year, day));
            string template = File.ReadAllText("ChallengeTemplate.txt");
            template = template.Replace("{0}", $"{year}");
            template = template.Replace("{1}", $"{day:00}");
            File.WriteAllText(filePath, template);
        }

        private static BaseChallenge CreateChallengeInstance(Type type) =>
            (BaseChallenge)type.GetConstructor(Type.EmptyTypes)?.Invoke(null)
                ?? throw new Exception($"Failed to create instance of {type}, no public empty constructor found");

        public static void Run(Type type)
        {
            BaseChallenge challenge = CreateChallengeInstance(type);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" <<< Advent of Code {challenge.year} Day {challenge.day} >>> ");
            Console.ResetColor();

            RunPart(challenge, ChallengePart.Part1);
            RunPart(challenge, ChallengePart.Part2);

            Console.WriteLine();
        }

        private static void RunPart(BaseChallenge challenge, ChallengePart part)
        {
            Results results = Execute(challenge, part);

            results.SetStatusColor();
            Console.Write($"[Part {(int)part}]");
            Console.ResetColor();
            Console.Write(" ");

            WriteBenchmark();

            Console.ResetColor();
            string[] messageParts = (results.message ?? string.Empty).Split("{0}");
            if (messageParts.Length > 0)
            {
                Console.Write(messageParts[0]);
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(results.givenAnswer);
            Console.ResetColor();
            if (messageParts.Length > 1)
            {
                Console.Write(messageParts[1]);
            }
            Console.WriteLine();
        }

        public static void Test(Type type)
        {
            BaseChallenge challenge = CreateChallengeInstance(type);

            TestPart(challenge, ChallengePart.Part1);
            TestPart(challenge, ChallengePart.Part2);
        }

        private static void TestPart(BaseChallenge challenge, ChallengePart part)
        {
            Console.SetOut(new StringWriter()); // Discard all output during part execution
            Results results = Execute(challenge, part, fullStackTrace:false);
            ConsoleUtil.RestoreDefaultOutput();

            Console.ForegroundColor = (part == ChallengePart.Part1 ? ConsoleColor.Blue : ConsoleColor.DarkCyan);
            Console.Write($"{challenge.day:00}-{(int)part} ");

            results.SetStatusColor();
            switch (results.status)
            {
                case ResultStatus.Development:
                case ResultStatus.Candidate:
                    Console.Write("WIP ");
                    break;
                case ResultStatus.WrongAnswer:
                case ResultStatus.Exception:
                    Console.Write("FAIL");
                    break;
                case ResultStatus.Success:
                    Console.Write("PASS");
                    break;
            }
            Console.ResetColor();
            Console.Write(" ");

            WriteBenchmark();

            Console.ResetColor();
            Console.WriteLine(results.status == ResultStatus.Exception ? results.message : results.givenAnswer);
        }

        private static Results Execute(BaseChallenge challenge, ChallengePart part, bool fullStackTrace = true)
        {
            Results data = new Results();

            try
            {
                Stopwatch.Restart();
                ResetMethod.Invoke(challenge, null);
                object output = SolvePartMethods[part].Invoke(challenge, null);
                Stopwatch.Stop();

                (string message, object answer) = ((string, object)?)output ?? (null, null);

                data.message = message;
                data.givenAnswer = answer?.ToString();

                string expected = ExpectedAnswerProps[part].GetValue(challenge)?.ToString();
                if (!string.IsNullOrEmpty(expected))
                {
                    data.status = (data.givenAnswer == $"{expected}" ? ResultStatus.Success : ResultStatus.WrongAnswer);
                }
                else if (!string.IsNullOrEmpty(data.givenAnswer))
                {
                    data.status = ResultStatus.Candidate;
                }
                else
                {
                    data.status = ResultStatus.Development;
                    Stopwatch.Reset();
                }
            } catch (Exception ex)
            {
                data.status = ResultStatus.Exception;
                Stopwatch.Reset();
                
                while (ex.InnerException != null) ex = ex.InnerException; // Skip Invoke() and nested exceptions

                data.message = ex.Message;
                if (fullStackTrace)
                {
                    data.message += "\n" + FormatStackTrace(ex.StackTrace);
                }
            }

            return data;
        }

        private static void WriteBenchmark()
        {
            if (Stopwatch.ElapsedTicks == 0) return;

            double elapsed = Stopwatch.Elapsed.TotalSeconds;

            string elapsedStr = elapsed switch
            {
                var e when e <   10 => $"{elapsed:0.000}",
                var e when e <  100 => $"{elapsed:00.00}",
                var e when e < 1000 => $"{elapsed:000.0}",
                _                   => ">1000"
            };

            Console.ForegroundColor = elapsed switch
            {
                var e when e >= 10.0  => ConsoleColor.Red,
                var e when e >=  5.0  => ConsoleColor.DarkRed,
                var e when e >=  1.0  => ConsoleColor.DarkYellow,
                var e when e >=  0.25 => ConsoleColor.DarkGreen,
                _                     => ConsoleColor.DarkGray
            };

            Console.Write($"({elapsedStr}s) ");
        }

        private static string FormatStackTrace(string stackTrace)
        {
            string[] lines = stackTrace.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                Match match = Regex.Match(lines[i], @" in (.*:line \d+)");
                lines[i] = "- " + (match.Success ? match.Groups[1].Value : lines[i].Substring(6)); // Remove "   at "
            }
            return "Stack trace:\n" + lines.Where(l => !string.IsNullOrWhiteSpace(l)).Aggregate((a, b) => $"{a}\n{b}");
        }
    }
}
