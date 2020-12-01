using System.Linq;

namespace AdventOfCode.Year2015.Day23
{
    public class Challenge : BaseChallenge
    {
        public override string part1ExpectedAnswer => "307";
        public override (string message, object answer) SolvePart1()
        {
            Computer computer = new Computer(inputList.ToArray());
            computer.Run();

            return ("Register B: ", computer["b"]);
        }
        
        public override string part2ExpectedAnswer => "160";
        public override (string message, object answer) SolvePart2()
        {
            Computer computer = new Computer(inputList.ToArray());
            computer["a"] = 1;
            computer.Run();

            return ("Register B: ", computer["b"]);
        }
    }
}
