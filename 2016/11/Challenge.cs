using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day11
{
    public class Challenge : BaseChallenge
    {
        public const int FloorCount = 4;

        private const string PatternGenerator = @"(\w+) generator";
        private const string PatternMicrochip = @"(\w+)-compatible microchip";

        public static int ElementCount { get; private set; }

        private List<int> _microchipFloors;
        private List<int> _generatorFloors;

        public Challenge()
        {
            MatchCollection elementMatches = Regex.Matches(inputList.Aggregate((a, b) => a + b), PatternGenerator);
            List<string> elements = elementMatches.Select(m => m.Groups[1].Value).ToList();
            ElementCount = elements.Count;

            _microchipFloors = new List<int>(new int[ElementCount]);
            _generatorFloors = new List<int>(new int[ElementCount]);

            for (int floor = 0; floor < FloorCount; floor++)
            {
                string input = inputList[floor];
                
                foreach (Match generatorMatch in Regex.Matches(input, PatternGenerator))
                {
                    string element = generatorMatch.Groups[1].Value;
                    _generatorFloors[elements.IndexOf(element)] = floor;
                }

                foreach (Match microchipMatch in Regex.Matches(input, PatternMicrochip))
                {
                    string element = microchipMatch.Groups[1].Value;
                    _microchipFloors[elements.IndexOf(element)] = floor;
                }
            }
        }

        public override object part1ExpectedAnswer => 31;
        public override (string message, object answer) SolvePart1()
        {
            State initialState = new State(_microchipFloors, _generatorFloors);

            return ("All objects moved to top floor in {0} moves", GetMinimumSteps(initialState));
        }
        
        public override object part2ExpectedAnswer => 55;
        public override (string message, object answer) SolvePart2()
        {
            // Add new equipment found on first floor
            _microchipFloors.AddRange(new [] { 0, 0 });
            _generatorFloors.AddRange(new [] { 0, 0 });

            ElementCount = _microchipFloors.Count;

            State initialState = new State(_microchipFloors, _generatorFloors);

            return ("All objects moved to top floor in {0} moves", GetMinimumSteps(initialState));
        }

        private int GetMinimumSteps(State initialState)
        {
            Queue<(State state, int steps)> queue = new Queue<(State, int)>(new [] { (initialState, 0) });
            HashSet<int> visited = new HashSet<int> { initialState.GetHashCode() };

            while (queue.Count > 0)
            {
                (State state, int steps) = queue.Dequeue();

                ElementObject[] floorObjects = state.GetElevatorFloorObjects().ToArray();

                IEnumerable<State> GetNextStates(Direction direction, Range comboCountRange)
                {
                    bool statesFound = false;
                    foreach (int comboCount in comboCountRange)
                    {
                        if (statesFound) break;
                        foreach (ElementObject[] objects in DataUtil.GetAllCombinations(floorObjects, comboCount))
                        {
                            State nextState = state.DeepClone();
                            if (nextState.TryStep(direction, objects) && visited.Add(nextState.GetHashCode()))
                            {
                                statesFound = true;
                                yield return nextState;
                            }
                        }
                    }
                }

                IEnumerable<State> nextStates = Enumerable.Empty<State>();
                if (state.elevatorFloor < FloorCount - 1)
                {
                    // If 2 objects can be moved up, don't bother moving 1
                    nextStates = nextStates.Concat(GetNextStates(Direction.Up, new Range(2, 1)));
                }
                // If the floors below are empty, don't bother moving objects back down
                if (state.elevatorFloor > 0 && state.AreObjectsBelowElevator())
                {
                    // If 1 object can be moved down, don't bother moving 2
                    nextStates = nextStates.Concat(GetNextStates(Direction.Down, new Range(1, 2)));
                }

                foreach (State nextState in nextStates)
                {
                    if (nextState.IsFinal())
                    {
                        return steps + 1;
                    }

                    queue.Enqueue((nextState, steps + 1));
                }
            }

            throw new Exception("Failed to find a sequence of steps to move all objects to top floor");
        }
    }
}
