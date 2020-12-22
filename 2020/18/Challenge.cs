using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day18
{
    public class Challenge : BaseChallenge
    {
        public enum RuleSet { Part1, Part2 }

        public static RuleSet ruleSet { get; private set; }

        public override object part1ExpectedAnswer => 5374004645253;
        public override (string message, object answer) SolvePart1()
        {
            ruleSet = RuleSet.Part1;

            return ("The sum of all expressions is ", inputList.Select(ParseExpression).Sum(x => x.value));
        }
        
        public override object part2ExpectedAnswer => 88782789402798;
        public override (string message, object answer) SolvePart2()
        {
            ruleSet = RuleSet.Part2;

            return ("The sum of all expressions is ", inputList.Select(ParseExpression).Sum(x => x.value));
        }

        private Expression ParseExpression(string expression)
        {
            List<IOperand> operands = new List<IOperand>();
            List<char> operators = new List<char>();

            int i = 0;
            while (i < expression.Length)
            {
                char c = expression[i];

                if ('0' <= c && c <= '9')
                {
                    operands.Add(new Number(long.Parse($"{c}")));
                }
                else
                {
                    switch (c)
                    {
                        case ' ':
                            break;
                        case '+':
                        case '*':
                            operators.Add(c);
                            break;
                        case '(':
                            int iClose = IndexOfClosing(expression, i);
                            operands.Add(ParseExpression(expression.Substring(i + 1, iClose - i - 1)));
                            i = iClose;
                            break;
                    }
                }

                i++;
            }

            return new Expression(operands, operators);
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
