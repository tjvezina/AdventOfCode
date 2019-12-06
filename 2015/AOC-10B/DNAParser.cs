using System.Collections.Generic;

public class DNA {
    public struct Element {
        public string value;
        public string name;
    }

    private List<Element> _elements;
    private MatrixInt _dna;

    public DNA(string[] dna) {
        _dna = new MatrixInt(dna.Length);

        foreach (string elementData in dna) {
            
        }
    }
}
