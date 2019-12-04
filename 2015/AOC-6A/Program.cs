using System;
using System.IO;
using System.Linq;

public static class Program {
    private enum InstructionType { TurnOn, TurnOff, Toggle }

    private const string TYPE_TURN_ON = "turn on";
    private const string TYPE_TURN_OFF = "turn off";
    private const string TYPE_TOGGLE = "toggle";
    private const string SEPARATOR = " through ";

    private struct Instruction {
        public InstructionType type;
        public Point start;
        public Point end;

        public Instruction(string data) {
            if (data.StartsWith(TYPE_TURN_ON)) {
                type = InstructionType.TurnOn;
                data = data.Substring(TYPE_TURN_ON.Length + 1);
            }
            else if (data.StartsWith(TYPE_TURN_OFF)) {
                type = InstructionType.TurnOff;
                data = data.Substring(TYPE_TURN_OFF.Length + 1);
            }
            else if (data.StartsWith(TYPE_TOGGLE)) {
                type = InstructionType.Toggle;
                data = data.Substring(TYPE_TOGGLE.Length + 1);
            }
            else {
                throw new Exception("Failed to determine instruction type: " + data);
            }

            int sepIndex = data.IndexOf(SEPARATOR);
            
            string pointA = data.Substring(0, sepIndex);
            string[] pointDataA = pointA.Split(',');
            start = new Point(int.Parse(pointDataA[0]), int.Parse(pointDataA[1]));

            string pointB = data.Substring(sepIndex + SEPARATOR.Length);
            string[] pointDataB = pointB.Split(',');
            end = new Point(int.Parse(pointDataB[0]), int.Parse(pointDataB[1]));
        }
    }

    private static void Main(string[] args) {
        string[] input = File.ReadAllLines("input.txt");

        LightBoard board = new LightBoard();

        foreach (Instruction instruction in input.Select(i => new Instruction(i))) {
            switch (instruction.type) {
                case InstructionType.TurnOn:  board.TurnOn(instruction.start, instruction.end);  break;
                case InstructionType.TurnOff: board.TurnOff(instruction.start, instruction.end); break;
                case InstructionType.Toggle:  board.Toggle(instruction.start, instruction.end);  break;
            }
        }

        Console.WriteLine("Lit lights: " + board.LitCount);
    }
}
