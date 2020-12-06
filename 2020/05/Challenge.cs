using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day05
{
    public class Challenge : BaseChallenge
    {
        private class Seat
        {
            public static Seat Parse(string data)
            {
                int row = 0;
                int column = 0;

                for (int i = 0; i < 7; i++)
                {
                    row += (data[6-i] == 'B' ? 1 : 0) << i;
                }

                for (int i = 0; i < 3; i++)
                {
                    column += (data[9-i] == 'R' ? 1 : 0) << i;
                }

                return new Seat(row, column);
            }

            private Seat(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            public int row { get; }
            public int column { get; }

            public int seatID => row * 8 + column;
        }

        private List<Seat> _seats;

        public override string part1ExpectedAnswer => "866";
        public override (string message, object answer) SolvePart1()
        {
            _seats = inputList.Select(Seat.Parse).OrderBy(x => x.seatID).ToList();

            return ("Highest seat ID: ", _seats.Last().seatID);
        }
        
        public override string part2ExpectedAnswer => "583";
        public override (string message, object answer) SolvePart2()
        {
            int lastID = _seats[0].seatID;

            for (int i = 1; i < _seats.Count; i++)
            {
                int nextID = _seats[i].seatID;

                if (lastID + 2 == nextID)
                {
                    return ($"My seat is #{{0}} (row {_seats[i].row} / column {_seats[i].column})", lastID + 1);
                }

                lastID = nextID;
            }

            throw new Exception("Failed to find a missing seat with filled neighbors!");
        }
    }
}
