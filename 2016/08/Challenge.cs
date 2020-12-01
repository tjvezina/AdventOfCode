using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day08
{
    public class Challenge : BaseChallenge
    {
        private const int Width = 50;
        private const int Height = 6;
        private bool[,] _board = new bool[Width, Height];

        private IEnumerable<Operation> _operations;
        
        public Challenge() => _operations = inputList.Select(Operation.Parse);

        public override string part1ExpectedAnswer => "106";
        public override (string message, object answer) SolvePart1()
        {
            foreach (Operation operation in _operations)
            {
                operation.Execute(_board);
            }

            int litCount = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_board[x, y]) litCount++;
                }
            }

            return ("Lit pixels: ", litCount);
        }
        
        public override string part2ExpectedAnswer => "CFLELOYFCS";
        public override (string message, object answer) SolvePart2()
        {
            ASCIIArt.Draw(_board, doubleWidth:false);

            return ("Message: ", ASCIIArt.ImageToText(_board));
        }
    }
}
