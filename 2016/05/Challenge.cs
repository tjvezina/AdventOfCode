using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace AdventOfCode.Year2016.Day05 {
    public class Challenge : BaseChallenge {
        private const string DoorID = "wtnhxymk";
        private const int PasswordLength = 8;

        private const int ThreadCount = 12;

        private SortedList<int, byte[]> _validHashes = new SortedList<int, byte[]>();
        private int _maxInput;

        public override string part1Answer => "2414BC77";
        public override (string, object) SolvePart1() {
            FindHashes(condition:() => _validHashes.Count >= PasswordLength);

            string password = string.Concat(_validHashes.Take(PasswordLength).Select(d => $"{d.Value[2]:X2}"[1]));

            return ("Password: ", password);
        }
        
        public override string part2Answer => "437E60FC";
        public override (string, object) SolvePart2() {
            IEnumerable<byte> indices = Enumerable.Range(0, PasswordLength).Select(i => (byte)i);

            FindHashes(condition:() => !indices.Except(_validHashes.Select(d => d.Value[2])).Any());

            IEnumerable<byte[]> passwordHashes = indices.Select(i => _validHashes.First(p => p.Value[2] == i).Value);
            string password = string.Concat(passwordHashes.Select(h => $"{h[3]:X2}"[0]));

            return ("Password: ", password);
        }

        private void FindHashes(Func<bool> condition) {
            _maxInput = int.MaxValue;
            Thread[] threads = new Thread[ThreadCount];

            int start = (int)Math.Ceiling(_validHashes.LastOrDefault().Key / 10.0m) * 10;

            for (int i = 0; i < ThreadCount; i++) {
                int threadStart = start + (i * 10);
                threads[i] = new Thread(() => FindValidHashesThread(threadStart));
                threads[i].Priority = ThreadPriority.Highest;
                threads[i].Start();
            }

            int lastCount = _validHashes.Count;
            while (true) {
                if (_validHashes.Count != lastCount) {
                    lock (_validHashes) {
                        if (condition.Invoke()) {
                            _maxInput = _validHashes.Last().Key;
                            break;
                        }
                    }
                }
            }

            while (threads.Any(t => t.IsAlive));
        }

        private void FindValidHashesThread(int start) {
            MD5 md5 = MD5.Create();

            for (int i = start; i < _maxInput; i += 10 * ThreadCount) {
                byte[] buffer = GetInputBytes(i);

                for (int j = 0; j < 10; j++, buffer[buffer.Length - 1]++) {
                    byte[] hash = md5.ComputeHash(buffer);

                    if (hash[0] + hash[1] == 0 && hash[2] < 0x10) {
                        lock (_validHashes) {
                            _validHashes.Add(i + j, hash);
                        }
                    }
                }
            }
        }

        private byte[] GetInputBytes(int input) => Encoding.ASCII.GetBytes($"{DoorID}{input}");
    }
}
