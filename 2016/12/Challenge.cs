using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day12 {
    public class Challenge : BaseChallenge {
        public override void InitPart1() { }

        public override string part1Answer => "318007";
        public override (string, object) SolvePart1() {
            Computer computer = new Computer(inputArray);
            computer.Run();

            return ("Password: ", computer["a"]);
        }
        
        public override string part2Answer => "9227661";
        public override (string, object) SolvePart2() {
            Computer computer = new Computer(inputArray);
            computer["c"] = 1;
            computer.Run();

            return ("Password: ", computer["a"]);
        }
    }
}
