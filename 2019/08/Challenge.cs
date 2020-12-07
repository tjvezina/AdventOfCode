namespace AdventOfCode.Year2019.Day08
{
     public class Challenge : BaseChallenge
     {
        private readonly SpaceImage image;

        public Challenge() => image = new SpaceImage(25, 6, inputList[0]);

        public override object part1ExpectedAnswer => 1360;
        public override (string message, object answer) SolvePart1()
        {
            int leastZerosLayer = -1;
            int leastZeros = int.MaxValue;
            int[] leastZerosValueCounts = new int[3];

            for (int z = 0; z < image.depth; z++)
            {
                int[] valueCounts = new int[3];
                for (int y = 0; y < image.height; y++)
                {
                    for (int x = 0; x < image.width; x++)
                    {
                        valueCounts[image[x, y, z]]++;
                    }
                }

                if (valueCounts[0] < leastZeros)
                {
                    leastZeros = valueCounts[0];
                    leastZerosLayer = z;
                    valueCounts.CopyTo(leastZerosValueCounts, 0);
                }
            }

            return ("Image validation output: ", leastZerosValueCounts[1] * leastZerosValueCounts[2]);
        }
        
        public override object part2ExpectedAnswer => "FPUAR";
        public override (string message, object answer) SolvePart2()
        {
            bool[,] data = image.Flatten();

           ASCIIArt.Draw(data);
           return ("Image text: ", ASCIIArt.ImageToText(data));
        }
    }
}
