using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Year2016.Day16
{
    public class Challenge : BaseChallenge
    {
        private const string Input = "00101000101111010";

        private const int Part1DiskSize = 272;
        private const int Part2DiskSize = 35651584;

        public override object part1ExpectedAnswer => "10010100110011100";
        public override (string message, object answer) SolvePart1()
        {
            List<bool> disk = Input.Select(c => c == '1').ToList();

            while (disk.Count < Part1DiskSize)
            {
                DragonCurve(disk);
            }

            if (disk.Count > Part1DiskSize)
            {
                disk.RemoveRange(Part1DiskSize, disk.Count - Part1DiskSize);
            }

            List<bool> checksum = Checksum(disk);

            return ("Checksum: ", checksum.Select(x => x ? "1" : "0").Aggregate((a, b) => $"{a}{b}"));
        }
        
        public override object part2ExpectedAnswer => null;
        public override (string message, object answer) SolvePart2()
        {
            return ("Checksum: ", DragonCurveOptimized(Part1DiskSize));
        }

        private void DragonCurve(IList<bool> data)
        {
            data.Add(false);
            for (int i = data.Count - 2; i >= 0; i--)
            {
                data.Add(!data[i]);
            }
        }

        private List<bool> Checksum(IList<bool> data)
        {
            while (data.Count % 2 == 0)
            {
                List<bool> checksum = new List<bool>(data.Count / 2);

                for (int i = 0; i < data.Count; i += 2)
                {
                    checksum.Add(data[i] == data[i + 1]);
                }

                data = checksum;
            }

            return new List<bool>(data);
        }

        private string DragonCurveOptimized(int diskSize)
        {
            int checksumSize = diskSize & -diskSize; // Largest power-of-two that divides into disk size
            int chunkSize = diskSize / checksumSize;

            long parity = 0;
            long cycleParity = 0;

            // Forwards
            for (int i = 0; i < Input.Length; i++)
            {
                parity ^= (Input[i] == '0' ? 0 : 1);
                cycleParity |= parity << i;
            }

            // Backwards inverted
            for (int i = Input.Length - 1; i >= 0; i--)
            {
                parity ^= (Input[i] == '0' ? 1 : 0);
                cycleParity |= parity << Input.Length + (Input.Length - i - 1);
            }

            long checksum = 0;
            int cycleLength = (Input.Length + 1) * 2;

            for (int i = 0; i < checksumSize; i++)
            {
                int start = i * chunkSize;
                int end = start + chunkSize - 1;

                int firstCycle = start / cycleLength;
                int lastCycle = end / cycleLength;
            }

            return new string(Convert.ToString(checksum, toBase: 2).Reverse().ToArray());
        }

        private long DragonParity(int start, int end) => Enumerable.Range(start, end - start + 1)
            .Select(i => DragonBit(i)).Aggregate((a, b) => a ^ b);

        private long DragonBit(long index) => (((index / (index & -index)) - 1) / 2) % 2;
    }
}
