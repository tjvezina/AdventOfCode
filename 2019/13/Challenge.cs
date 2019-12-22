using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day13 {
     public class Challenge : BaseChallenge {
        private string _intCodeMemory;

        public override void InitPart1() {
            _intCodeMemory = input;
        }

        public override string SolvePart1() {
            new Arcade(_intCodeMemory);
            return null;
        }
        
        public override string SolvePart2() {
            new Arcade(_intCodeMemory, insertQuarters:true);
            return null;
        }
    }
}
