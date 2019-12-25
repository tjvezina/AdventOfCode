using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day24 {
    public class Challenge : BaseChallenge {
        private CharMap _map;

        public override void InitPart1() {
            SpaceUtil.system = CoordSystem.YDown;
            _map = new CharMap(inputSet);
        }

        public override string part1Answer => "1113073";
        public override (string, object) SolvePart1() {
            HashSet<int> ratings = new HashSet<int>();

            int lastRating;
            while (ratings.Add((lastRating = CalculateBiodiversityRating()))) {
                Step();
            }

            int repeatIndex = 0;
            foreach (int rating in ratings) {
                if (rating == lastRating) break;
                ++repeatIndex;
            }

            return ($"Rating {{0}} repeated at #{repeatIndex} and #{ratings.Count}", lastRating);
        }
        
        public override string part2Answer => null;
        public override (string, object) SolvePart2() {
            return (null, null);
        }

        private void Step() {
            CharMap nextMap = new CharMap(_map);

            for (int y = 0; y < _map.height; ++y) {
                for (int x = 0; x < _map.width; ++x) {
                    Point p = new Point(x, y);
                    int bugNeighbors = EnumUtil.GetValues<Direction>().Select(d => p + d).Count(IsBug);

                    if (IsBug(p)) {
                        nextMap[p] = (bugNeighbors == 1 ? '#' : '.');
                    } else {
                        nextMap[p] = (bugNeighbors >= 1 && bugNeighbors <= 2 ? '#' : '.');
                    }
                }
            }

            _map = nextMap;
        }

        private int CalculateBiodiversityRating() {
            int rating = 0;

            for (int y = _map.height - 1; y >= 0; --y) {
                for (int x = _map.width - 1; x >= 0; --x) {
                    rating <<= 1;
                    if (_map[x, y] == '#') ++rating;
                }
            }

            return rating;
        }

        private bool IsBug(Point p) => IsValid(p) && _map[p] == '#';
        private bool IsValid(Point p) => p.x >= 0 && p.x < _map.width && p.y >= 0 && p.y < _map.height;
    }
}
