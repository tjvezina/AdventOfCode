using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day20
{
    public class Challenge : BaseChallenge
    {
        private Tile[] _tiles;

        public override object part1ExpectedAnswer => 17250897231301;
        public override (string message, object answer) SolvePart1()
        {
            _tiles = inputFile.Split("\n\n").Select(x => new Tile(x.Split('\n'))).ToArray();

            Dictionary<int, Tile> uniqueCodes = new Dictionary<int, Tile>();

            foreach (Tile tile in _tiles)
            {
                foreach (int sideCode in tile.sideCodes)
                {
                    if (uniqueCodes.ContainsKey(sideCode))
                    {
                        uniqueCodes.Remove(sideCode);
                    }
                    else
                    {
                        uniqueCodes[sideCode] = tile;
                    }
                }
            }

            Tile[] cornerTiles = uniqueCodes.Values.GroupBy(x => x).Where(x => x.Count() == 2).Select(x => x.First()).ToArray();

            Console.WriteLine(_tiles.Length);
            Console.WriteLine(uniqueCodes.Count);
            Console.WriteLine(cornerTiles.Length);

            return ("Product of corner tile ID's: ", cornerTiles.Select(x => (long)x.id).Aggregate((a, b) => a * b));
        }
        
        public override object part2ExpectedAnswer => null;
        public override (string message, object answer) SolvePart2()
        {
            return ("", null);
        }
    }
}
