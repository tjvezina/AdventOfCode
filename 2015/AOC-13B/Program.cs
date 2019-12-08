using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static MatrixInt _happyMatrix;

    private static int[] _order;
    private static int[] _bestOrder;
    private static int _maxHappiness = int.MinValue;

    private static void Main(string[] args) {
        BuildHappyMatrix();
        Init();
        FindBestOrder();
    }

    private static void BuildHappyMatrix() {
        string[] input = File.ReadAllLines("input.txt");

        List<string> guests = input.Select(l => l.Substring(0, l.IndexOf(' '))).Distinct().ToList();
        _happyMatrix = new MatrixInt(guests.Count + 1); // Include yourself

        foreach (string data in input) {
            string[] parts = data.Split(' ');
            string guestA = parts[0];
            string guestB = parts[parts.Length - 1];
            guestB = guestB.Substring(0, guestB.Length - 1); // Trim period
            int points = int.Parse(parts[3]);
            points *= (parts[2] == "gain" ? 1 : -1);

            int indexA = guests.IndexOf(guestA);
            int indexB = guests.IndexOf(guestB);

            _happyMatrix[indexA, indexB] = points;
        }
    }

    private static void Init() {
        _bestOrder = new int[_happyMatrix.rows];
        _order = new int[_happyMatrix.rows];

        for (int i = 0; i < _order.Length; ++i) {
            _order[i] = i;
        }
    }

    private static void FindBestOrder() {
        do {
            CheckOrder();
        } while (NextPermutation());

        string order = _bestOrder.Select(i => $"{i}").Aggregate((a, b) => $"{a},{b}");

        Console.WriteLine($"Max happiness: {_maxHappiness} ({order})");
    }

    private static void CheckOrder() {
        int totalHappiness = 0;

        for (int i = 0; i < _order.Length; ++i) {
            int guest = _order[i];
            int neighborA = _order[(i > 0 ? i - 1 : _order.Length - 1)];
            int neighborB = _order[(i < _order.Length - 1 ? i + 1 : 0)];

            totalHappiness += _happyMatrix[guest, neighborA];
            totalHappiness += _happyMatrix[guest, neighborB];
        }

        if (_maxHappiness < totalHappiness) {
            _maxHappiness = totalHappiness;
            _order.CopyTo(_bestOrder, 0);
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
