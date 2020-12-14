using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day13
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 2406;
        public override (string message, object answer) SolvePart1()
        {
            int startTime = int.Parse(inputList[0]);
            int[] busIDs = inputList[1].Split(',').Where(x => x != "x").Select(int.Parse).ToArray();

            (int busID, int wait) = busIDs.Select(x => (x, wait: x - startTime % x)).OrderBy(x => x.wait).First();

            return ($"Bus {busID} will arrive in {wait} minutes = ", busID * wait);
        }
        
        public override object part2ExpectedAnswer => 225850756401039;
        public override (string message, object answer) SolvePart2()
        {
            List<(int busID, int offset)> schedule = new List<(int, int)>();

            string[] parts = inputList[1].Split(',');
            for (int i = 0; i < parts.Length; i++)
            {
                if (int.TryParse(parts[i], out int busID))
                {
                    schedule.Add((busID, i));
                }
            }

            long step = 1;
            (int lastBusID, long startTime) = schedule.First();

            foreach ((int busID, int offset) in schedule.Skip(1))
            {
                // In this case, checking GCD is unnecessary as all bus ID's are primes, but that is not a requirement
                step *= lastBusID / MathUtil.GCD(step, lastBusID);
                Console.WriteLine($"Step x {lastBusID,3} = {step,19:N0} | t = {startTime:N0}");

                while ((startTime + offset) % busID != 0)
                {
                    startTime += step;
                }

                lastBusID = busID;
            }

            return ("The first valid start time is ", startTime);
        }
    }
}
