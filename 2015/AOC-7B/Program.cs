using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
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

    private const string SEPARATOR = " -> ";

    private static Dictionary<string, WireOp> _wireMap = new Dictionary<string, WireOp>();

    private static void Main(string[] args) {
        ParseInput();

        // Sub the original output from wire A for the input of wire B, then resolve again
        _wireMap["b"].operandA = "3176";

        Console.WriteLine(Resolve("a"));
    }

    private static void ParseInput() {
        string[] input = File.ReadAllLines("input.txt");

        foreach (string instruction in input) {
            int sepIndex = instruction.IndexOf(SEPARATOR);
            string outputWire = instruction.Substring(sepIndex + SEPARATOR.Length);
            string[] opData = instruction.Substring(0, sepIndex).Split(' ');

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
                switch (opData[1]) {
                    case "AND":    op.type = WireOpType.And;    break;
                    case "OR":     op.type = WireOpType.Or;     break;
                    case "LSHIFT": op.type = WireOpType.LShift; break;
                    case "RSHIFT": op.type = WireOpType.RShift; break;
                    default:
                        throw new Exception("Unrecognized op: " + opData[1]);
                }

                op.operandA = opData[0];
                op.operandB = opData[2];
            }

            _wireMap[outputWire] = op;
        }
    }

    private static UInt16 Resolve(string operand) {
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
