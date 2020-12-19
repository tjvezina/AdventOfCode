using System;
using System.Collections.Generic;

namespace AdventOfCode.Year2020.Day17
{
    public class Challenge : BaseChallenge
    {
        public override object part1ExpectedAnswer => 257;
        public override (string message, object answer) SolvePart1()
        {
            List<Point3> cubes = new List<Point3>();

            for (int y = 0; y < inputList.Count; y++)
            {
                string input = inputList[y];
                for (int x = 0; x < input.Length; x++)
                {
                    if (input[x] == '#')
                    {
                        cubes.Add(new Point3(x, y, 0));
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Cycle {i}: {cubes.Count}");
                ExecuteCycle(cubes);
            }

            return ("There are {0} cubes after 6 cycles", cubes.Count);
        }

        private void ExecuteCycle(List<Point3> cubes)
        {
            Dictionary<Point3, int> neighborMap = new Dictionary<Point3, int>();

            foreach (Point3 cube in cubes)
            {
                for (int z = -1; z <= 1; z++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            if (x == 0 && y == 0 && z == 0)
                            {
                                if (!neighborMap.ContainsKey(cube))
                                {
                                    neighborMap[cube] = 0;
                                }
                                continue;
                            }

                            Point3 neighbor = cube + new Point3(x, y, z);

                            if (!neighborMap.ContainsKey(neighbor))
                            {
                                neighborMap[neighbor] = 0;
                            }

                            neighborMap[neighbor]++;
                        }
                    }
                }
            }

            foreach ((Point3 point, int neighborCount) in neighborMap)
            {
                if (cubes.Contains(point))
                {
                    if (neighborCount < 2 || neighborCount > 3)
                    {
                        cubes.Remove(point);
                    }
                }
                else if (neighborCount == 3)
                {
                    cubes.Add(point);
                }
            }
        }

        public override object part2ExpectedAnswer => 2532;
        public override (string message, object answer) SolvePart2()
        {
            List<Point4> cubes = new List<Point4>();

            for (int y = 0; y < inputList.Count; y++)
            {
                string input = inputList[y];
                for (int x = 0; x < input.Length; x++)
                {
                    if (input[x] == '#')
                    {
                        cubes.Add(new Point4(x, y, 0, 0));
                    }
                }
            }

            for (int i = 0; i < 6; i++)
            {
                Console.WriteLine($"Cycle {i}: {cubes.Count}");
                ExecuteCycle(cubes);
            }

            Profiler.FlushResults();

            return ("There are {0} hypercubes after 6 cycles", cubes.Count);
        }

        private void ExecuteCycle(List<Point4> cubes)
        {
            Dictionary<Point4, int> neighborMap = new Dictionary<Point4, int>();

            foreach (Point4 cube in cubes)
            {
                for (int w = -1; w <= 1; w++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            for (int x = -1; x <= 1; x++)
                            {
                                if (x == 0 && y == 0 && z == 0 && w == 0)
                                {
                                    if (!neighborMap.ContainsKey(cube))
                                    {
                                        neighborMap[cube] = 0;
                                    }
                                    continue;
                                }

                                Point4 neighbor = cube + new Point4(x, y, z, w);

                                if (!neighborMap.ContainsKey(neighbor))
                                {
                                    neighborMap[neighbor] = 0;
                                }

                                neighborMap[neighbor]++;
                            }
                        }
                    }
                }
            }

            foreach ((Point4 point, int neighborCount) in neighborMap)
            {
                if (cubes.Contains(point))
                {
                    if (neighborCount < 2 || neighborCount > 3)
                    {
                        cubes.Remove(point);
                    }
                }
                else if (neighborCount == 3)
                {
                    cubes.Add(point);
                }
            }
        }
    }
}
