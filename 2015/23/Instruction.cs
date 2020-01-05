using System;

namespace AdventOfCode.Year2015.Day23 {
    public class Instruction {
        public enum Type {
            Half,
            Triple,
            Increment,
            Jump,
            JumpIfEven,
            JumpIfOne
        }

        public Type type { get; }
        public string register { get; }
        public int offset { get; }

        public Instruction(string data) {
            string[] parts = data.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

            type = parts[0] switch {
                "hlf" => Type.Half,
                "tpl" => Type.Triple,
                "inc" => Type.Increment,
                "jmp" => Type.Jump,
                "jie" => Type.JumpIfEven,
                "jio" => Type.JumpIfOne,
                _     => throw new Exception($"Unrecognized instruction code: {parts[0]}")
            };

            if (type != Type.Jump) {
                register = parts[1];
            }
            
            switch (type) {
                case Type.Jump:
                    offset = int.Parse(parts[1]);
                    break;
                case Type.JumpIfEven:
                case Type.JumpIfOne:
                    offset = int.Parse(parts[2]);
                    break;
            }
        }
    }
}
