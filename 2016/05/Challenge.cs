using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2016.Day05 {
    public class Challenge : BaseChallenge {
        private const string DoorID = "wtnhxymk";

        public override string part1Answer => "2414BC77";
        public override (string, object) SolvePart1() => ("Password: ", GetPasswordA());
        
        public override string part2Answer => "437E60FC";
        public override (string, object) SolvePart2() => ("Password: ", GetPasswordB());

        private string GetPasswordA() {
            string password = string.Empty;

            int input = 0;
            while (true) {
                byte[] hash = CalculateMD5Hash($"{DoorID}{input}");

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16) { // Starts with 5 zeros
                    password += $"{hash[2]:X2}"[1];
                    Console.WriteLine($"Character found in input {input:N0}: {password}");

                    if (password.Length == 8) break;
                }

                input++;
            }

            return password;
        }

        private string GetPasswordB() {
            char[] password = new string('_', 8).ToCharArray();

            int input = 0;
            while (true) {
                byte[] hash = CalculateMD5Hash($"{DoorID}{input}");

                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16) { // Starts with 5 zeros
                    byte index = hash[2];
                    if (index < 8 && password[index] == '_') {
                        password[index] = $"{hash[3]:X2}"[0];
                        Console.WriteLine($"Character found in input {input:N0}: {new string(password)}");

                        if (!password.Contains('_')) break;
                    }
                }

                input++;
            }

            return new string(password);
        }

        private byte[] CalculateMD5Hash(string input) => MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
    }
}
