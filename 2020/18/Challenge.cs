using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day18
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 5374004645253;
        public override (string message, object answer) SolvePart1()
        {
            return ("The sum of all expressions is ", inputList.Select(Evaluate).Sum());
        }
        
        public override object part2ExpectedAnswer => null;
        public override (string message, object answer) SolvePart2()
        {
            return ("", null);
        }

        private long Evaluate(string expression)
        {
            int index = 0;

            long? result = null;
            char op = default;

            while (index < expression.Length)
            {
                char c = expression[index];
                long? value = null;

                if ('0' <= c && c <= '9')
                {
                    value = long.Parse($"{c}");
                }
                else
                {
                    switch (c)
                    {
                        case ' ':
                            break;
                        case '+':
                        case '*':
                            op = c;
                            break;
                        case '(':
                            int iClose = IndexOfClosing(expression, index);
                            value = Evaluate(expression.Substring(index + 1, iClose - index - 1));
                            index = iClose;
                            break;
                    }
                }

                if (value.HasValue)
                {
                    if (!result.HasValue)
                    {
                        result = value;
                    }
                    else
                    {
                        result = op switch
                        {
                            '+' => result + value,
                            '*' => result * value,
                            _ => throw new Exception($"Unknown operator {op}")
                        };
                        op = default;
                    }
                }

                index++;
            }

            return result ?? throw new Exception($"Failed to evaluate \"{expression}\", no numbers found");
        }

        private int IndexOfClosing(string expression, int iOpen)
        {
            int depth = 0;

            for (int i = iOpen; i < expression.Length; i++)
            {
                switch (expression[i])
                {
                    case '(':
                        depth++;
                        break;
                    case ')':
                        depth--;
                        if (depth == 0) return i;
                        break;
                }
            }

            throw new Exception($"No matching closing brace in expression \"{expression}\"");
        }
    }
}
