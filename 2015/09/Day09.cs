using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015 {
    public class Day09 : Challenge {
        public class DistSubMap : Dictionary<int, int> { }
        public class DistMap : Dictionary<int, DistSubMap> { }

        private List<string> _cities = new List<string>();
        private DistMap _distMap = new DistMap();

        private int[] _order;
        private int[] _bestOrder;
        private int _bestDist = int.MaxValue;

        private void Init(string[] input) {
            foreach (string data in input) {
                string[] elements = data.Split(' ');

                string cityA = elements[0];
                string cityB = elements[2];
                int distance = int.Parse(elements[4]);

                if (!_cities.Contains(cityA)) {
                    _cities.Add(cityA);
                }
                if (!_cities.Contains(cityB)) {
                    _cities.Add(cityB);
                }

                int indexA = _cities.IndexOf(cityA);
                int indexB = _cities.IndexOf(cityB);

                if (!_distMap.ContainsKey(indexA)) {
                    _distMap[indexA] = new DistSubMap();
                }
                if (!_distMap.ContainsKey(indexB)) {
                    _distMap[indexB] = new DistSubMap();
                }

                _distMap[indexA][indexB] = distance;
                _distMap[indexB][indexA] = distance;
            }

            Reset();
        }

        protected override void Reset() {
            _order = new int[_cities.Count];
            _bestOrder = new int[_cities.Count];

            for (int i = 0; i < _cities.Count; ++i) {
                _order[i] = i;
            }
        }

        protected override string SolvePart1() {
            bool IsShorter(int dist, int best) => dist < best;
            
            _bestDist = int.MaxValue;
            FindBestOrder(condition:IsShorter);
            return $"Shortest route: {_bestDist}";
        }
        
        protected override string SolvePart2() {
            bool IsLonger(int dist, int best) => dist > best;
            
            _bestDist = 0;
            FindBestOrder(condition:IsLonger);
            return $"Shortest route: {_bestDist}";
        }

        private void FindBestOrder(Func<int, int, bool> condition) {
            do {
                int dist = 0;

                int prevCity = _order[0];
                for (int i = 1; i < _cities.Count; ++i) {
                    int nextCity = _order[i];

                    dist += _distMap[prevCity][nextCity];

                    prevCity = nextCity;
                }

                if (condition(dist, _bestDist)) {
                    _bestDist = dist;

                    for (int i = 0; i < _cities.Count; ++i) {
                        _bestOrder[i] = _order[i];
                    }
                }
            } while (DataUtil.NextPermutation(_order));
        }
    }
}
