using System;
using System.Collections.Generic;
using System.Linq;

namespace Macquarie.Handbook.Helpers.Prerequisites
{
    public static class PrerequisiteParser
    {
        public static void Parse(string prerequisite) {
            var sanitisedPrerequisiteString = ParenthesesSanitiser.Sanitise(prerequisite);
            var ranges = ParentheseMatcher.Match(sanitisedPrerequisiteString);

            var prereqs = new List<PrerequisiteElement>(ExtractPrerequisiteElements(prerequisite, ranges));

            var elementWithParents = FindParentRanges(prereqs);

            var prereqDictionary = CreatePrerequisiteDictionary(prereqs);

            var topLevelElement = ProcessPrerequisiteGuids(prerequisite, prereqDictionary);

            System.Console.WriteLine("test");
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
                    element.ParentGUID = parent.GUID;


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

        private static IEnumerable<PrerequisiteElement> ExtractPrerequisiteElements(string prerequisite, IEnumerable<(Range Range, int Depth)> ranges) {
            yield return new PrerequisiteElement(prerequisite, new Range(0, prerequisite.Length), 0);
            foreach (var range in ranges) {
                yield return new PrerequisiteElement(prerequisite.Substring(range.Range.Start.Value, range.Range.GetLength()), range.Range, range.Depth);
            }
        }

        private static Dictionary<string, PrerequisiteElement> CreatePrerequisiteDictionary(IEnumerable<PrerequisiteElement> prerequisites) {
            var dictionary = new Dictionary<string, PrerequisiteElement>();
            foreach (var pr in prerequisites) {
                dictionary.Add(pr.GUID, pr);
            }
            return dictionary;
        }

        private static PrerequisiteElement ProcessPrerequisiteGuids(string prerequisite, Dictionary<string, PrerequisiteElement> elements) {
            foreach (var kv in elements.Reverse()) {
                if (elements.TryGetValue(kv.Value.ParentGUID ??= "0", out PrerequisiteElement parent)) {
                    //Remove original value\
                    var length = kv.Value.RangeInParentString.GetLength() - 1;
                    var temp = parent.Prerequisite.Remove(kv.Value.RangeInParentString.Start.Value, length);
                    //Insert new value (GUID)
                    temp = temp.Insert(kv.Value.RangeInParentString.Start.Value, kv.Key);
                    parent.Prerequisite = temp;
                }
            }
            return elements.Single(x => x.Value.Depth == 0).Value;
        }
    }

    public class Expression {
        /*
            AND
            OR
            ADMISSION TO
            #CP
            #CP INCLUDING
            #CP IN &&&& UNITS
            #CP AT #### LEVEL OR ABOVE
            Permission by special approval



        */
    }

    public class StatementExpression : Expression { 
        //Unitcode | Course | Degree...
    }

    public class AndExpression : Expression {
        //Expression AND Expression
        //Unit AND Unit
     }

    public class OrExpression : Expression { 
        //Expression OR Expression
        //Unit OR Unit
    }

    public class AdmissionToExpression : Expression {
        //ADMISSION TO Expression | Statement
     }

    public class SpecialApprovalExpression : Expression {
        //Permission by special approval
     }

    public class IncludingExpression : Expression {
        //Expression INCLUDING Expression
     }
}