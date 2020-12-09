using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day12
{
    public class Challenge : BaseChallenge
    {
        private struct Body
        {
            public Point3 position;
            public Point3 velocity;

            public int potentialEnergy => Math.Abs(position.x) + Math.Abs(position.y) + Math.Abs(position.z);
            public int kineticEnergy => Math.Abs(velocity.x) + Math.Abs(velocity.y) + Math.Abs(velocity.z);
            public int totalEnergy => potentialEnergy * kineticEnergy;
        }

        private class AxisData
        {
            public int initial;
            public int pos;
            public int vel;

            public bool HasLooped => vel == 0 && pos == initial;

            public AxisData(int initial)
            {
                this.initial = initial;
                pos = initial;
                vel = 0;
            }

            public void UpdateVelocity(IEnumerable<AxisData> axisData)
            {
                vel += axisData.Select(d => d.pos - pos).Sum(d => d == 0 ? 0 : d / Math.Abs(d));
            }

            public void UpdatePosition() => pos += vel;
        }

        private readonly List<Body> _bodies = new List<Body>();
        private readonly List<Point3> _initialBodyPositions;
        private List<AxisData> _axisData;

        public Challenge()
        {
            foreach (string data in inputList)
            {
                string[] parts = data.Split(',');
                Point3 pos = new Point3(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
                _bodies.Add(new Body { position = pos });
            }

            _initialBodyPositions = _bodies.Select(b => b.position).ToList();
        }

        public override object part1ExpectedAnswer => 6849;
        public override (string message, object answer) SolvePart1()
        {
            for (int i = 0; i < 1000; i++)
            {
                UpdateVelocities();
                UpdatePositions();
            }

            int totalEnergy = _bodies.Sum(b => b.totalEnergy);
            return ("Total energy: ", totalEnergy);
        }
        
        public override object part2ExpectedAnswer => 356658899375688;
        public override (string message, object answer) SolvePart2()
        {
            long[] cycleLengths = new long[3];

            // Each axis is independent; brute-force simulate to determine how long each takes to repeat
            for (int axis = 0; axis < 3; axis++)
            {
                cycleLengths[axis] = CalculateCycleLength(axis);
            }

            // Find the LCM of all 3 axes; the total number of steps before all bodies are back where they started
            long totalSteps = MathUtil.LCM(cycleLengths[0], MathUtil.LCM(cycleLengths[1], cycleLengths[2]));

            return ("System cycle length: ", totalSteps);
        }

        private void UpdateVelocities()
        {
            for (int i = 0; i < _bodies.Count; i++)
            {
                Body body = _bodies[i];
                Point3 newVelocity = Point3.zero;
                for (int a = 0; a < 3; a++)
                {
                    newVelocity[a] = _bodies.Select(b => b.position[a] - body.position[a])
                                            .Sum(d => d == 0 ? 0 : d / Math.Abs(d));
                }
                body.velocity += newVelocity;
                _bodies[i] = body;
            }
        }

        private void UpdatePositions()
        {
            for (int i = 0; i < _bodies.Count; i++)
            {
                Body body = _bodies[i];
                body.position += body.velocity;
                _bodies[i] = body;
            }
        }

        private long CalculateCycleLength(int axis)
        {
            _axisData = _initialBodyPositions.Select(p => new AxisData(p[axis])).ToList();

            bool HasLooped() => _axisData.All(d => d.vel == 0 && d.pos == d.initial);

            long cycle = 0;
            do
            {
                _axisData.ForEach(d => d.UpdateVelocity(_axisData));
                _axisData.ForEach(d => d.UpdatePosition());
                cycle++;
            } while (!HasLooped());

            return cycle;
        }
    }
}
