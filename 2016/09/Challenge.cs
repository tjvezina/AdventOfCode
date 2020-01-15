using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day09 {
    public class Challenge : BaseChallenge {
        public override string part1Answer => "97714";
        public override (string, object) SolvePart1() {
            return ("Decompressed length: ", GetDecompressedLength(input));
        }
        
        public override string part2Answer => "10762972461";
        public override (string, object) SolvePart2() {
            return ("Decompressed length (recursive): ", GetDecompressedLength(input, recursive:true));
        }

        private long GetDecompressedLength(string compressed, bool recursive = false) {
            long length = 0;

            int index = 0;
            foreach (Match marker in Regex.Matches(compressed, @"\((\d+)x(\d+)\)")) {
                if (marker.Index < index) continue;

                length += (marker.Index - index);

                int repeatStart = marker.Index + marker.Length;
                int repeatLength = int.Parse(marker.Groups[1].Value);
                int repeatCount = int.Parse(marker.Groups[2].Value);
                if (recursive) {
                    string substring = compressed.Substring(repeatStart, repeatLength);
                    length += GetDecompressedLength(substring, true) * repeatCount;
                }
                else {
                    length += repeatLength * repeatCount;
                }

                index = repeatStart + repeatLength;
            }

            length += (compressed.Length - index);

            return length;
        }
    }
}
