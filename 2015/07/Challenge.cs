using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day07 {
     public class Challenge : BaseChallenge {
        public enum WireOpType {
            Assign,
            And,
            Or,
            Not,
            LShift,
            RShift
        }

        public class WireOp {
            public WireOpType type;
            public string operandA;
            public string operandB;
            public UInt16? value;

            public override string ToString() => $"{operandA} {type} {operandB} = {value}";
        }

        private static Dictionary<string, WireOp> _wireMap = new Dictionary<string, WireOp>();
        private UInt16 _part1Output;

        public override void InitPart1() {
            foreach (string instruction in inputArray) {
                string[] parts = instruction.Split(" -> ", StringSplitOptions.None);
                string[] opData = parts[0].Split(' ');
                string outputWire = parts[1];

                WireOp op = new WireOp();
                if (opData.Length == 1) { // Assignment op
                    op.type = WireOpType.Assign;
                    op.operandA = opData[0];
                }
                else if (opData.Length == 2) { // NOT op
                    op.type = WireOpType.Not;
                    op.operandA = opData[1];
                }
                else {
                    op.type = opData[1] switch {
                        "AND" => WireOpType.And,
                        "OR" => WireOpType.Or,
                        "LSHIFT" => WireOpType.LShift,
                        "RSHIFT" => WireOpType.RShift,
                        _ => throw new Exception($"Unrecognized op: {opData[1]}")
                    };

                    op.operandA = opData[0];
                    op.operandB = opData[2];
                }

                _wireMap[outputWire] = op;
            }
        }

        public override string part1Answer => "3176";
        public override (string, object) SolvePart1() {
            _part1Output = Resolve("a");
            return ("Wire \"a\" output: ", _part1Output);
        }
        
        public override void InitPart2() {
            foreach (WireOp op in _wireMap.Values) {
                op.value = null;
            }
        }

        public override string part2Answer => "14710";
        public override (string, object) SolvePart2() {
            // Sub the original output from wire A for the input of wire B, then resolve again
            _wireMap["b"].operandA = $"{_part1Output}";
            return ("Wire \"a\" output (round 2): ", Resolve("a"));
        }

        private UInt16 Resolve(string operand) {
            // Handle literal values (base case)
            if (UInt16.TryParse(operand, out UInt16 value)) return value;

            WireOp op = _wireMap[operand];

            if (op.value == null) {
                // Unary operators
                UInt16 valueA = Resolve(op.operandA);
                if      (op.type == WireOpType.Assign) op.value = valueA;
                else if (op.type == WireOpType.Not)    op.value = (UInt16)~valueA;
                else {
                    // Binary operators
                    UInt16 valueB = Resolve(op.operandB);
                    if      (op.type == WireOpType.LShift) op.value = (UInt16)(valueA << valueB);
                    else if (op.type == WireOpType.RShift) op.value = (UInt16)(valueA >> valueB);
                    else if (op.type == WireOpType.And)    op.value = (UInt16)(valueA & valueB);
                    else if (op.type == WireOpType.Or)     op.value = (UInt16)(valueA | valueB);
                }
            }

            if (op.value == null) {
                throw new Exception($"Failed to resolve operand {operand} with operator {op.type}");
            }

            return (UInt16)op.value;
        }
    }
}
