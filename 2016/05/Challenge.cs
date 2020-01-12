using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day05 {
    public class Challenge : BaseChallenge {
        private const string DoorID = "wtnhxymk";
        private const int PasswordLength = 8;
        private const char Placeholder = '-';

        private MD5 _md5 = MD5.Create();

        public override string part1Answer => "2414BC77";
        public override (string, object) SolvePart1() {
            string password = string.Empty;

            foreach ((int input, byte[] hash) in GetValidHashes()) {
                password += $"{hash[2]:X2}"[1];
                Console.WriteLine($"Password: {password,-PasswordLength} ({input,10:N0})");

                if (password.Length == PasswordLength) break;
            }

            return ("Password: ", password);
        }
        
        public override string part2Answer => "437E60FC";
        public override (string, object) SolvePart2() {
            char[] password = new string(Placeholder, PasswordLength).ToCharArray();

            foreach ((int input, byte[] hash) in GetValidHashes()) {
                byte index = hash[2];
                if (index < 0x08 && password[index] == Placeholder) {
                    password[index] = $"{hash[3]:X2}"[0];
                    Console.WriteLine($"Password: {new string(password)} ({input,10:N0})");

                    if (!password.Contains(Placeholder)) break;
                }

            }

            return ("Password: ", new string(password));
        }

        private IEnumerable<(int input, byte[] hash)> GetValidHashes() {
            byte[] ToBytes(int i) => Encoding.UTF8.GetBytes($"{DoorID}{i}");

            byte[] buffer = ToBytes(0);

            for (int i = 0; i < int.MaxValue; ++i) {
                byte[] hash = _md5.ComputeHash(buffer);

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 0x0F) { // Starts with 5 zeros
                    yield return (i, hash);
                }

                if (buffer[buffer.Length - 1]++ == '9') {
                    buffer = ToBytes(i);
                }
            }
        }
    }
}
