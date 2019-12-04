using System;
using System.Collections.Generic;
using System.IO;

public static class Program {
    public enum WireOpType {
        Assign,
        And,
        Or,
        Not,
        LShift,
        RShift
    }

    public struct WireOp {
        public WireOpType type;
        public string operandA;
        public string operandB;
    }

    private const string SEPARATOR = " -> ";

    private static Dictionary<string, WireOp> _wireMap = new Dictionary<string, WireOp>();

    private static void Main(string[] args) {
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

        // output = (UInt16)(_wireMap[opData[0]] & _wireMap[opData[2]]);
        // output = (UInt16)(_wireMap[opData[0]] | _wireMap[opData[2]]);
        // output = (UInt16)(_wireMap[opData[0]] << int.Parse(opData[2]));
        // output = (UInt16)(_wireMap[opData[0]] >> int.Parse(opData[2]));
    }
}
