using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day07
{
    public class Challenge : BaseChallenge
    {
        private class Bag
        {
            public readonly List<Bag> parents = new List<Bag>();
            public readonly List<(int count, Bag bag)> children = new List<(int, Bag)>();
        }

        private const string MyBagColor = "shiny gold";

        private readonly Dictionary<string, Bag> _bagMap = new Dictionary<string, Bag>();

        public override object part1ExpectedAnswer => 211;
        public override (string message, object answer) SolvePart1()
        {
            BuildBagMap();

            Bag myBag = _bagMap[MyBagColor];

            HashSet<Bag> parents = new HashSet<Bag>();
            Queue<Bag> parentsToCheck = new Queue<Bag>();
            parentsToCheck.Enqueue(myBag);

            while (parentsToCheck.TryDequeue(out Bag bag))
            {
                foreach (Bag parent in bag.parents.Where(parent => parents.Add(parent)))
                {
                    parentsToCheck.Enqueue(parent);
                }
            }

            return ($"There are {{0}} bag colors that could contain my {MyBagColor} bag", parents.Count);
        }
        
        public override object part2ExpectedAnswer => 12414;
        public override (string message, object answer) SolvePart2()
        {
            Bag myBag = _bagMap[MyBagColor];

            return ("My bag contains {0} other bags", GetChildCountRecursive(myBag));

            static long GetChildCountRecursive(Bag bag)
            {
                return bag.children.Sum(x => x.count * (1 + GetChildCountRecursive(x.bag)));
            }
        }

        private void BuildBagMap()
        {
            foreach (string line in inputList)
            {
                Match match = Regex.Match(line, @"(.+) bags contain (.+)");
                string bagName = match.Groups[1].Value;
                string contents = match.Groups[2].Value;

                Bag bag = GetOrCreate(bagName);

                foreach (Match childMatch in Regex.Matches(contents, @"(\d+) ([a-z ]+) bags?"))
                {
                    int count = int.Parse(childMatch.Groups[1].Value);
                    string childName = childMatch.Groups[2].Value;

                    Bag child = GetOrCreate(childName);

                    bag.children.Add((count, child));
                    child.parents.Add(bag);
                }
            }

            Bag GetOrCreate(string bagName)
            {
                if (_bagMap.ContainsKey(bagName))
                {
                    return _bagMap[bagName];
                }

                Bag bag = new Bag();
                _bagMap[bagName] = bag;
                return bag;
            }
        }
    }
}
