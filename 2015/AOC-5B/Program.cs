using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        Console.WriteLine("Nice strings: " + input.Count(IsNice));
    }

    private static bool IsNice(string str) {
        return CheckRule1(str) && CheckRule2(str);
    }

    private static bool CheckRule1(string str) {
        for (int i = 0; i < str.Length - 3; ++i) {
            string pairA = str.Substring(i, 2);
            for (int j = i + 2; j < str.Length - 1; ++j) {
                string pairB = str.Substring(j, 2);
                if (pairA == pairB) {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool CheckRule2(string str) {
        for (int i = 0; i < str.Length - 2; ++i) {
            if (str[i] == str[i + 2]) {
                return true;
            }
        }

        return false;
    }
}