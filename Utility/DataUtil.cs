using System.Collections.Generic;

namespace AdventOfCode {
    public static class DataUtil {
        public static bool NextPermutation(int[] data) {
            // Find greatest index x, where p[x] < p[x+1]
            int x = -1;
            for (int i = data.Length - 2; i>= 0; --i) {
                if (data[i] < data[i+1]) {
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
            for (int i = data.Length - 1; i >= x + 1; --i) {
                if (data[x] < data[i]) {
                    y = i;
                    break;
                }
            }
            
            // Swap p[x] and p[y]
            int hold = data[y];
            data[y] = data[x];
            data[x] = hold;

            // Reverse elements from p[x+1]..p[n]
            int[] reverse = new int[data.Length - 1 - x];
            for (int i = 0; i < reverse.Length; ++i) {
                reverse[i] = data[x + 1 + i];
            }
            for (int i = 0; i < reverse.Length; ++i) {
                data[data.Length - 1 - i] = reverse[i];
            }

            return true;
        }

        public static IEnumerable<(int x, int y, T)> GetElements<T>(this T[,] array) {
            for (int y = 0; y < array.GetLength(1); ++y) {
                for (int x = 0; x < array.GetLength(0); ++x) {
                    yield return (x, y, array[x, y]);
                }
            }
        }
    }
}
