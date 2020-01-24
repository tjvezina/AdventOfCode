using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode {
    public static class DataUtil {
        public static void Swap<T>(ref T a, ref T b) {
            T hold = a;
            a = b;
            b = hold;
        }

        public static void Swap<T>(this T[] array, int iA, int iB) {
            T hold = array[iA];
            array[iA] = array[iB];
            array[iB] = hold;
        }

        public static void Reverse<T>(this T[] array) => Reverse(array, 0, array.Length);
        public static void Reverse<T>(this T[] array, int start) => Reverse(array, start, array.Length - start);
        public static void Reverse<T>(this T[] array, int start, int count) {
            int end = start + count - 1;
            for (int i = 0; i < count / 2; i++) {
                array.Swap(start + i, end - i);
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
            for (int i = order.Length - 2; i >= 0; --i) {
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
            order.Swap(x, y);

            // Reverse elements from order[x+1]..order[n-1]
            order.Reverse(x + 1);

            return true;
        }

        /// <summary>
        /// Determines the next combination of items in a list, where the first combination is the first X items, and
        /// the final combination is the last X items. For example, if you have a list of 4 items and want combinations
        /// of 2, the sequence of combinations is [0, 1], [0, 2], [0, 3], [1, 2], [1, 3], [2, 3].
        /// </summary>
        /// <returns>False if the combo given is already the final combination, i.e. nothing was changed.</returns>
        public static bool NextCombination(int[] combo, int listLength) {
            int n = combo.Length;
            int i = n - 1;
            for (int j = listLength - 1; i >= 0; --i, --j) {
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
        /// Determines and returns all possible combinations of comboCount items in the given list.
        /// </summary>
        public static IEnumerable<T[]> GetAllCombinations<T>(IList<T> list, int comboCount) {
            if (comboCount > list.Count) yield break;

            int[] combo = new int[comboCount];
            for (int i = 0; i < combo.Length; ++i) {
                combo[i] = i;
            }

            do {
                T[] items = new T[comboCount];
                for (int i = 0; i < combo.Length; ++i) {
                    items[i] = list[combo[i]];
                }
                yield return items;
            } while (NextCombination(combo, list.Count));
        }

        /// <summary>
        /// Determines and returns all possible combinations of items in the given list containing a number of items
        /// within the given range. For example, if the range [1, 3] is given, all combinations of 1, 2, or 3 items
        /// will be returned.
        /// </summary>
        public static IEnumerable<T[]> GetAllCombinations<T>(IList<T> list, Range comboCountRange) {
            foreach (int comboCount in comboCountRange) {
                foreach (T[] items in GetAllCombinations(list, comboCount)) {
                    yield return items;
                }
            }
        }
    }
}
