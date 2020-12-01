using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day11
{
    public enum Direction { Up = 1, Down = -1 }

    public struct ElementObject
    {
        public enum Type { Microchip, Generator }
        
        public int index;
        public Type type;

        public ElementObject(int index, Type type)
        {
            this.index = index;
            this.type = type;
        }

        public override string ToString() => $"{type} {index}";
    }

    public class State : IDeepCloneable<State>
    {
        public int elevatorFloor { get; private set; }
        private int[] _microchipFloors;
        private int[] _generatorFloors;

        private State() { }

        public State(IList<int> microchipFloors, IList<int> generatorFloors)
        {
            _microchipFloors = microchipFloors.ToArray();
            _generatorFloors = generatorFloors.ToArray();
            elevatorFloor = 0;
        }

        public State DeepClone()
        {
            State clone = new State();
            clone.elevatorFloor = elevatorFloor;
            clone._microchipFloors = _microchipFloors.ToArray();
            clone._generatorFloors = _generatorFloors.ToArray();
            return clone;
        }

        public IEnumerable<ElementObject> GetElevatorFloorObjects() => GetFloorObjects(elevatorFloor);
        public IEnumerable<ElementObject> GetFloorObjects(int floor)
        {
            for (int i = 0; i < Challenge.ElementCount; i++)
            {
                if (_microchipFloors[i] == floor)
                {
                    yield return new ElementObject(i, ElementObject.Type.Microchip);
                }
                if (_generatorFloors[i] == floor)
                {
                    yield return new ElementObject(i, ElementObject.Type.Generator);
                }
            }
        }

        public bool AreObjectsBelowElevator()
        {
            return _microchipFloors.Any(f => f < elevatorFloor) || _generatorFloors.Any(f => f < elevatorFloor);
        }

        public bool TryStep(Direction direction, params ElementObject[] objects)
        {
            int offset = (int)direction;
            elevatorFloor += offset;
            
            foreach (ElementObject obj in objects)
            {
                switch (obj.type)
                {
                    case ElementObject.Type.Microchip:
                        _microchipFloors[obj.index] += offset;
                        break;
                    case ElementObject.Type.Generator:
                        _generatorFloors[obj.index] += offset;
                        break;
                }
            }

            for (int m = 0; m < Challenge.ElementCount; m++)
            {
                // If a microchip is on the same floor as another element's generator without its generator, it's fried
                int microchipFloor = _microchipFloors[m];
                if (microchipFloor != _generatorFloors[m] && _generatorFloors.Any(g => g == microchipFloor))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFinal()
        {
            if (elevatorFloor == Challenge.FloorCount - 1)
            {
                return _microchipFloors.Concat(_generatorFloors).All(f => f == Challenge.FloorCount - 1);
            }
            return false;
        }

        public bool Equals(State other)
        {
            return elevatorFloor == other.elevatorFloor &&
                _microchipFloors.SequenceEqual(other._microchipFloors) &&
                _generatorFloors.SequenceEqual(other._generatorFloors);
        }

        public override bool Equals(object obj) => obj is State other && Equals(other);
        public override int GetHashCode()
        {
            // Sort element pairs, as states where pairs are swapped should be considered equivalent
            int hashCode = Enumerable.Range(0, Challenge.ElementCount)
                .Select(i => (_microchipFloors[i] << 2) + _generatorFloors[i])
                .OrderBy(x => x).Aggregate((a, b) => (a << 4) | b);
            return (hashCode << 2) + elevatorFloor;
        }
    }
}
