using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public class BinaryMinHeap<T> {
        private IComparer<T> _comparer;
        private List<T> data = new List<T>();

        public BinaryMinHeap(IComparer<T> comparer = null) {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public int Count => data.Count;

        public T Peek() => data[0];

        public void Insert(T item) {
            data.Add(item);

            int iChild = data.Count - 1;
            while (iChild > 0) {
                int iParent = (iChild - 1) / 2;

                if (_comparer.Compare(data[iChild], data[iParent]) >= 0) break;

                Swap(iParent, iChild);
                iChild = iParent;
            }
        }

        public bool TryExtract(out T result) {
            if (data.Count == 0) {
                result = default;
                return false;
            }

            result = Extract();
            return true;
        }

        public T Extract() {
            int iLast = data.Count - 1;
            T front = data[0];
            data[0] = data[iLast];
            data.RemoveAt(iLast);

            iLast--;
            int iParent = 0;
            while (true) {
                // Left child
                int iChild = iParent * 2 + 1;
                if (iChild > iLast) break;

                if (_comparer.Compare(data[iParent], data[iChild]) > 0) {
                    Swap(iParent, iChild);
                    iParent = iChild;
                    continue;
                }

                // Right child
                iChild++;
                if (iChild > iLast) break;

                if (_comparer.Compare(data[iParent], data[iChild]) > 0) {
                    Swap(iParent, iChild);
                    iParent = iChild;
                    continue;
                }

                break;
            }

            return front;
        }

        private void Swap(int a, int b) {
            T temp = data[a];
            data[a] = data[b];
            data[b] = temp;
        }
    }
}
