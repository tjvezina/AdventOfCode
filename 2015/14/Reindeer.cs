using System;

namespace AdventOfCode.Year2015.Day14 {
    public class Reindeer {
        public int speed { get; }
        public int flyTime { get; }
        public int cycleTime { get; }

        public Reindeer(int speed, int flyTime, int restTime) {
            this.speed = speed;
            this.flyTime = flyTime;
            cycleTime = flyTime + restTime;
        }

        public int GetDistance(int time) {
            int distPerCycle = speed * flyTime;

            int dist = (time / cycleTime) * distPerCycle;
            
            int remain = time % cycleTime;
            dist += Math.Min(remain, flyTime) * speed;

            return dist;
        }
    }
}
