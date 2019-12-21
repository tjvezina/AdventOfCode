using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Year2015 {
    public class Day04 : Challenge {
        private const string KEY = "ckczppom";

        protected override string SolvePart1() => $"Desired input: {FindInput("00000")}";
        
        protected override string SolvePart2() => $"Desired input: {FindInput("000000")}";

        private int FindInput(string target) {
            for (int i = 1; i < int.MaxValue; ++i) {
                string input = KEY + i;
                if (CalculateMD5Hash(input).StartsWith(target)) {
                    return i;
                }
            }

            throw new Exception("Failed to find desired input");
        }

        private string CalculateMD5Hash(string input) {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            return hash.Select(b => $"{b:X2}").Aggregate((a, b) => a + b);
        }
    }
}
