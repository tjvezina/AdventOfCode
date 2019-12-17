using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day13 : Challenge {
        private string _intCodeMemory;

        private void Init(string input) => _intCodeMemory = input;

        protected override string SolvePart1() {
            new Arcade(_intCodeMemory);
            return null;
        }
        
        protected override string SolvePart2() {
            new Arcade(_intCodeMemory, insertQuarters:true);
            return null;
        }
    }
}
