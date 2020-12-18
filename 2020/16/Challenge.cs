using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2020.Day16
{
    public class Challenge : BaseChallenge
    {
        private Field[] _fields;
        private int[] _myTicket;
        private int[][] _nearbyTickets;
        private readonly List<int[]> _validTickets = new List<int[]>();

        public override object part1ExpectedAnswer => 27870;
        public override (string message, object answer) SolvePart1()
        {
            string[][] parts = inputFile.Split("\n\n").Select(x => x.Split('\n')).ToArray();

            _fields = parts[0].Select(Field.Parse).ToArray();

            _myTicket = parts[1].Last().Split(',').Select(int.Parse).ToArray();

            _nearbyTickets = parts[2].Skip(1).Select(x => x.Split(',').Select(int.Parse).ToArray()).ToArray();

            int ticketScanningErrorRate = 0;

            foreach (int[] ticket in _nearbyTickets)
            {
                int[] invalid = ticket.Where(v => _fields.All(f => !f.IsValid(v))).ToArray();

                if (invalid.Any())
                {
                    ticketScanningErrorRate += invalid.Sum();
                }
                else
                {
                    _validTickets.Add(ticket);
                }
            }

            return ("Ticket scanning error rate = ", ticketScanningErrorRate);
        }
        
        public override object part2ExpectedAnswer => 3173135507987;
        public override (string message, object answer) SolvePart2()
        {
            List<Field>[] possibilityMatrix = Enumerable.Range(0, _fields.Length)
                .Select(i => new List<Field>(_fields)).ToArray();

            for (int i = 0; i < possibilityMatrix.Length; i++)
            {
                List<Field> possibleFields = possibilityMatrix[i];

                for (int t = 0; t < _validTickets.Count && possibleFields.Count > 1; t++)
                {
                    int[] ticket = _validTickets[t];

                    for (int f = possibleFields.Count - 1; f >= 0; f--)
                    {
                        if (!possibleFields[f].IsValid(ticket[i]))
                        {
                            possibleFields.RemoveAt(f);
                        }
                    }
                }

                if (possibleFields.Count == 1)
                {
                    for (int j = 0; j < possibilityMatrix.Length; j++)
                    {
                        if (i == j) continue;

                        possibilityMatrix[j].Remove(possibleFields[0]);
                    }
                }
            }

            List<List<Field>> toResolve = possibilityMatrix.Where(x => x.Count > 1).ToList();

            while (toResolve.Any())
            {
                for (int i = toResolve.Count - 1; i >= 0; i--)
                {
                    IEnumerable<Field> allPossibilities = toResolve.SelectMany(x => x);
                    Field unique = toResolve[i].FirstOrDefault(f => allPossibilities.Count(x => x == f) == 1);

                    if (unique != null)
                    {
                        for (int j = 0; j < toResolve.Count; j++)
                        {
                            if (i == j) continue;

                            toResolve[j].Remove(unique);
                        }

                        toResolve[i].Clear();
                        toResolve[i].Add(unique);
                        toResolve.RemoveAt(i);
                    }
                }
            }

            Field[] fieldOrder = possibilityMatrix.Select(x => x.Single()).ToArray();

            long destinationProduct = 1;

            for (int i = 0; i < fieldOrder.Length; i++)
            {
                if (fieldOrder[i].name.StartsWith("departure"))
                {
                    destinationProduct *= _myTicket[i];
                }
            }

            return ("Product of destination values on my ticket: ", destinationProduct);
        }
    }
}
