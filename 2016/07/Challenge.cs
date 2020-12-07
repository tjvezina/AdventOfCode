using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day07
{
    public class Challenge : BaseChallenge
    {
        private const string PatternABA = @"(\w)(?=(?!\1)(\w)\1)";
        private const string PatternABBA = @"(\w)(?=(?!\1)(\w)\2\1)";
        private const string PatternHypernet = @"(?=[^\[]*\])";
        private const string PatternSupernet = @"(?![^\[]*\])";

        public override object part1ExpectedAnswer => 110;
        public override (string message, object answer) SolvePart1() => ("IP's with TLS support: ", inputList.Count(SupportsTLS));
        
        public override object part2ExpectedAnswer => 242;
        public override (string message, object answer) SolvePart2() => ("IP's with SSL support: ", inputList.Count(SupportsSSL));

        private bool SupportsTLS(string ip)
        {
            return Regex.IsMatch(ip, PatternABBA) &&
                  !Regex.IsMatch(ip, PatternABBA + PatternHypernet);
        }

        private bool SupportsSSL(string ip)
        {
            foreach (Match match in Regex.Matches(ip, PatternABA + PatternSupernet))
            {
                string patternBAB = string.Format("{1}{0}{1}", match.Groups[1], match.Groups[2]);
                if (Regex.IsMatch(ip, patternBAB + PatternHypernet))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
