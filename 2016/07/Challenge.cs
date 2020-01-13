using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day07 {
    public class Challenge : BaseChallenge {
        private const string PatternABA = @"(\w)(?=(?!\1)(\w)\1)";
        private const string PatternABBA = @"(\w)(?=(?!\1)(\w)\2\1)";
        private const string PatternHypernet = @"(?=[^\[]*\])";
        private const string PatternSupernet = @"(?![^\[]*\])";

        public override void InitPart1() { }

        public override string part1Answer => "110";
        public override (string, object) SolvePart1() => ("IP's with TLS support: ", inputSet.Count(SupportsTLS));
        
        public override string part2Answer => "242";
        public override (string, object) SolvePart2() => ("IP's with SSL support: ", inputSet.Count(SupportsSSL));

        private bool SupportsTLS(string ip) {
            return Regex.IsMatch(ip, PatternABBA) &&
                  !Regex.IsMatch(ip, PatternABBA + PatternHypernet);
        }

        private bool SupportsSSL(string ip) {
            foreach (Match match in Regex.Matches(ip, PatternABA + PatternSupernet)) {
                string patternBAB = string.Format("{1}{0}{1}", match.Groups[1], match.Groups[2]);
                if (Regex.IsMatch(ip, patternBAB + PatternHypernet)) {
                    return true;
                }
            }
            return false;
        }
    }
}
