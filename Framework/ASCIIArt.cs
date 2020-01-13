using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode {
    public static class ASCIIArt {
        private const int ArtCharWidth = 5;
        private const int ArtCharHeight = 6;

        private static readonly IList<uint> LetterMasks;

        static ASCIIArt() {
            int dataWidth = ArtCharWidth * 26;
            int dataHeight = ArtCharHeight;

            string[] rawData = File.ReadAllLines("ASCIIArtLetters.txt");
            bool[,] data = new bool[dataWidth, dataHeight];
            for (int y = 0; y < dataHeight; y++) {
                for (int x = 0; x < dataWidth; x++) {
                    data[x, y] = (rawData[y][x] != ' ');
                }
            }

            LetterMasks = Enumerable.Range(0, 26).Select(i => GetLetterMask(data, i)).ToList();
        }

        public static void Draw(bool[,] data, bool doubleWidth = true) {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    if (data[x, y]) Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(doubleWidth ? "  " : " ");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        public static string ImageToText(bool[,] data) {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            Debug.Assert(height == ArtCharHeight, $"Image data must be {ArtCharHeight} rows");

            string text = string.Empty;
            
            int letterCount = ((width - 1) / ArtCharWidth) + 1; // Allow the final letter to use less than the full width
            for (int i = 0; i < letterCount; i++) {
                uint mask = GetLetterMask(data, i);
                Debug.Assert(mask != 0, "Blank space is not supported");

                int letterIndex = LetterMasks.IndexOf(mask);
                Debug.Assert(letterIndex != -1, $"Unrecognized ASCII art letter (index {i})");

                text += (char)('A' + letterIndex);
            }

            return text;
        }

        private static uint GetLetterMask(bool[,] data, int letterIndex) {
            uint mask = 0;
            int width = data.GetLength(0);

            int xStart = ArtCharWidth * letterIndex;
            for (int y = 0; y < ArtCharHeight; y++) {
                for (int x = xStart; x < xStart + ArtCharWidth; x++) {
                    mask <<= 1;
                    if (x < width && data[x, y]) mask++;
                }
            }

            return mask;
        }
    }
}
