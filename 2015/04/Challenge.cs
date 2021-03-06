using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AdventOfCode.Year2015.Day04
{
    public class Challenge : BaseChallenge
    {
        private const string Key = "ckczppom";
        private const int ThreadCount = 10;

        private readonly object _lockObj = new object();
        private int _targetInput;

        public override void Reset() => _targetInput = 0;

        public override object part1ExpectedAnswer => 117946;
        public override (string message, object answer) SolvePart1()
        {
            int input = FindInput(h => h[0] + h[1] == 0 && h[2] < 0x10); // Starts with 5 zeros
            return ("Desired input: ", input);
        }

        public override object part2ExpectedAnswer => 3938038;
        public override (string message, object answer) SolvePart2()
        {
            int input = FindInput(h => h[0] + h[1] + h[2] == 0); // Starts with 6 zeros
            return ("Desired input: ", input);
        }

        private int FindInput(Func<byte[], bool> isMatch)
        {
            Thread[] threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; i++)
            {
                int start = i + 1;
                threads[i] = new Thread(() => FindInputThread(isMatch, start));
                threads[i].Start();
            }

            while (threads.Any(t => t.IsAlive)) { }

            return _targetInput;
        }

        private void FindInputThread(Func<byte[], bool> isMatch, int start)
        {
            MD5 md5 = MD5.Create();

            for (int i = start; i < int.MaxValue; i += ThreadCount)
            {
                string input = Key + i;
                byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                if (isMatch(hash))
                {
                    lock (_lockObj)
                    {
                        _targetInput = i;
                    }
                    return;
                }

                if (0 < _targetInput && _targetInput < i) break;
            }
        }
    }
}
