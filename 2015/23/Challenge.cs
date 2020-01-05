using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2015.Day23 {
    public class Challenge : BaseChallenge {
        public override string part1Answer => "307";
        public override (string, object) SolvePart1() {
            Computer computer = new Computer(inputSet);
            computer.Run();

            return ("Register B: ", computer["b"]);
        }
        
        public override string part2Answer => "160";
        public override (string, object) SolvePart2() {
            Computer computer = new Computer(inputSet);
            computer["a"] = 1;
            computer.Run();

            return ("Register B: ", computer["b"]);
        }
    }
}
