using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public static class Program {
    private const string KEY = "ckczppom";
    private const string TARGET = "000000";

    private static void Main(string[] args) {
        for (int i = 1; i < int.MaxValue; ++i) {
            string input = KEY + i;
            if (CalculateMD5Hash(input).StartsWith(TARGET)) {
                Console.WriteLine(i);
                return;
            }
        }

        Console.WriteLine("No match found");
    }

    private static string CalculateMD5Hash(string input) {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hash = md5.ComputeHash(inputBytes);
        return hash.Select(b => $"{b:X2}").Aggregate((a, b) => a + b);
    }
}
