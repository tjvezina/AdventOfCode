namespace AdventOfCode.Year2019.Day15
{
    [CoordSystem(CoordSystem.YUp)]
    public class Challenge : BaseChallenge
    {
        private RepairBot _bot;

        public override object part1ExpectedAnswer => 374;
        public override (string message, object answer) SolvePart1()
        {
            _bot = new RepairBot(inputList[0]);
            return ($"Oxygen system is {{0}} steps away at {_bot.goalPos.Value}.", _bot.goalDist);
        }
        
        public override object part2ExpectedAnswer => 482;
        public override (string message, object answer) SolvePart2()
        {
            return ("It will take {0} minutes to fill the region with oxygen.", _bot.timeToFill);
        }
    }
}
