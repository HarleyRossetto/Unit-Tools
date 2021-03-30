using System.Collections.Generic;

namespace Macquarie.Handbook.Data.Helpers
{
    public static class PrerequisiteParser
    {
        private static Dictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>();

        static PrerequisiteParser() {
            Keywords.Add("admission",   TokenType.Admission);
            Keywords.Add("and",         TokenType.And);
            Keywords.Add("or",          TokenType.Or);
            Keywords.Add("including",   TokenType.Including);
        }

        //(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prerequisite">String representation of a pre-requisite enrolment rule.</param>
        public static void ParsePrerequisiteString(string prerequisite) {
            var split = prerequisite.Split(' ');

            foreach (var element in split) {

                var parentheseSplit = new List<string>(element.Split(new char[] {'(', ')'}));
                //If greater than 1 then we contain parenthese
                bool elementContainsParenthese = parentheseSplit.Count > 1;
                if (elementContainsParenthese) {
                    int openIndex = element.IndexOf('(');
                    int closeIndex = element.LastIndexOf(')');

                    System.Console.WriteLine(openIndex + " " + closeIndex);
                }
                var cleanedElement = element.Replace("(", string.Empty).Replace(")", string.Empty);

                TokenType token;
                bool isToken = IsToken(element, out token);

                if (isToken) {
                    //System.Console.WriteLine(token.ToString());
                }
            }
        }

        private static bool PrecedingValueIsLetter() {
            return false;
        }

        private static bool ContainsParenthese(string value) {
            return value.Contains('(') || value.Contains(')');
        }

        private static bool IsToken(string value, out TokenType token) {
            if (Keywords.ContainsKey(value.ToLower())) {
                token = Keywords[value.ToLower()];
                return true;
            }
            token = TokenType.None;
            return false;
        }
    }

    internal enum TokenType
    {
        None,
        Statement,
        OpenBrace,
        CloseBrace,
        And,
        Or,
        Admission,
        Including
    }

    public class PrerequisiteStatement {
        string Statement { get; set; }
        public List<PrerequisiteStatement> Statements { get; set; }
    }
}