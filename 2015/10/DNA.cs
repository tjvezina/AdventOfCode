using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015.Day10
{
    public class DNA
    {
        private struct Element
        {
            public int index;
            public string value;
        }

        public int elementCount => _elements.Count;

        private readonly List<Element> _elements = new List<Element>();
        private readonly MatrixInt _dna;

        public DNA(IReadOnlyList<string> dna)
        {
            _dna = new MatrixInt(dna.Count);
        
            Dictionary<string, Element> elementMap = new Dictionary<string, Element>();

            string[][] ruleSet = new string[dna.Count][];
            for (int i = 0; i < dna.Count; i++)
            {
                string[] parts = dna[i].Split('=');
                string elementName = parts[0];
                string[] subElements = parts[1].Split('.');

                // One of the sub elements will be the value of this element; find its index
                int valueIndex = subElements.ToList().FindIndex(r => Regex.IsMatch(r, "\\d"));
                // Store the element's name and value
                Element element = new Element { index = i, value = subElements[valueIndex] };
                _elements.Add(element);
                elementMap[elementName] = element;
                // Replace the element's value with it's name (the rules will only contain names now)
                subElements[valueIndex] = elementName;

                // Rules apply to the next element
                if (i < dna.Count - 1)
                {
                    ruleSet[i + 1] = subElements;
                }

                // The first rule is simply the first element (it simply breaks down into itself)
                if (i == 0)
                {
                    ruleSet[0] = new [] { elementName };
                }
            }

            for (int i = 0; i < ruleSet.Length; i++)
            {
                foreach (int subIndex in ruleSet[i].Select(e => elementMap[e].index))
                {
                    _dna[subIndex, i]++;
                }
            }
        }
        
        public ulong GetIterationLength(int[] initial, int iterations)
        {
            MatrixInt iterDNA = MatrixInt.Power(_dna, iterations);

            int[] elementCounts = iterDNA * initial;

            ulong length = 0;

            for (int i = 0; i < elementCounts.Length; i++)
            {
                int count = elementCounts[i];

                if (count > 0)
                {
                    length += (ulong)_elements[i].value.Length * (ulong)count;
                }
            }

            return length;
        }
    }
}
