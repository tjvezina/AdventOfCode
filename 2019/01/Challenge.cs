using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day01 {
     public class Challenge : BaseChallenge {
        private IEnumerable<int> _massList;

        public override void InitPart1() {
            _massList = inputArray.Select(int.Parse);
        }

        public override string part1Answer => "3323874";
        public override (string, object) SolvePart1() {
            int fuelCost = _massList.Select(GetFuelCost).Sum();
            return ("Total fuel cost: ", fuelCost);
        }
        
        public override string part2Answer => "4982961";
        public override (string, object) SolvePart2() {
            int fuelCost = _massList.Select(GetFuelCostRecursive).Sum();
            return ("Total fuel cost (including fuel itself): ", fuelCost);
        }

        private int GetFuelCost(int mass) => (int)Math.Max(0, mass / 3 - 2);

        private int GetFuelCostRecursive(int mass) {
            int fuel = GetFuelCost(mass);

            if (fuel > 0) {
                fuel += GetFuelCostRecursive(fuel);
            }

            return fuel;
        }
    }
}
