using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day11 {
     public class Challenge : BaseChallenge {
        private const string INPUT = "vzbxkghb";

        private Password _password = new Password(INPUT);

        public override string part1Answer => "vzbxxyzz";
        public override (string, object) SolvePart1() {
            NextValidPassword();
            return ("Next valid password: ", _password);
        }
        
        public override string part2Answer => "vzcaabcc";
        public override (string, object) SolvePart2() {
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
