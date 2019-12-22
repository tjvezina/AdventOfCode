using System;
using System.IO;

namespace AdventOfCode {
    public static class ChallengeManager {
        public static Type GetType(int year, int day) => Type.GetType($"AdventOfCode.Year{year}.Day{day:00}.Challenge");

        public static void Run(Type type, bool test = false) {
            BaseChallenge challenge = (BaseChallenge)type.GetConstructor(Type.EmptyTypes).Invoke(null);

            if (!test) {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" <<< Advent of Code {challenge.year} Day {challenge.day} >>> ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            RunPart(challenge, 1, challenge.InitPart1, challenge.SolvePart1, test);
            RunPart(challenge, 2, challenge.InitPart2, challenge.SolvePart2, test);
        }

        private static void RunPart(BaseChallenge challenge, int part, Action init, Func<string> solve, bool test) {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (!test) {
                Console.Write($"[Part {part}] ");
            } else {
                Console.Write($"[{challenge.year} {challenge.day:00} {(part == 1 ? 'A' : 'B')}] ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            TextWriter prevWriter = Console.Out;
            TextWriter partWriter = new DelayedWriter();
            Console.SetOut(partWriter);
            init.Invoke();
            string solution = solve.Invoke();
            Console.SetOut(prevWriter);

            if (!test) {
                if (!string.IsNullOrWhiteSpace(solution)) {
                    Console.WriteLine($"{solution}");
                }
                partWriter.Flush();
                Console.WriteLine();
            } else {
                Console.WriteLine(solution);
            }
        }
    }
}
