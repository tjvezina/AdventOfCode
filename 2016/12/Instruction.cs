using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day12 {
    public class Instruction {
        public enum Type {
            Increment,
            Decrement,
            Copy,
            JumpIfNonZero
        }

        public Type type { get; }
        public IReadOnlyList<object> args { get; }

        public Instruction(string data) {
            string[] parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            type = parts[0] switch {
                "inc" => Type.Increment,
                "dec" => Type.Decrement,
                "cpy" => Type.Copy,
                "jnz" => Type.JumpIfNonZero,
                _     => throw new Exception($"Unrecognized instruction code: {parts[0]}")
            };

            object GetArg(string argData) {
                if (int.TryParse(argData, out int value)) {
                    return value;
                }
                return argData;
            }
            
            args = parts.TakeLast(parts.Length - 1).Select(GetArg).ToList();
        }
    }
}
