using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Program {
    private static void Main(string[] args) {
        SpaceImage image = new SpaceImage(25, 6, File.ReadAllLines("input.txt")[0]);
        image.Draw();
    }
}
