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
        private const int ThreadCount = 6;

        private volatile SortedList<int, (char a, char b)> _passwordValues = new SortedList<int, (char a, char b)>();

        private Thread[] _threads = new Thread[ThreadCount];
        private volatile bool[] _atPart1Target = new bool[ThreadCount];
        private int _part1Target = int.MaxValue;
        private int _part2Target = int.MaxValue;

        public override void InitPart1() => StartThreads();

        public override string part1Answer => "2414BC77";
        public override (string, object) SolvePart1() {
            WaitForPart1Values();

            string password;

            lock (_passwordValues) {
                password = string.Concat(_passwordValues.Values.Take(PasswordLength).Select(v => v.a));
            }

            return ("Password: ", password);
        }
        
        public override string part2Answer => "437E60FC";
        public override (string, object) SolvePart2() {
            WaitForPart2Values();

            char[] password = new char[PasswordLength];

            lock (_passwordValues) {
                for (int i = 0; i < PasswordLength; i++) {
                    password[i] = _passwordValues.First(p => p.Value.a == IndexToChar(i)).Value.b;
                }
            }

            return ("Password: ", string.Concat(password));
        }

        private void StartThreads() {
            for (int i = 0; i < ThreadCount; i++) {
                int threadIndex = i;
                _threads[i] = new Thread(() => FindValidHashesThread(threadIndex));
                _threads[i].Priority = ThreadPriority.Highest;
                _threads[i].Start();
            }
        }

        private void WaitForPart1Values() {
            while (_passwordValues.Count < PasswordLength) { Thread.Yield(); }

            lock (_passwordValues) {
                _part1Target = _passwordValues.Last().Key;
            }

            while (_atPart1Target.Any(b => !b)) { Thread.Yield(); }
        }

        private void WaitForPart2Values() {
            IList<char> missingIndices = Enumerable.Range(0, PasswordLength).Select(IndexToChar).ToList();

            int iChecked = 0;
            int maxKeyChecked = 0;
            while (missingIndices.Any()) {
                Thread.Yield();
                if (_passwordValues.Count > iChecked) {
                    lock (_passwordValues) {
                        KeyValuePair<int, (char a, char b)> pair = _passwordValues.ElementAt(iChecked++);
                        maxKeyChecked = Math.Max(maxKeyChecked, pair.Key);
                        missingIndices.Remove(pair.Value.a);
                    }
                }
            }

            _part2Target = maxKeyChecked;

            while (_threads.Any(t => t.IsAlive)) { Thread.Yield(); }
        }

        private void FindValidHashesThread(int threadIndex) {
            MD5 md5 = MD5.Create();

            byte[] buffer = new byte[DoorID.Length + (int)Math.Log10(int.MaxValue)];
            byte[] doorIDBytes = Encoding.ASCII.GetBytes(DoorID);
            Array.Copy(doorIDBytes, buffer, doorIDBytes.Length);

            int iFirst = doorIDBytes.Length;
            for (int i = 10 * threadIndex; i < _part2Target; i += 10 * ThreadCount) {
                if (!_atPart1Target[threadIndex] && i > _part1Target) {
                    lock (_atPart1Target) {
                        _atPart1Target[threadIndex] = true;
                    }
                }

                // Iterations always start at a multiple of 10, so skip the last digit
                int iInput = 0;
                for (int value = i / 10; value > 0; value /= 10, iInput++) {
                    buffer[iFirst + iInput] = (byte)('0' + value % 10);
                }

                buffer.Reverse(iFirst, iInput);

                // Handle the one's place
                buffer[iFirst + iInput] = (byte)'0';
                ++iInput;

                for (int j = 0; j < 10; j++) {
                    byte[] hash = md5.ComputeHash(buffer, 0, iFirst + iInput);

                    if (hash[0] + hash[1] == 0 && hash[2] < 0x10) {
                        lock (_passwordValues) {
                            _passwordValues.Add(i + j, (GetHashChar(hash, 5), GetHashChar(hash, 6)));
                        }
                    }

                    buffer[iFirst + iInput - 1]++;
                }
            }
        }

        private byte[] GetInputBytes(int input) => Encoding.ASCII.GetBytes($"{DoorID}{input}");

        private char IndexToChar(int index) => (char)('0' + index);

        private char GetHashChar(byte[] hash, int index) => $"{hash[index / 2]:X2}"[index % 2];
    }
}
