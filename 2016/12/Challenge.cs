using System.Linq;

namespace AdventOfCode.Year2016.Day12
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 318007;
        public override (string message, object answer) SolvePart1()
        {
            Computer computer = new Computer(inputList.ToArray());
            computer.Run();

            return ("Password: ", computer["a"]);
        }
        
        public override object part2ExpectedAnswer => 9227661;
        public override (string message, object answer) SolvePart2()
        {
            Computer computer = new Computer(inputList.ToArray());
            computer["c"] = 1;
            computer.Run();

            return ("Password: ", computer["a"]);
        }
    }
}
