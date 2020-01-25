using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2019.Day24 {
    public partial class Challenge : BaseChallenge {
        private class BugMap {
            private readonly Point3 _size;
            private readonly Point3 _mid;
            private bool[,,] _map;
            private bool[,,] _buffer;

            public BugMap(string[] inputSet) {
                _size = new Point3(GridSize, GridSize, Steps + 3);
                _mid = _size / 2;
                _map = new bool[_size.x, _size.y, _size.z];
                _buffer = new bool[_size.x, _size.y, _size.z];
                for (int y = 0; y < _size.x; y++) {
                    for (int x = 0; x < _size.y; x++) {
                        _map[x, y, _mid.z] = (inputSet[y][x] == '#');
                    }
                }
            }

            public bool this[Point3 p] {
                get => this[p.x, p.y, p.z];
                set => this[p.x, p.y, p.z] = value;
            }
            public bool this[int x, int y, int z] {
                get =>    _map[x + _mid.x, y + _mid.y, z + _mid.z];
                set => _buffer[x + _mid.x, y + _mid.y, z + _mid.z] = value;
            }

            public void Swap() {
                bool[,,] temp = _map;
                _map = _buffer;
                _buffer = temp;
            }
        }

        private const int GridSize = 5;
        private const int Steps = 200;

        private Dictionary<Point, HashSet<Point3>> neighborMap;

        private BugMap _map3D;

        public override string part2ExpectedAnswer => "1928";
        public override (string message, object answer) SolvePart2() {
            _map3D = new BugMap(inputList.ToArray());

            BuildNeighborMap();

            for (int i = 0; i < Steps; i++) {
                Step3D();
            }

            int bugCount = 0;
            for (int z = -Steps / 2; z <= Steps / 2; z++) {
                for (int y = -GridSize / 2; y <= GridSize / 2; y++) {
                    for (int x = -GridSize / 2; x <= GridSize / 2; x++) {
                        if (_map3D[x, y, z]) bugCount++;
                    }
                }
            }

            return ($"Total bugs after {Steps} minutes: ", bugCount);
        }

        private void BuildNeighborMap() {
            neighborMap = new Dictionary<Point, HashSet<Point3>>();

            // Outer corner
            neighborMap[new Point(-2, -2)] = new HashSet<Point3> {
                new Point3(1, 0, 0), new Point3(0, 1, 0), new Point3(2, 1, -1), new Point3(1, 2, -1)
            };
            // Outer edges
            for (int x = -1; x <= 1; x++) {
                neighborMap[new Point(x, -2)] = new HashSet<Point3> {
                    new Point3(1, 0, 0), new Point3(0, 1, 0), new Point3(-1, 0, 0), new Point3(-x, 1, -1)
                };
            }
            // Inner space
            neighborMap[new Point(-1, -1)] = EnumUtil.GetValues<Direction>().Select(d => (Point3)(Point)d).ToHashSet();
            // Inner edge
            neighborMap[new Point(0, -1)] = new HashSet<Point3> {
                new Point3(1, 0, 0), new Point3(-1, 0, 0), new Point3(0, -1, 0),
                new Point3(-2, -1, 1),
                new Point3(-1, -1, 1),
                new Point3( 0, -1, 1),
                new Point3( 1, -1, 1),
                new Point3( 2, -1, 1)
            };

            List<KeyValuePair<Point, HashSet<Point3>>> prevSet = new List<KeyValuePair<Point, HashSet<Point3>>>(neighborMap);
            for (int i = 0; i < 3; i++) {
                List<KeyValuePair<Point, HashSet<Point3>>> nextSet = new List<KeyValuePair<Point, HashSet<Point3>>>();
                foreach (KeyValuePair<Point, HashSet<Point3>> pair in prevSet) {
                    Point rotatedKey = new Point(-pair.Key.y, pair.Key.x);
                    HashSet<Point3> rotatedSet = pair.Value.Select(p => new Point3(-p.y, p.x, p.z)).ToHashSet();
                    nextSet.Add(new KeyValuePair<Point, HashSet<Point3>>(rotatedKey, rotatedSet));
                    neighborMap.Add(rotatedKey, rotatedSet);
                }
                prevSet = nextSet;
            }
        }

        private void Step3D() {
            for (int z = -Steps / 2; z <= Steps / 2; z++) {
                for (int y = -GridSize / 2; y <= GridSize / 2; y++) {
                    for (int x = -GridSize / 2; x <= GridSize / 2; x++) {
                        if (x == 0 && y == 0) continue;
                        Point3 p = new Point3(x, y, z);
                        int bugNeighbors = neighborMap[(Point)p].Select(n => _map3D[p + n]).Count(b => b);

                        _map3D[p] = bugNeighbors == 1 || (!_map3D[p] && bugNeighbors == 2);
                    }
                }
            }

            _map3D.Swap();
        }
    }
}
