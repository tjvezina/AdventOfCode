using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2015.Day07
{
    public class Challenge : BaseChallenge
    {
        private enum WireOpType
        {
            Assign,
            And,
            Or,
            Not,
            LShift,
            RShift
        }

        private class WireOp
        {
            public WireOpType type;
            public string operandA;
            public string operandB;
            public ushort? value;

            public override string ToString() => $"{operandA} {type} {operandB} = {value}";
        }

        private readonly Dictionary<string, WireOp> _wireMap = new Dictionary<string, WireOp>();
        private ushort _part1Output;

        public Challenge()
        {
            foreach (string instruction in inputList)
            {
                string[] parts = instruction.Split(" -> ", StringSplitOptions.None);
                string[] opData = parts[0].Split(' ');
                string outputWire = parts[1];

                WireOp op = new WireOp();
                switch (opData.Length)
                {
                    case 1: // Assignment op
                        op.type = WireOpType.Assign;
                        op.operandA = opData[0];
                        break;
                    case 2: // NOT op
                        op.type = WireOpType.Not;
                        op.operandA = opData[1];
                        break;
                    default:
                        op.type = opData[1] switch
                        {
                            "AND" => WireOpType.And,
                            "OR" => WireOpType.Or,
                            "LSHIFT" => WireOpType.LShift,
                            "RSHIFT" => WireOpType.RShift,
                            _ => throw new Exception($"Unrecognized op: {opData[1]}")
                        };

                        op.operandA = opData[0];
                        op.operandB = opData[2];
                        break;
                }

                _wireMap[outputWire] = op;
            }
        }

        public override void Reset()
        {
            foreach (WireOp op in _wireMap.Values)
            {
                op.value = null;
            }
        }

        public override object part1ExpectedAnswer => 3176;
        public override (string message, object answer) SolvePart1()
        {
            _part1Output = Resolve("a");
            return ("Wire \"a\" output: ", _part1Output);
        }
        
        public override object part2ExpectedAnswer => 14710;
        public override (string message, object answer) SolvePart2()
        {
            // Sub the original output from wire A for the input of wire B, then resolve again
            _wireMap["b"].operandA = $"{_part1Output}";
            return ("Wire \"a\" output (round 2): ", Resolve("a"));
        }

        private ushort Resolve(string operand)
        {
            // Handle literal values (base case)
            if (ushort.TryParse(operand, out ushort value)) return value;

            WireOp op = _wireMap[operand];

            if (op.value == null)
            {
                // Unary operators
                ushort valueA = Resolve(op.operandA);
                switch (op.type)
                {
                    case WireOpType.Assign: op.value = valueA;          break;
                    case WireOpType.Not:    op.value = (ushort)~valueA; break;
                    default:
                        // Binary operators
                        ushort valueB = Resolve(op.operandB);
                        op.value = op.type switch
                        {
                            WireOpType.LShift => (ushort)(valueA << valueB),
                            WireOpType.RShift => (ushort)(valueA >> valueB),
                            WireOpType.And => (ushort)(valueA & valueB),
                            WireOpType.Or => (ushort)(valueA | valueB),
                            _ => op.value
                        };
                        break;
                }
            }

            if (op.value == null)
            {
                throw new Exception($"Failed to resolve operand {operand} with operator {op.type}");
            }

            return (ushort)op.value;
        }
    }
}
