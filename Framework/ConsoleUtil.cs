using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode
{
    public static class ConsoleUtil
    {
        private static Stack<ConsoleColor> _foregroundStack = new Stack<ConsoleColor>();
        private static Stack<ConsoleColor> _backgroundStack = new Stack<ConsoleColor>();

        public static void RestoreDefaultOutput()
        {
            StreamWriter standardOut = new StreamWriter(Console.OpenStandardOutput());
            standardOut.AutoFlush = true;
            Console.SetOut(standardOut);
        }

        public static void PushForeground(ConsoleColor foreground)
        {
            _foregroundStack.Push(Console.ForegroundColor);
            Console.ForegroundColor = foreground;
        }

        public static void PopForeground()
        {
            Console.ForegroundColor = _foregroundStack.Pop();
        }

        public static void PushBackground(ConsoleColor background)
        {
            _backgroundStack.Push(Console.BackgroundColor);
            Console.BackgroundColor = background;
        }

        public static void PopBackground(ConsoleColor background)
        {
            Console.BackgroundColor = _backgroundStack.Pop();
        }
    }
}
