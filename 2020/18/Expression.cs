using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day18
{
    public interface IOperand
    {
        long value { get; }
    }

    public class Number : IOperand
    {
        public long value { get; }

        public Number(long value) => this.value = value;
    }

    public class Expression : IOperand
    {
        public long value => Challenge.ruleSet switch
        {
            Challenge.RuleSet.Part1 => EvaluatePart1(),
            Challenge.RuleSet.Part2 => EvaluatePart2(),
            _ => throw new Exception()
        };

        private readonly IList<IOperand> _operands;
        private readonly IList<char> _operators;

        public Expression(IList<IOperand> operands, IList<char> operators)
        {
            _operands = operands;
            _operators = operators;
        }

        private long EvaluatePart1()
        {
            long result = _operands[0].value;

            for (int i = 0; i < _operators.Count; i++)
            {
                result = _operators[i] switch
                {
                    '+' => result + _operands[i+1].value,
                    '*' => result * _operands[i+1].value,
                    _ => throw new Exception($"Unhandled operator: {_operators[i]}")
                };
            }

            return result;
        }

        private long EvaluatePart2()
        {
            LinkedList<long> operands = new LinkedList<long>(_operands.Select(x => x.value));
            LinkedList<char> operators = new LinkedList<char>(_operators);

            LinkedListNode<char> op = operators.First;
            LinkedListNode<long> operand = operands.First;

            while (op != null && operand != null)
            {
                bool merge = false;

                if (op.Value == '+' && operand.Next != null)
                {
                    operand.Next.Value += operand.Value;
                    merge = true;
                }

                LinkedListNode<char> nextOp = op.Next;
                LinkedListNode<long> nextValue = operand.Next;

                if (merge)
                {
                    operators.Remove(op);
                    operands.Remove(operand);
                }

                op = nextOp;
                operand = nextValue;
            }

            return operands.Aggregate((a, b) => a * b);
        }
    }
}
