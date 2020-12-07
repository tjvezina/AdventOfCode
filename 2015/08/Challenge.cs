using System.Linq;

namespace AdventOfCode.Year2015.Day08
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 1333;
        public override (string message, object answer) SolvePart1()
        {
            int totalChars = inputList.Sum(s => s.Length);
            int visibleChars = inputList.Sum(GetCharCount);
            return ("Non-visible characters: ", totalChars - visibleChars);
        }
        
        public override object part2ExpectedAnswer => 2046;
        public override (string message, object answer) SolvePart2()
        {
            int totalChars = inputList.Sum(s => s.Length);
            int escapeChars = inputList.Sum(GetEscapedCount);
            return ("New escape char count: ", escapeChars - totalChars);
        }

        private int GetCharCount(string str)
        {
            // Remove enclosing quotes
            str = str.Substring(1, str.Length - 2);

            int length = str.Length;

            int escIndex;
            while ((escIndex = str.IndexOf('\\')) != -1)
            {
                int delta;
                if (str[escIndex + 1] == 'x')
                {
                    delta = 3;
                } else
                {
                    delta = 1;
                }

                length -= delta;
                str = str.Substring(escIndex + 1 + delta);
            }

            return length;
        }

        private int GetEscapedCount(string str)
        {
            for (int i = str.Length - 1; i >= 0; i--)
            {
                if (str[i] == '\"' || str[i] == '\\')
                {
                    str = str.Substring(0, i) + "\\" + str.Substring(i);
                }
            }

            str = $"\"{str}\"";

            return str.Length;
        }
    }
}
