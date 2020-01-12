using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016.Day04 {
    public class Room {
        public int sectorID { get; }
        public bool isValid { get; }
        public string name {
            get {
                Debug.Assert(_name != null, $"Call {nameof(Decrypt)} before accessing the decrypted room name");
                return _name;
            }
        }
        
        private string _encryptedName;
        private string _name;

        public Room(string encryptedData) {
            Match match = Regex.Match(encryptedData, @"(.*)-(\d+)\[([a-z]{5})\]");
            Debug.Assert(match.Success, "Encrypted room data did not match expected format");

            _encryptedName = match.Groups[1].Value;
            sectorID = int.Parse(match.Groups[2].Value);
            string checksum = match.Groups[3].Value;

            // Grab the set of all letters that appear in the name
            IEnumerable<char> letters = _encryptedName.Where(char.IsLower).Distinct();

            // Sort by number of occurances, then alphabetically
            letters = letters.Select(c => (letter: c, count: _encryptedName.Count(c2 => c == c2)))
                .OrderByDescending(d => d.count).ThenBy(d => d.letter).Select(d => d.letter);

            isValid = (letters.Count() >= 5 && new string(letters.Take(5).ToArray()) == checksum);
        }

        public void Decrypt() {
            char Shift(char c) => (c == '-' ? ' ' : (char)((c + sectorID - 'a') % 26 + 'a'));

            _name = new string(_encryptedName.Select(Shift).ToArray());
        }
    }
}
