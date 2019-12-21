using System;
using System.Diagnostics;

namespace AdventOfCode {
    public struct MatrixInt {
        public static MatrixInt Identity(int size) {
            MatrixInt m = new MatrixInt(size, size);

            for (int r = 0; r < size; ++r) {
                for (int c = 0; c < size; ++c) {
                    m[r, c] = (r == c ? 1 : 0);
                }
            }

            return m;
        }

        public static implicit operator MatrixInt(int[] vector) {
            MatrixInt m = new MatrixInt(vector.Length, 1);

            for (int i = 0; i < vector.Length; ++i) {
                m[i, 0] = vector[i];
            }

            return m;
        }

        public static implicit operator int[](MatrixInt matrix) {
            Debug.Assert(matrix.cols == 1, "Cannot convert a matrix with more than one column into a vector (int[])");

            int[] v = new int[matrix.rows];

            for (int i = 0; i < matrix.rows; ++i) {
                v[i] = matrix[i, 0];
            }

            return v;
        }

        public static MatrixInt operator*(MatrixInt a, MatrixInt b) {
            Debug.Assert(a.cols == b.rows, "Invalid arguments, unable to multiply mismatched matrix sizes.");

            MatrixInt c = new MatrixInt(a.rows, b.cols);

            for (int row = 0; row < c.rows; ++row) {
                for (int col = 0; col < c.cols; ++col) {
                    for (int i = 0; i < a.cols; ++i) {
                        c[row, col] += a[row, i] * b[i, col];
                    }
                }
            }

            return c;
        }

        public static MatrixInt Power(MatrixInt m, int e) {
            Debug.Assert(m.rows == m.cols, "Cannot raise non-square matrix to a power");

            MatrixInt result = Identity(m.rows);
            while (e > 0) {
                if ((e & 1) != 0) {
                    result *= m;
                    e -= 1;
                }
                m *= m;
                e /= 2;
            }
            return result;
        }

        private int[,] _matrix;

        public int rows => _matrix.GetLength(0);
        public int cols => _matrix.GetLength(1);

        public int this[int r, int c] {
            get => _matrix[r, c];
            set => _matrix[r, c] = value;
        }

        public MatrixInt(int size) : this(size, size) { }
        public MatrixInt(int rows, int cols) {
            _matrix = new int[rows, cols];
        }

        public MatrixInt(int[,] data) {
            _matrix = new int[data.GetLength(0), data.GetLength(1)];

            for (int r = 0; r < rows; ++r) {
                for (int c = 0; c < cols; ++c) {
                    _matrix[r, c] = data[r, c];
                }
            }
        }

        public override string ToString() {
            int digits = 1;
            for (int r = 0; r < rows; ++r) {
                for (int c = 0; c < cols; ++c) {
                    digits = Math.Max(digits, (int)Math.Log10(_matrix[r, c]) + 1);
                }
            }
            string format = $"{{0,{digits}}}";

            string output = string.Empty;

            for (int r = 0; r < rows; ++r) {
                output += (rows == 0 ? "[" : (r == 0 ? "┌" : (r == rows - 1 ? "└" : "|")));
                for (int c = 0; c < cols; ++c) {
                    output += string.Format(format, _matrix[r, c]) + (c == cols - 1 ? "" : ",");
                }
                output += (rows == 0 ? "]" : (r == 0 ? "┐" : (r == rows - 1 ? "┘" : "|"))) + "\n";
            }

            return output;
        }
    }
}
