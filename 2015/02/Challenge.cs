using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day02 {
     public class Challenge : BaseChallenge {
        private struct Box {
            private List<int> _sides;

            public int l => _sides[0];
            public int w => _sides[1];
            public int h => _sides[2];

            public int volume => l * w * h;
            public int surfaceArea => (2 * l * w) + (2 * w * h) + (2 * h * l);
            public int smallestSideArea => _sides[0] * _sides[1];
            public int smallestSidePerim => (2 * _sides[0]) + (2 * _sides[1]);

            public Box(string data) {
                _sides = data.Split('x').Select(int.Parse).ToList();
                _sides.Sort();
            }
        }

        private IEnumerable<Box> _boxes;

        public override void InitPart1() {
            _boxes = inputArray.Select(data => new Box(data));
        }

        public override string part1Answer => "1586300";
        public override (string, object) SolvePart1() {
            return ("Wrapping paper needed: ", _boxes.Sum(b => b.surfaceArea + b.smallestSideArea));
        }
        
        public override string part2Answer => "3737498";
        public override (string, object) SolvePart2() {
            return ("Ribbon needed: ", _boxes.Sum(b => b.smallestSidePerim + b.volume));
        }
    }
}
