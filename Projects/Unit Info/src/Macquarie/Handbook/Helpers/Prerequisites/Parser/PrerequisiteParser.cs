using System;
using System.Collections.Generic;
using System.Linq;

using Macquarie.Handbook.Helpers.Prerequisites.Sanitisers;
using Macquarie.Handbook.Helpers.Extensions;

namespace Macquarie.Handbook.Helpers.Prerequisites.Parser
{
    public static class PrerequisiteParser
    {
        public static void Parse(string prerequisite) {
            var sanitisedPrerequisiteString = SanitiseString(prerequisite);

            //Breaks down the string into matching parenthese pairings
            //Assigns a ID ({0} up) to each split.
            var prereqDictionary = CreatePrerequisiteDictionary(sanitisedPrerequisiteString);

            //Give us the first element, from theere we can navigate through the children and back to the parent as needed
            //when further parsing the structure.
            var rootElement = GetRootElement(prereqDictionary);

            Console.WriteLine(rootElement.ToString());
            
        }

        private static string SanitiseString(string prerequisite) {
            var sanitisedPrerequisiteString = PrerequisiteSanitiser.Sanitise(prerequisite);
            sanitisedPrerequisiteString = ParenthesesSanitiser.Sanitise(sanitisedPrerequisiteString);
            return sanitisedPrerequisiteString;
        }

        private static IEnumerable<PrerequisiteElement> FindParentRanges(IEnumerable<PrerequisiteElement> elements) {
            var results = new List<PrerequisiteElement>();

            foreach (var element in elements) {
                if (element.Depth == 0) {
                    results.Add(element);
                    continue;
                }

                var parent = FindParentElement(element, elements);
                if (parent is not null && parent != element) {
                    element.Parent = parent;
                    //Add element at child of parent
                    parent.Children.Add(element);

                    int index = parent.Prerequisite.IndexOf(element.Prerequisite);
                    int end = index + element.Prerequisite.Length;
                    element.RangeInParentString = new Range(index, end);

                    results.Add(element);
                }
            }

            return results;
        }

        public static PrerequisiteElement FindParentElement(PrerequisiteElement current, IEnumerable<PrerequisiteElement> elements) {
            foreach (var element in elements) {
                if (current.IsInRange(element))
                    return element;
            }
            return null;
        }

        private static IEnumerable<(string Prereq, Range StringRange)> ExtractStrings(string prerequisite, IEnumerable<Range> ranges) {
            yield return (prerequisite, new Range(0, prerequisite.Length));
            foreach (var range in ranges) {
                yield return (prerequisite.Substring(range.Start.Value, range.GetLength()), range);
            }
        }

        /// <summary>
        /// Returns a list of PrerequisiteElements which represent the string component within a pair of parentheses.
        /// </summary>
        /// <param name="prerequisite">The input string from which to divide into ranges.</param>
        /// <param name="ranges">List of ranges representing matched parenthese pairs.</param>
        /// <returns>A list of Prerequisite elements.</returns>
        private static IEnumerable<PrerequisiteElement> ExtractPrerequisiteElements(string prerequisite, IEnumerable<(Range Range, int Depth)> ranges) {
            int idCounter = 0;
            yield return new PrerequisiteElement(prerequisite, new Range(0, prerequisite.Length), 0, "{" + idCounter++.ToString() + "}");
            foreach (var range in ranges) {
                yield return new PrerequisiteElement(prerequisite.Substring(range.Range.Start.Value, range.Range.GetLength()), range.Range, range.Depth, "{" + idCounter++.ToString() + "}");
            }
        }

        private static Dictionary<string, PrerequisiteElement> CreatePrerequisiteDictionary(string prerequisite) {
            var ranges = ParentheseMatcher.Match(prerequisite);

            var prereqs = new List<PrerequisiteElement>(ExtractPrerequisiteElements(prerequisite, ranges));

            var elementWithParents = FindParentRanges(prereqs);

            var dictionary = new Dictionary<string, PrerequisiteElement>();
            foreach (var pr in prereqs) {
                dictionary.Add(pr.ID, pr);
            }

            ReplaceGroupedElementsWithGuids(dictionary);

            return dictionary;
        }

        private static void ReplaceGroupedElementsWithGuids(Dictionary<string, PrerequisiteElement> elements) {
            foreach (var kv in elements.Reverse()) {
                if (kv.Value.Parent != null) {
                    if (elements.TryGetValue(kv.Value.Parent.ID, out PrerequisiteElement parent)) {
                        //Remove original value\
                        var length = kv.Value.RangeInParentString.GetLength() - 1;
                        var temp = parent.Prerequisite.Remove(kv.Value.RangeInParentString.Start.Value, length);
                        //Insert new value (ID)
                        temp = temp.Insert(kv.Value.RangeInParentString.Start.Value, kv.Key);
                        parent.Prerequisite = temp;
                    }
                }
            }
        }

        public static PrerequisiteElement GetRootElement(Dictionary<string, PrerequisiteElement> dictionary) {
            return dictionary.Single(x => x.Value.Depth == 0).Value;
        }

        private delegate string KeywordParser(string lex);
        private delegate bool CanParseKeyword(string lex);

        private static readonly List<(string Keyword, CanParseKeyword CanParse, KeywordParser Parser)> keywords = new()
        {
            ("admission",   CanParseKeywordAdmission,       KeywordAdmissionParser),
            ("or",          null,       null),
            ("and",         null,       null),
            ("permission",  null,       null),
        };

        private static string KeywordAdmissionParser(string lex) {
            var splitLex = lex.Split(" ");
            //Todo parse admission statements
            return lex;
        }

        private static bool CanParseKeywordAdmission(string lex) {
            var splitLex = lex.Split(" ");

            if (splitLex.Any()) {
                if (splitLex[0].ToLower() == "admission") {
                    var preposition = splitLex?[1].ToLower();
                    if (preposition != null 
                        && preposition == "in"
                        || preposition == "to"
                        || preposition == "into") {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    /*
           AND
           OR
           ADMISSION TO
           ADMISSION IN
           ADMISSION INTO
           #CP
           #CP INCLUDING
           #CP IN &&&& UNITS
           #CP AT #### LEVEL OR ABOVE
           Permission by special approval
           Completion of


       */

    /*
        Statement

            Noun
            Verb
            Preposition


        OR
        OR ABOVE        
        ADMISSION TO
        AND
        

    */

    public class Expression
    {

    }

    public class StatementExpression : Expression
    {
        //Unitcode | CourseRecord | Degree...
    }

    public class AndExpression : Expression
    {
        //Expression AND Expression
        //Unit AND Unit
    }

    public class OrExpression : Expression
    {
        //Expression OR Expression
        //Unit OR Unit
    }

    public class AdmissionToExpression : Expression
    {
        //ADMISSION TO Expression | Statement
    }

    public class SpecialApprovalExpression : Expression
    {
        //Permission by special approval
    }

    public class IncludingExpression : Expression
    {
        //Expression INCLUDING Expression
    }
}