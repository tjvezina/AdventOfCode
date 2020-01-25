using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day14 {
     public class Challenge : BaseChallenge {
        private const int RaceDuration = 2503;
        private const string Pattern = @"can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds";

        private List<Reindeer> _reindeer = new List<Reindeer>();

        public Challenge() {
            foreach (string data in inputList) {
                Match match = Regex.Match(data, Pattern);
                int speed = int.Parse(match.Groups[1].Value);
                int flyTime = int.Parse(match.Groups[2].Value);
                int restTime = int.Parse(match.Groups[3].Value);

                _reindeer.Add(new Reindeer(speed, flyTime, restTime));
            }
        }

        public override string part1ExpectedAnswer => "2640";
        public override (string message, object answer) SolvePart1() {
            return ("Furthest dist: ", _reindeer.Max(r => r.GetDistance(RaceDuration)));
        }
        
        public override string part2ExpectedAnswer => "1102";
        public override (string message, object answer) SolvePart2() {
            List<int> distances = new List<int>(new int[_reindeer.Count]);
            List<int> scores = new List<int>(new int[_reindeer.Count]);

            for (int t = 1; t <= RaceDuration; t++) {
                // Calculate distances for the next second of the race
                for (int i = 0; i < _reindeer.Count; i++) {
                    Reindeer r = _reindeer[i];
                    if ((t - 1) % r.cycleTime < r.flyTime) {
                        distances[i] += r.speed;
                    }
                }

                // Award leaders 1 point
                int lead = distances.Max();
                for (int i = 0; i < distances.Count; i++) {
                    if (distances[i] == lead) {
                        scores[i]++;
                    }
                }
            }

            return ("Winning score: ", scores.Max());
        }
    }
}
