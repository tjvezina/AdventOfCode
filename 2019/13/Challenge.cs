namespace AdventOfCode.Year2019.Day13
{
     public class Challenge : BaseChallenge
     {
        private readonly string _intCodeMemory;

        public Challenge() => _intCodeMemory = inputList[0];

        public override object part1ExpectedAnswer => 452;
        public override (string message, object answer) SolvePart1()
        {
            Arcade arcade = new Arcade(_intCodeMemory);
            return ("{0} blocks on screen", arcade.blockCount);
        }
        
        public override object part2ExpectedAnswer => 21415;
        public override (string message, object answer) SolvePart2()
        {
            Arcade arcade = new Arcade(_intCodeMemory, insertQuarters:true);
            return ("Final score: ", arcade.score);
        }
    }
}
