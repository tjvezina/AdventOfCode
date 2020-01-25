using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day01 {
     public class Challenge : BaseChallenge {
        private readonly IEnumerable<int> _massList;

        public Challenge() => _massList = inputList.Select(int.Parse);

        public override string part1ExpectedAnswer => "3323874";
        public override (string message, object answer) SolvePart1() {
            int fuelCost = _massList.Select(GetFuelCost).Sum();
            return ("Total fuel cost: ", fuelCost);
        }
        
        public override string part2ExpectedAnswer => "4982961";
        public override (string message, object answer) SolvePart2() {
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
