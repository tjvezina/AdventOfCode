using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Year2016.Day04 {
    public class Challenge : BaseChallenge {
        private List<Room> _rooms;

        public override void InitPart1() => _rooms = inputSet.Select(i => new Room(i)).Where(r => r.isValid).ToList();

        public override string part1Answer => "158835";
        public override (string, object) SolvePart1() {
            return ("Sum of sector ID's of valid rooms: ", _rooms.Sum(r => r.sectorID));
        }
        
        public override string part2Answer => "993";
        public override (string, object) SolvePart2() {
            foreach (Room room in _rooms) {
                room.Decrypt();
            }

            Room northPoleRoom = _rooms.Single(r => r.name.Contains("northpole"));

            return ($"{northPoleRoom.name} sector ID: ", northPoleRoom.sectorID);
        }
    }
}
