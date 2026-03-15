using System;
using System.Linq;

namespace AdventOfCode.Year2020.Day20
{
    public class Tile
    {
        public int id { get; }
        private readonly CharMap _imageData;

        public string[] sides;
        public int[] sideCodes;

        public Tile(string[] input)
        {
            id = int.Parse(input[0].Substring(5, 4));
            _imageData = new CharMap(input.Skip(1).ToArray());

            sides = new []
            {
                new string(Enumerable.Range(0, _imageData.width).Select(x => _imageData[x, 0]).ToArray()),
                new string(Enumerable.Range(0, _imageData.height).Select(x => _imageData[_imageData.width - 1, x]).ToArray()),
                new string(Enumerable.Range(0, _imageData.width).Select(x => _imageData[_imageData.width - 1 - x, _imageData.height - 1]).ToArray()),
                new string(Enumerable.Range(0, _imageData.height).Select(x => _imageData[0, _imageData.height - 1 - x]).ToArray())
            };

            sideCodes = new int[sides.Length];
            for (int i = 0; i < sides.Length; i++)
            {
                string binary = sides[i].Replace('.', '0').Replace('#', '1');
                int codeA = Convert.ToInt32(binary, fromBase:2);
                int codeB = Convert.ToInt32(new string(binary.Reverse().ToArray()), fromBase: 2);
                sideCodes[i] = Math.Min(codeA, codeB);
            }
        }
    }
}
