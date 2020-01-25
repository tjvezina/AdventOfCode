using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2016.Day04 {
    public class Challenge : BaseChallenge {
        private List<Room> _rooms;

        public override string part1ExpectedAnswer => "158835";
        public override (string message, object answer) SolvePart1() {
            _rooms = inputList.Select(i => new Room(i)).Where(r => r.isValid).ToList();
            
            return ("Sum of sector ID's of valid rooms: ", _rooms.Sum(r => r.sectorID));
        }
        
        public override string part2ExpectedAnswer => "993";
        public override (string message, object answer) SolvePart2() {
            foreach (Room room in _rooms) {
                room.Decrypt();
            }

            Room northPoleRoom = _rooms.Single(r => r.name.Contains("northpole"));

            return ($"{northPoleRoom.name} sector ID: ", northPoleRoom.sectorID);
        }
    }
}
