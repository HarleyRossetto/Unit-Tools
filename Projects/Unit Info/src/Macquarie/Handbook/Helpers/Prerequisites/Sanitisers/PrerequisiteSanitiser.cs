using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Macquarie.Handbook.Helpers.Prerequisites.Sanitisers
{
    public static class PrerequisiteSanitiser
    {
        private delegate string Sanitiser(string input);
        private static readonly Sanitiser[] prerequisiteSanitisers = {
            RemoveWhitespaceEscapeSequences,
            NormaliseCreditPointRepresentations,
            CreditPointRequirmentOfToIn
        };

        public static string Sanitise(string prerequisite) {

            string result = prerequisite;
            foreach (var sanitiser in prerequisiteSanitisers) {
                result = sanitiser(result);
            }
            return result;
        }

        public static string NormaliseCreditPointRepresentations(string prerequisite) {
            return prerequisite.Replace("cps", "cp")
                                .Replace("credit points", "cp");
        }
        public static string CreditPointRequirmentOfToIn(string prerequisite) {
            return prerequisite.Replace("cp of", "cp in"); //English might not be great here. But only 8 occourances but helps to remove.
        }

        public static string RemoveWhitespaceEscapeSequences(string prerequisite) {
            return prerequisite.Replace("\t", " ")
                               .Replace("\n", "")
                               .Replace("\r", "")
                               .Trim(); //Remove top and tail whitespace.
        }
    }
}