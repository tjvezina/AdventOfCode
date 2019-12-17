using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019 {
    public class Day01 : Challenge {
        private IEnumerable<int> _massList;

        private void Init(string[] input) {
            _massList = input.Select(int.Parse);
        }

        protected override string SolvePart1() {
            int fuelCost = _massList.Select(GetFuelCost).Sum();
            return $"Total fuel cost: {fuelCost}";
        }
        
        protected override string SolvePart2() {
            int fuelCost = _massList.Select(GetFuelCostRecursive).Sum();
            return $"Total fuel cost (including fuel itself): {fuelCost}";
        }

        private int GetFuelCost(int mass) => (int)Math.Max(0, mass / 3 - 2);

        private int GetFuelCostRecursive(int mass) {
            int fuel = GetFuelCost(mass);

            if (fuel > 0) {
                fuel += GetFuelCost(fuel);
            }

            return fuel;
        }
    }
}
