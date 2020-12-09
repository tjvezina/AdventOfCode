using System.Linq;

namespace AdventOfCode.Year2015.Day18
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 814;
        public override (string message, object answer) SolvePart1()
        {
            LightBoard board = new LightBoard(inputList.ToArray());

            for (int i = 0; i < 100; i++)
            {
                board.Update();
            }

            return ("Lights on: ", board.litCount);
        }
        
        public override object part2ExpectedAnswer => 924;
        public override (string message, object answer) SolvePart2()
        {
            LightBoard board = new LightBoard(inputList.ToArray(), cornersAlwaysOn:true);

            for (int i = 0; i < 100; i++)
            {
                board.Update();
            }

            return ("Lights on: ", board.litCount);
        }
    }
}
