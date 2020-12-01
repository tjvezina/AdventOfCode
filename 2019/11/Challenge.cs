namespace AdventOfCode.Year2019.Day11
{
     public class Challenge : BaseChallenge
     {
        public override CoordSystem? coordSystem => CoordSystem.YUp;

        private readonly PaintBot _paintBot;

        public Challenge() => _paintBot = new PaintBot(inputList[0]);

        public override string part1ExpectedAnswer => "1771";
        public override (string message, object answer) SolvePart1()
        {
            _paintBot.Run(firstTileIsWhite:false);
            return ("Tiles painted: ", _paintBot.paintedCount);
        }
        
        public override string part2ExpectedAnswer => "HGEHJHUZ";
        public override (string message, object answer) SolvePart2()
        {
            _paintBot.Run(firstTileIsWhite:true);
            bool[,] data = _paintBot.GetImage();

            ASCIIArt.Draw(data);
            return ("Image text: ", ASCIIArt.ImageToText(data));
        }
    }
}
