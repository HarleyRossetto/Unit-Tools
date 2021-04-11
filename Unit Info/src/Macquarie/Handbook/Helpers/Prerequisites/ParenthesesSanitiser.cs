using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Macquarie.Handbook.Helpers.Prerequisites
{
    public static class ParenthesesSanitiser
    {
        //Matches (P|C|D|HD) elements
        static Regex _regexGradeRequirement = new Regex(@"\([CPGD]\)");
        //Matches (OUA|ECE|ECS)
        static Regex _regexOuaRequirement = new Regex(@"\(.{3}\)");
        //Matches (waiver)
        static Regex _regexWaiverRequirement = new Regex(@"\(waiver\)");
        //Matches (Beijing)
        static Regex _regexBeijingRequirement = new Regex(@"\(Beijing\)");
        //Matches (Prim|Hons)
        static Regex _regexPrimHonsRequirement = new Regex(@"\(\w{4}\)");
        //Matches (Singe or double degrees)
        static Regex _regexSglOrDblDegreeRequirement = new Regex(@"\(Single or double degrees\)");
        //Matches (0 to 5)
        static Regex _regexZeroToFiveNumericRequirement = new Regex(@"\(0 to 5\)");
        //Matches (Birth to five)
        static Regex _regexZeroToFiveCharRequirement = new Regex(@"\(Birth to five\)");

        static List<Regex> _regexFilters = new()
        {
            _regexGradeRequirement,
            _regexOuaRequirement,
            _regexWaiverRequirement,
            _regexBeijingRequirement,
            _regexPrimHonsRequirement,
            _regexSglOrDblDegreeRequirement,
            _regexZeroToFiveCharRequirement
        };

        public static string Sanitise(string prerequsite) {
            var standardisedBrackets = ReplaceSquareBrackets(prerequsite);
            var qualifiersCorrected = ReplaceBracketedQualifiers(standardisedBrackets);
            return qualifiersCorrected;
        }

        public static string ReplaceBracketedQualifiers(string input) {
            string result = input;
            foreach (var filter in _regexFilters) {
                result = QualifyFilterBrackets(result, filter.Matches(result));
            }
            return result;
        }

        private static string QualifyFilterBrackets(string prerequsites, MatchCollection matches) {
            foreach (Match match in matches) {
                var replaced = ReplaceParenthesesWithSquareBrackets(match.Value);
                var inputWithMatchRemoved = prerequsites.Remove(match.Index, match.Length);
                prerequsites = inputWithMatchRemoved.Insert(match.Index, replaced);
            }

            return prerequsites;
        }

        public static string ReplaceSquareBrackets(string input) {
            var result = input.Replace('[', '(');
            return result.Replace(']', ')');
        }

        private static string ReplaceParenthesesWithSquareBrackets(string input) {
            var temp = input.Replace('(', '[');
            return temp.Replace(')', ']');
        }
    }
}