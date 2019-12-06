using System;
using System.Collections.Generic;
using System.IO;

public class DistSubMap : Dictionary<int, int> { }
public class DistMap : Dictionary<int, DistSubMap> { }

public static class Program {
    private static List<string> _cities = new List<string>();
    private static DistMap _distMap = new DistMap();

    private static int[] _order;
    private static int[] _longestOrder;
    private static int _longestDist;

    private static void Main(string[] args) {
        BuildDistMap(File.ReadAllLines("input.txt"));
        Initialize();
        FindLongestOrder();

        Console.WriteLine(_longestDist);
    }

    private static void BuildDistMap(string[] input) {
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
    }

    private static void Initialize() {
        _order = new int[_cities.Count];
        _longestOrder = new int[_cities.Count];

        for (int i = 0; i < _cities.Count; ++i) {
            _order[i] = i;
        }
    }

    private static void FindLongestOrder() {
        do {
            CheckOrderDist();
        } while (NextPermutation());
    }

    private static void CheckOrderDist() {
        int dist = 0;

        int prevCity = _order[0];
        for (int i = 1; i < _cities.Count; ++i) {
            int nextCity = _order[i];

            dist += _distMap[prevCity][nextCity];

            prevCity = nextCity;
        }

        if (dist > _longestDist) {
            _longestDist = dist;

            for (int i = 0; i < _cities.Count; ++i) {
                _longestOrder[i] = _order[i];
            }
        }
    }

    private static bool NextPermutation() {
        // Find greatest index x, where p[x] < p[x+1]
        int x = -1;
        for (int i = _order.Length - 2; i>= 0; --i) {
            if (_order[i] < _order[i+1]) {
                x = i;
                break;
            }
        }

        // Final permutation reached
        if (x == -1) {
            return false;
        }

        // Find greatest index y, where p[x] < p[y]
        int y = -1;
        for (int i = _order.Length - 1; i >= x + 1; --i) {
            if (_order[x] < _order[i]) {
                y = i;
                break;
            }
        }
        
        // Swap p[x] and p[y]
        int hold = _order[y];
        _order[y] = _order[x];
        _order[x] = hold;

        // Reverse elements from p[x+1]..p[n]
        int[] reverse = new int[_order.Length - 1 - x];
        for (int i = 0; i < reverse.Length; ++i) {
            reverse[i] = _order[x + 1 + i];
        }
        for (int i = 0; i < reverse.Length; ++i) {
            _order[_order.Length - 1 - i] = reverse[i];
        }

        return true;
    }
}
