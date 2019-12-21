using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015 {
    public class Day08 : Challenge {
        private string[] _input;

        private void Init(string[] input) => _input = input;

        protected override string SolvePart1() {
            return $"{_input.Sum(s => s.Length) - _input.Sum(GetCharCount)}";
        }
        
        protected override string SolvePart2() {
            return $"{_input.Sum(GetEscapedCount) - _input.Sum(s => s.Length)}";
        }

        private int GetCharCount(string str) {
            // Remove enclosing quotes
            str = str.Substring(1, str.Length - 2);

            int length = str.Length;

            int escIndex;
            while ((escIndex = str.IndexOf('\\')) != -1) {
                int delta;
                if (str[escIndex + 1] == 'x') {
                    delta = 3;
                } else {
                    delta = 1;
                }

                length -= delta;
                str = str.Substring(escIndex + 1 + delta);
            }

            return length;
        }

        private int GetEscapedCount(string str) {
            for (int i = str.Length - 1; i >= 0; --i) {
                if (str[i] == '\"' || str[i] == '\\') {
                    str = str.Substring(0, i) + "\\" + str.Substring(i);
                }
            }

            str = $"\"{str}\"";

            return str.Length;
        }
    }
}
