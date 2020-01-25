using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day11 {
     public class Challenge : BaseChallenge {
        private const string Input = "vzbxkghb";

        private Password _password = new Password(Input);

        public override string part1ExpectedAnswer => "vzbxxyzz";
        public override (string message, object answer) SolvePart1() {
            NextValidPassword();
            return ("Next valid password: ", _password);
        }
        
        public override string part2ExpectedAnswer => "vzcaabcc";
        public override (string message, object answer) SolvePart2() {
            NextValidPassword();
            return ("Next valid password: ", _password);
        }

        private void NextValidPassword() {
            do {
                _password.Increment();
            } while (!_password.IsValid());
        }
    }
}
