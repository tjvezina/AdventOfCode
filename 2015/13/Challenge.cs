using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2015.Day13 {
     public class Challenge : BaseChallenge {
        private MatrixInt _happyMatrix;

        private int[] _order;
        private int[] _bestOrder;
        private int _maxHappiness;

        private void Reset() {
            _maxHappiness = int.MinValue;
        }

        public override void InitPart1() => Reset();

        public override string part1Answer => "709";
        public override (string, object) SolvePart1() {
            BuildHappyMatrix();
            FindBestOrder();

            string orderStr = _bestOrder.Select(i => $"{i}").Aggregate((a, b) => $"{a},{b}");
            return ($"Max happiness: {{0}} ({orderStr})", _maxHappiness);
        }

        public override void InitPart2() => Reset();
        
        public override string part2Answer => "668";
        public override (string, object) SolvePart2() {
            BuildHappyMatrix(includeSelf:true);
            FindBestOrder();

            string orderStr = _bestOrder.Select(i => $"{i}").Aggregate((a, b) => $"{a},{b}");
            return ($"Max happiness: {{0}} ({orderStr})", _maxHappiness);
        }

        private void BuildHappyMatrix(bool includeSelf = false) {
            List<string> guests = inputSet.Select(l => l.Substring(0, l.IndexOf(' '))).Distinct().ToList();
            _happyMatrix = new MatrixInt(guests.Count + (includeSelf ? 1 : 0));

            foreach (string data in inputSet) {
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

            _bestOrder = new int[_happyMatrix.rows];
            _order = new int[_happyMatrix.rows];

            for (int i = 0; i < _order.Length; ++i) {
                _order[i] = i;
            }
        }

        private void FindBestOrder() {
            do {
                CheckOrder();
            } while (DataUtil.NextPermutation(_order));
        }

        private void CheckOrder() {
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
    }
}
