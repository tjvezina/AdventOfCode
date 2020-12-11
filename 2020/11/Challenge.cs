using System.Linq;

namespace AdventOfCode.Year2020.Day11
{
    public class Challenge : BaseChallenge
    {
        private delegate int CountNeighborsFunc(CharMap map, int x, int y);

        private const char Seat = 'L';
        private const char Person = '#';

        private CountNeighborsFunc _countNeighbors;
        private int _neighborLimit;

        public override object part1ExpectedAnswer => 2359;
        public override (string message, object answer) SolvePart1()
        {
            _countNeighbors = CountNeighbors;
            _neighborLimit = 4;

            (int personCount, int roundCount) = StabilizeSeating();

            return ($"After {roundCount} rounds there are {{0}} seats occupied", personCount);
        }
        
        public override object part2ExpectedAnswer => 2131;
        public override (string message, object answer) SolvePart2()
        {
            _countNeighbors = CountNeighborsInSight;
            _neighborLimit = 5;

            (int personCount, int roundCount) = StabilizeSeating();

            return ($"After {roundCount} rounds there are {{0}} seats occupied", personCount);
        }

        private (int personCount, int roundCount) StabilizeSeating()
        {
            CharMap map = new CharMap(inputList.ToArray());

            int roundCount = 0;
            while (ExecuteRound(ref map))
            {
                roundCount++;
            }

            int personCount = map.Count(pos => pos.c == Person);

            return (personCount, roundCount);
        }

        private bool ExecuteRound(ref CharMap map)
        {
            CharMap result = new CharMap(map);
            bool wasSeatChanged = false;

            foreach ((int x, int y, char c) in map)
            {
                int neighborCount = _countNeighbors(map, x, y);

                if (c == Seat && neighborCount == 0)
                {
                    result[x, y] = Person;
                    wasSeatChanged = true;
                }
                else if (c == Person && neighborCount >= _neighborLimit)
                {
                    result[x, y] = Seat;
                    wasSeatChanged = true;
                }
            }

            map = result;
            return wasSeatChanged;
        }

        private int CountNeighbors(CharMap map, int x, int y)
        {
            int count = 0;

            for (int yDelta = -1; yDelta <= 1; yDelta++)
            {
                for (int xDelta = -1; xDelta <= 1; xDelta++)
                {
                    if (yDelta == 0 && xDelta == 0) continue;

                    int xNeighbor = x + xDelta;
                    int yNeighbor = y + yDelta;

                    if (xNeighbor >= 0 && xNeighbor < map.width && yNeighbor >= 0 && yNeighbor < map.height)
                    {
                        count += (map[xNeighbor, yNeighbor] == Person ? 1 : 0);
                    }
                }
            }

            return count;
        }

        private int CountNeighborsInSight(CharMap map, int x, int y)
        {
            int count = 0;

            for (int yDelta = -1; yDelta <= 1; yDelta++)
            {
                for (int xDelta = -1; xDelta <= 1; xDelta++)
                {
                    if (xDelta == 0 && yDelta == 0) continue;

                    int xNeighbor = x + xDelta;
                    int yNeighbor = y + yDelta;

                    while (xNeighbor >= 0 && xNeighbor < map.width && yNeighbor >= 0 && yNeighbor < map.height)
                    {
                        char neighbor = map[xNeighbor, yNeighbor];

                        if (neighbor == Person)
                        {
                            count++;
                            break;
                        }
                        if (neighbor == Seat)
                        {
                            break;
                        }

                        xNeighbor += xDelta;
                        yNeighbor += yDelta;
                    }

                }
            }

            return count;
        }
    }
}
