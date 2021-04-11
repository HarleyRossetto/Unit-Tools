using System;
using System.Collections.Generic;
using System.Linq;

namespace Macquarie.Handbook.Helpers.Prerequisites
{
    public static class ParentheseMatcher
    {

        //Example input
        //"(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))"

        public static IEnumerable<(int Index, ParentheseType BraceType, int Depth)> GetParenthesePairings(string prerequisite) {
            //Int index, bool (true = open, false = close)
            int depth = 0;

            for (int strIdx = 0; strIdx < prerequisite.Length; strIdx++) {
                char currentCharacter = prerequisite.ElementAt(strIdx);

                if (currentCharacter == '(') {
                    yield return (strIdx, ParentheseType.Open, depth);
                    depth++;
                } else if (currentCharacter == ')') {
                    depth--;
                    yield return (strIdx, ParentheseType.Close, depth);
                }
            }
        }

        public static bool IsPreviousCharacterALetterOrDigit(string input, int currentIndex) {
            if (string.IsNullOrEmpty(input))
                return false;
            if (currentIndex - 1 < 0)
                return false;
            if (currentIndex - 1 > input.Length)
                return false;

            char previousCharacter = input.ElementAt(currentIndex - 1);
            return char.IsLetterOrDigit(previousCharacter);
        }
    }

    public enum ParentheseType
        {
            Open,
            Close
        }
}