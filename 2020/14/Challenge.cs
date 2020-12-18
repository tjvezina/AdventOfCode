using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day14
{
    public class Challenge : BaseChallenge
    {
        private const int MemorySize = ushort.MaxValue + 1;

        public override object part1ExpectedAnswer => 9296748256641;
        public override (string message, object answer) SolvePart1()
        {
            string mask = string.Empty;
            long[] memory = new long[MemorySize];

            foreach (string input in inputList)
            {
                Match maskMatch = Regex.Match(input, @"mask = ([X01]{36})");
                if (maskMatch.Success)
                {
                    mask = maskMatch.Groups[1].Value;
                }
                else
                {
                    Match memMatch = Regex.Match(input, @"mem\[(\d+)\] = (\d+)");
                    ushort index = ushort.Parse(memMatch.Groups[1].Value);
                    long value = long.Parse(memMatch.Groups[2].Value);

                    for (int i = 0; i < mask.Length; i++)
                    {
                        int bit = mask.Length - i - 1;
                        switch (mask[i])
                        {
                            case '1': value |= 1L << bit; break;
                            case '0': value &= ~(1L << bit); break;
                        }
                    }

                    memory[index] = value;
                }
            }
            
            return ("Sum of all values in memory: ", memory.Sum(x => x));
        }
        
        public override object part2ExpectedAnswer => 4877695371685;
        public override (string message, object answer) SolvePart2()
        {
            char[] mask = { };
            Dictionary<long, int> memory = new Dictionary<long, int>();

            foreach (string input in inputList)
            {
                Match maskMatch = Regex.Match(input, @"mask = ([X01]{36})");
                if (maskMatch.Success)
                {
                    mask = maskMatch.Groups[1].Value.ToCharArray();
                }
                else
                {
                    Match memMatch = Regex.Match(input, @"mem\[(\d+)\] = (\d+)");
                    long address = long.Parse(memMatch.Groups[1].Value);
                    int value = int.Parse(memMatch.Groups[2].Value);

                    string rawBinary = Convert.ToString(address, toBase: 2);
                    int padding = mask.Length - rawBinary.Length;

                    char[] addressBinary = new char[mask.Length];
                    for (int i = 0; i < mask.Length; i++)
                    {
                        if (mask[i] != '0')
                        {
                            addressBinary[i] = mask[i];
                        }
                        else if (i >= padding)
                        {
                            addressBinary[i] = rawBinary[i - padding];
                        }
                        else
                        {
                            addressBinary[i] = '0';
                        }
                    }

                    foreach (char[] subAddressBinary in ExpandAddresses(addressBinary))
                    {
                        long subAddress = Convert.ToInt64(new string(subAddressBinary), fromBase: 2);
                        memory[subAddress] = value;
                    }
                }
            }

            return ("Sum of all values in memory: ", memory.Sum(x => (long)x.Value));
        }

        private IEnumerable<char[]> ExpandAddresses(char[] address)
        {
            int firstX = -1;
            for (int i = 0; i < address.Length; i++)
            {
                if (address[i] == 'X')
                {
                    firstX = i;
                    break;
                }
            }

            if (firstX == -1)
            {
                yield return address;
                yield break;
            }

            char[] sub0 = address.ToArray();
            sub0[firstX] = '0';
            foreach (char[] subAddress in ExpandAddresses(sub0))
            {
                yield return subAddress;
            }

            char[] sub1 = address.ToArray();
            sub1[firstX] = '1';
            foreach (char[] subAddress in ExpandAddresses(sub1))
            {
                yield return subAddress;
            }
        }
    }
}
