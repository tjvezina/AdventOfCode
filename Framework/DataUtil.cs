using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode {
    public static class DataUtil {
        public static void Swap<T>(ref T a, ref T b) {
            T hold = a;
            a = b;
            b = hold;
        }

        public static void Reverse<T>(this IList<T> array, int start = 0) => Reverse<T>(array, start, array.Count - start);
        public static void Reverse<T>(this IList<T> array, int start, int count) {
            Debug.Assert(0 <= start && start < array.Count, "Start index is out of range");
            Debug.Assert(count <= array.Count - start, "Reverse count is out of range");
            T[] reverse = new T[count];
            for (int i = 0; i < reverse.Length; ++i) {
                reverse[i] = array[start + i];
            }
            int end = start + count - 1;
            for (int i = 0; i < reverse.Length; ++i) {
                array[end - i] = reverse[i];
            }
        }

        /// <summary>
        /// Determines the next permutation of the given sequence of ints, where the first permutation is ascending
        /// order (ex. 1, 2, 3) and the final permutation is descending order (ex. 3, 2, 1).
        /// </summary>
        /// <returns>False if the order given is already the final permutation, i.e. nothing was changed.</returns>
        public static bool NextPermutation(int[] order) {
            // Find greatest index x, where order[x] < order[x+1]
            int x = -1;
            for (int i = order.Length - 2; i>= 0; --i) {
                if (order[i] < order[i+1]) {
                    x = i;
                    break;
                }
            }

            // Final permutation reached
            if (x == -1) {
                return false;
            }

            // Find greatest index y, where order[x] < order[y]
            int y = -1;
            for (int i = order.Length - 1; i >= x + 1; --i) {
                if (order[x] < order[i]) {
                    y = i;
                    break;
                }
            }
            
            // Swap order[x] and order[y]
            Swap(ref order[x], ref order[y]);

            // Reverse elements from order[x+1]..order[n-1]
            order.Reverse(x + 1);

            return true;
        }

        /// <summary>
        /// Determines the next combination of items in a set, where the first combination is the first X items, and
        /// the final combination is the last X items. For example, if you have a set of 4 items and want combinations
        /// of 2, the sequence of combinations is [0, 1], [0, 2], [0, 3], [1, 2], [1, 3], [2, 3].
        /// </summary>
        /// <returns>False if the combo given is already the final combination, i.e. nothing was changed.</returns>
        public static bool NextCombination(int[] combo, int setLength) {
            int n = combo.Length;
            int i = n - 1;
            for (int j = setLength - 1; i >= 0; --i, --j) {
                if (combo[i] < j) {
                    break;
                }
            }

            if (i < 0) {
                return false;
            }

            for (int j = combo[i] + 1; i < n; ++i, ++j) {
                combo[i] = j;
            }

            return true;
        }

        /// <summary>
        /// Determines and returns all possible combinations of comboCount items in the given set.
        /// </summary>
        public static IEnumerable<T[]> GetAllCombinations<T>(IList<T> set, int comboCount) {
            Debug.Assert(comboCount <= set.Count, "Cannot create combinations with more items than the set contains");

            int[] combo = new int[comboCount];
            for (int i = 0; i < combo.Length; ++i) {
                combo[i] = i;
            }

            do {
                T[] items = new T[comboCount];
                for (int i = 0; i < combo.Length; ++i) {
                    items[i] = set[combo[i]];
                }
                yield return items;
            } while (NextCombination(combo, set.Count));
        }

        /// <summary>
        /// Determines and returns all possible combinations of items in the given set containing a number of items
        /// within the given range. For example, if the range [1, 3] is given, all combinations of 1, 2, or 3 items
        /// will be returned.
        /// </summary>
        public static IEnumerable<T[]> GetAllCombinations<T>(IList<T> set, Range comboCountRange) {
            for (int i = comboCountRange.min; i <= comboCountRange.max; ++i) {
                foreach (T[] items in GetAllCombinations(set, i)) {
                    yield return items;
                }
            }
        }
    }
}
