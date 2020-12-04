using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020.Day04
{
    public class Passport
    {
        private enum Field
        {
            BirthYear,
            IssueYear,
            ExpirationYear,
            Height,
            HairColor,
            EyeColor,
            PassportID,
            CountryID
        }

        private static readonly IReadOnlyList<string> ValidEyeColors = Array.AsReadOnly(new []
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        });

        public static Passport Parse(string data)
        {
            return new Passport(
                data.Split(' ', '\n')
                .Select(pair => pair.Split(':'))
                .ToDictionary(x => CodeToField(x[0]), x => x[1])
            );
        }

        private static Field CodeToField(string fieldCode)
        {
            switch (fieldCode)
            {
                case "byr": return Field.BirthYear;
                case "iyr": return Field.IssueYear;
                case "eyr": return Field.ExpirationYear;
                case "hgt": return Field.Height;
                case "hcl": return Field.HairColor;
                case "ecl": return Field.EyeColor;
                case "pid": return Field.PassportID;
                case "cid": return Field.CountryID;
                default:
                    throw new Exception($"Unrecognized field code: {fieldCode}");
            }
        }

        private static bool ValidateBirthYear(string data) => ValidateRange(data, 1920, 2002);

        private static bool ValidateIssueYear(string data) => ValidateRange(data, 2010, 2020);

        private static bool ValidateExpirationYear(string data) => ValidateRange(data, 2020, 2030);

        private static bool ValidateHeight(string data)
        {
            if (data.EndsWith("cm"))
            {
                return ValidateRange(data.Substring(0, data.Length - 2), 150, 193);
            }

            return data.EndsWith("in") && ValidateRange(data.Substring(0, data.Length - 2), 59, 76);
        }

        private static bool ValidateHairColor(string data) => Regex.IsMatch(data, "^#[0-9a-f]{6}$");

        private static bool ValidateEyeColor(string data) => ValidEyeColors.Contains(data);

        private static bool ValidatePassportID(string data) => data.Length == 9 && int.TryParse(data, out int _);

        private static bool ValidateRange(string number, int min, int max)
        {
            return int.TryParse(number, out int value) && min <= value && value <= max;
        }

        private readonly Dictionary<Field, string> _fieldMap;

        private Passport(Dictionary<Field, string> fieldMap) => _fieldMap = fieldMap;

        public bool ValidateFields()
        {
            return EnumUtil.GetValues<Field>()
                .Where(field => field != Field.CountryID)
                .All(field => _fieldMap.ContainsKey(field));
        }

        public bool ValidateData()
        {
            bool isValid = true;

            foreach ((Field field, string data) in _fieldMap)
            {
                isValid &= field switch
                {
                    Field.BirthYear      => ValidateBirthYear(data),
                    Field.IssueYear      => ValidateIssueYear(data),
                    Field.ExpirationYear => ValidateExpirationYear(data),
                    Field.Height         => ValidateHeight(data),
                    Field.HairColor      => ValidateHairColor(data),
                    Field.EyeColor       => ValidateEyeColor(data),
                    Field.PassportID     => ValidatePassportID(data),
                    Field.CountryID      => true,
                    _ => throw new Exception($"Unrecognized field type: {field}")
                };
            }

            return isValid;
        }
    }
}
