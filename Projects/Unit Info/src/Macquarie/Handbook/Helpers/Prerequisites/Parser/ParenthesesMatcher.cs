using System;
using System.Collections.Generic;
using System.Linq;

namespace Macquarie.Handbook.Helpers.Prerequisites.Parser
{
    public static class ParentheseMatcher
    {

        //Example input
        //"(Admission to BEd(Prim) and (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670)) or (130cp including (EDUC258 or EDUC2580) and (EDUC260 or EDUC2600) and (EDUC267 or EDUC2670) and (EDTE353 or EDTE3530))"

        public static IEnumerable<(Range Range, int Depth)> Match(string prerequisite) {
            //Gets a list of all the opening and closing braces in the prerequisite string.
            var braces = GetBraces(prerequisite);
            //Joins those braces together as a Range and return.
            return BuildRanges(braces);
        }

        public static IEnumerable<(int Index, ParentheseType BraceType, int Depth)> GetBraces(string prerequisite) {
            //Int index, bool (true = open, false = close)
            int depth = 0;

            for (int strIdx = 0; strIdx < prerequisite.Length; strIdx++) {
                char currentCharacter = prerequisite.ElementAt(strIdx);

                if (currentCharacter == '(') {
                    depth++;
                    yield return (strIdx, ParentheseType.Open, depth);
                } else if (currentCharacter == ')') {
                    yield return (strIdx, ParentheseType.Close, depth);
                    depth--;
                }
            }
        }

        private static IEnumerable<(Range Range, int Depth)> BuildRanges(IEnumerable<(int Index, ParentheseType BraceType, int Depth)> braces) {
            for (int i = 0; i < braces.Count(); i++) {
                var topElement = braces.ElementAt(i);
                if (topElement.BraceType == ParentheseType.Open) {
                    for (int j = i + 1; j < braces.Count(); j++) {
                        var bottomElement = braces.ElementAt(j);
                        if (bottomElement.BraceType == ParentheseType.Close && bottomElement.Depth == topElement.Depth) {
                            yield return (new Range(topElement.Index, bottomElement.Index), topElement.Depth);
                            break;
                        }
                    }
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