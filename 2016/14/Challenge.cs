using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day14
{
    public class Challenge : BaseChallenge
    {
        private const string Input = "yjdafjpo";
        private const string Pattern3InARow = @"(.)\1{2}";
        private const string Pattern5InARow = @"(.)\1{4}";

        public override string part1ExpectedAnswer => "25427";
        public override (string message, object answer) SolvePart1()
        {
            return ("64th key index: ", GetKeys().Last());
        }
        
        public override string part2ExpectedAnswer => "22045";
        public override (string message, object answer) SolvePart2()
        {
            return ("64th key index: ", GetKeys(year).Last());
        }

        private IEnumerable<int> GetKeys(int additionalHashes = 0)
        {
            MD5 md5 = MD5.Create();

            // I'm done being forced to optimize MD5 hash computing via multithreading (2015/04, 2016/05),
            // so cache the calculated hashes for next time!
            string hashFileName = $"{Input}-{additionalHashes}.txt";
            List<string> hashList = LoadFileLines(hashFileName).ToList();

            string GetHash(int index)
            {
                if (hashList.Count > index) return hashList[index];

                string data = $"{Input}{index}";

                for (int j = 0; j < 1 + additionalHashes; j++)
                {
                    byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(data));
                    data = string.Concat(hash.Select(b => $"{b:x2}"));
                }

                hashList.Add(data);
                return data;
            }

            List<int> keys = new List<int>();
            Queue<(int i, char c)> potentialKeys = new Queue<(int, char)>();

            int? maxIndex = null;
            for (int i = 0; maxIndex == null || i < maxIndex; i++)
            {
                string hash = GetHash(i);

                if (potentialKeys.Count > 0)
                {
                    foreach (char c in Regex.Matches(hash, Pattern5InARow).Select(m => m.Value[0]))
                    {
                        // Drop "expired" potential keys
                        while (potentialKeys.Peek().i < i - 1000)
                        {
                            potentialKeys.Dequeue();
                        }

                        IEnumerable<(int i, char c)> validKeys = potentialKeys.Where(k => k.c == c);
                        keys.AddRange(validKeys.Select(k => k.i));
                        potentialKeys = new Queue<(int i, char c)>(potentialKeys.Except(validKeys));

                        if (maxIndex == null && keys.Count >= 64)
                        {
                            maxIndex = keys.Max() + 1000;
                        }
                    }
                }

                Match match = Regex.Match(hash, Pattern3InARow);
                if (match.Success)
                {
                    potentialKeys.Enqueue((i, match.Value[0]));
                }
            }

            SaveFile(hashFileName, hashList);

            return keys.OrderBy(x => x).Take(64);
        }
    }
}
