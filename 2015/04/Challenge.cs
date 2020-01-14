using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AdventOfCode.Year2015.Day04 {
     public class Challenge : BaseChallenge {
        private const string Key = "ckczppom";
        private const int ThreadCount = 2;

        private object _lockObj = new object();
        private int _targetInput;

        public override string part1Answer => "117946";
        public override (string, object) SolvePart1() {
            int input = FindInput((hash) => hash[0] + hash[1] == 0 && hash[2] < 16); // Starts with 5 zeros
            return ("Desired input: ", input);
        }
        
        public override string part2Answer => "3938038";
        public override (string, object) SolvePart2() {
            // Unable to optimize further; calculating MD5 hash takes about 8 ticks, and there is no other way to find
            // the answer than brute forcing almost 4 million hashes requiring about 32 million ticks (3.2 sec).
            // 2 threads are faster, but more is slower (too much overhead?)
            int input = FindInputThreaded((hash) => hash[0] + hash[1] + hash[2] == 0); // Starts with 6 zeros
            return ("Desired input: ", input);
        }

        private int FindInput(Func<byte[], bool> isMatch) {
            for (int i = 1; i < int.MaxValue; ++i) {
                string input = Key + i;
                if (isMatch(CalculateMD5Hash(input))) {
                    return i;
                }
            }

            throw new Exception("Failed to find desired input");
        }

        private int FindInputThreaded(Func<byte[], bool> isMatch) {
            Thread[] threads = new Thread[ThreadCount];

            for (int i = 0; i < ThreadCount; ++i) {
                int start = i + 1;
                threads[i] = new Thread(() => ThreadFindInput(isMatch, start));
                threads[i].Start();
            }

            while (threads.Any(t => t.IsAlive));

            return _targetInput;
        }

        private void ThreadFindInput(Func<byte[], bool> isMatch, int start) {
            for (int i = start; i < int.MaxValue; i += ThreadCount) {
                string input = Key + i;
                if (isMatch(CalculateMD5Hash(input))) {
                    lock (_lockObj) {
                        _targetInput = i;
                    }
                    return;
                }

                if (_targetInput > 0 && i > _targetInput) break;
            }
        }

        private byte[] CalculateMD5Hash(string input) => MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
    }
}
