using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day09
{
    public class Challenge : BaseChallenge
    {
        private readonly string _input;

        public Challenge() => _input = inputList[0];

        public override object part1ExpectedAnswer => 97714;
        public override (string message, object answer) SolvePart1()
        {
            return ("Decompressed length: ", GetDecompressedLength(_input));
        }
        
        public override object part2ExpectedAnswer => 10762972461;
        public override (string message, object answer) SolvePart2()
        {
            return ("Decompressed length (recursive): ", GetDecompressedLength(_input, recursive:true));
        }

        private long GetDecompressedLength(string compressed, bool recursive = false)
        {
            long length = 0;

            int index = 0;
            foreach (Match marker in Regex.Matches(compressed, @"\((\d+)x(\d+)\)"))
            {
                if (marker.Index < index) continue;

                length += (marker.Index - index);

                int repeatStart = marker.Index + marker.Length;
                int repeatLength = int.Parse(marker.Groups[1].Value);
                int repeatCount = int.Parse(marker.Groups[2].Value);
                if (recursive)
                {
                    string substring = compressed.Substring(repeatStart, repeatLength);
                    length += GetDecompressedLength(substring, true) * repeatCount;
                }
                else
                {
                    length += repeatLength * repeatCount;
                }

                index = repeatStart + repeatLength;
            }

            length += (compressed.Length - index);

            return length;
        }
    }
}
