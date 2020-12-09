namespace AdventOfCode.Year2019.Day11
{
    [CoordSystem(CoordSystem.YUp)]
    public class Challenge : BaseChallenge
    {
        private readonly PaintBot _paintBot;

        public Challenge() => _paintBot = new PaintBot(inputList[0]);

        public override object part1ExpectedAnswer => 1771;
        public override (string message, object answer) SolvePart1()
        {
            _paintBot.Run(firstTileIsWhite:false);
            return ("Tiles painted: ", _paintBot.paintedCount);
        }
        
        public override object part2ExpectedAnswer => "HGEHJHUZ";
        public override (string message, object answer) SolvePart2()
        {
            _paintBot.Run(firstTileIsWhite:true);
            bool[,] data = _paintBot.GetImage();

            ASCIIArt.Draw(data);
            return ("Image text: ", ASCIIArt.ImageToText(data));
        }
    }
}
