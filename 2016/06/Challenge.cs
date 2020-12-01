using System;
using System.Linq;

namespace AdventOfCode.Year2016.Day06
{
    public class Challenge : BaseChallenge
    {
        public override string part1ExpectedAnswer => "gebzfnbt";
        public override (string message, object answer) SolvePart1()
        {
            char GetMostFrequent(char[] set) => set.GroupBy(c => c).OrderBy(g => g.Count()).Last().First();

            return ("Message: ", GetMessage(charSelector:GetMostFrequent));
        }
        
        public override string part2ExpectedAnswer => "fykjtwyn";
        public override (string message, object answer) SolvePart2()
        {
            char GetLeastFrequent(char[] set) => set.GroupBy(c => c).OrderBy(g => g.Count()).First().First();

            return ("Message: ", GetMessage(charSelector:GetLeastFrequent));
        }

        private string GetMessage(Func<char[], char> charSelector)
        {
            int messageLength = inputList[0].Length;
            
            char[][] characters = new char[messageLength][];
            for (int i = 0; i < messageLength; i++)
            {
                characters[i] = inputList.Select(input => input[i]).ToArray();
            }

            char[] message = characters.Select(charSelector).ToArray();

            return new string(message);
        }
    }
}
