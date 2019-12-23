using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day14 {
     public class Challenge : BaseChallenge {
        private const int RACE_DURATION = 2503;

        private List<Reindeer> _reindeer = new List<Reindeer>();

        public override void InitPart1() {
            foreach (string data in inputSet) {
                string[] parts = data.Split(' ');
                int speed = int.Parse(parts[3]);
                int flyTime = int.Parse(parts[6]);
                int restTime = int.Parse(parts[13]);

                _reindeer.Add(new Reindeer(speed, flyTime, restTime));
            }
        }

        public override string SolvePart1() {
            return $"Furthest dist: {_reindeer.Max(r => r.GetDistance(RACE_DURATION))}";
        }
        
        public override string SolvePart2() {
            List<int> distances = new List<int>(new int[_reindeer.Count]);
            List<int> scores = new List<int>(new int[_reindeer.Count]);

            for (int t = 1; t <= RACE_DURATION; ++t) {
                // Calculate distances for the next second of the race
                for (int i = 0; i < _reindeer.Count; ++i) {
                    Reindeer r = _reindeer[i];
                    if ((t - 1) % r.cycleTime < r.flyTime) {
                        distances[i] += r.speed;
                    }
                }

                // Award leaders 1 point
                int lead = distances.Max();
                for (int i = 0; i < distances.Count; ++i) {
                    if (distances[i] == lead) {
                        ++scores[i];
                    }
                }
            }

            return $"Winning score: {scores.Max()}";
        }
    }
}
