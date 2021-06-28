

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Macquarie.Handbook.Data.Unit.Prerequisites;

namespace Macquarie.Handbook.Helpers
{
    public static class EnrolmentRuleParentheseParser
    {
        public static Dictionary<string, ParentheseGroup> BreakdownParentheseGroupingsRecursive(string rule, int level, string parentID) {
            var groups = new Dictionary<string, ParentheseGroup>();

            int depth = 0;
            int indexOfFirstOpenBracket = 0;
            bool ignoreNextClose = false;

            for (int stringIndexer = 0; stringIndexer < rule.Length; stringIndexer++) {
                char currentCharacter = rule.ElementAt(stringIndexer);
                bool isValidPreviousIndex = stringIndexer - 1 > 0;
                char? previousCharacter = isValidPreviousIndex ? rule.ElementAt(stringIndexer - 1) : null;

                ///
                /// Handles opening braces 
                ///
                if (currentCharacter == '(') {
                    //If the previous character is a letter, assume this particular parenthese group belongs with the prior string
                    if (previousCharacter is not null) {
                        bool previousCharIsLetter = char.IsLetter((char)previousCharacter);
                        if (previousCharIsLetter) {
                            //Otherwise ignore it because it is part of a name.
                            ignoreNextClose = true;
                        }
                    }
                    //If we should not be ignoring the next closing brace
                    if (!ignoreNextClose) {
                        //Looking at top level parenthese only
                        if (depth == 0) {
                            indexOfFirstOpenBracket = stringIndexer;
                        }
                        depth++;
                    }
                } 
                ///
                /// Handles closing braces 
                ///
                else if (currentCharacter == ')') {
                    //If the other branch decided that this particular group is not a group pre-req clause but belongs to a string.
                    //Reset the flat
                    if (ignoreNextClose)
                        ignoreNextClose = false;
                    else
                        depth--; //Otherwise decrease the depth of the search

                    //If we are dealing with the top level parenthese group again
                    if (depth == 0) {
                        int lengthOfSubstring = (stringIndexer + 1) - (indexOfFirstOpenBracket);

                        //Select the substring, NOT removing the parenthese on each end, because the end characters may not be parenthese!.
                        string groupSubstring = rule.Substring(indexOfFirstOpenBracket, lengthOfSubstring);
                        //Remove ( ) here!
                        if (groupSubstring.ElementAt(0) == '(') {
                            groupSubstring = groupSubstring.Remove(0, 1);
                            //Assume we have matching closing parenthese, potentially a bold assumption..
                            groupSubstring = groupSubstring.Remove(groupSubstring.Length - 1, 1);
                        }

                        //Get the index of the first parenthese in the substring.
                        //If there is a parenthese then there is more work needed to deconstruct the string.
                        int indexOfFirstOpenParen = groupSubstring.IndexOf('(');
                        //Indicates if this string has more substrings within it.
                        bool furtherWorkRequired = false;
                        //if we can look back at previous char, i.e. the index is not 0
                        if (indexOfFirstOpenParen >= 1) {
                            char prevChar = groupSubstring.ElementAt(indexOfFirstOpenParen - 1);
                            //If the previous character is not a letter then we are assuming there is more parsing work to be done
                            if (!char.IsLetter(prevChar)) {
                                furtherWorkRequired = true;
                            } 
                            //If it is a letter then the parenthese belongs with previous words.
                            else {
                                if (groupSubstring.Count(i => i == '(') > 1) {
                                    furtherWorkRequired = true;
                                }
                            }
                        }
                        var range = new Range(indexOfFirstOpenBracket, stringIndexer);

                        var guid = Guid.NewGuid();

                        groups.Add(guid.ToString(),
                                   new ParentheseGroup(range,
                                                       groupSubstring,
                                                       furtherWorkRequired,
                                                       level,
                                                       guid.ToString(),
                                                       parentID));
                    }
                }
            }

            level += 1;

            //Go through 
            var tempResults = new Dictionary<string, ParentheseGroup>();
            foreach (var element in groups.Values) {
                //If we need to break this down further...
                if (element.CanBeBrokenDownFurther) {
                    var groupings = BreakdownParentheseGroupingsRecursive(element.GroupString, level, element.ID);
                    foreach (var keyValuePair in groupings) {
                        if (!tempResults.ContainsKey(keyValuePair.Key))
                            tempResults.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }

            //Add temporary results back into main dictionary.
            foreach (var element in tempResults) {
                groups.Add(element.Key, element.Value);
            }

            return groups;
        }

        public static Dictionary<string, ParentheseGroup> ParseParentheseGroups(IEnumerable<EnrolmentRule> rules) {
            //Get breakdown for all rules.
            Guid TOP_LEVEL_PARENT_ID = Guid.NewGuid();
            int decompositionLevel = 0;
            var results = new Dictionary<string, ParentheseGroup>();

            //If the collection isnt empty
            if (rules.Any()) {
                //Add Top level statement to dictionary first.
                var topLevelRule = rules.ElementAt(0);
                var topLevelRange = new Range(0, topLevelRule.Description.Length - 1);

                var topLevelId = Guid.NewGuid();

                //Add the first element as the root to the list.
                results.Add(topLevelId.ToString(), new ParentheseGroup(topLevelRange,
                                                                     topLevelRule.Description,
                                                                     true,
                                                                     decompositionLevel++,
                                                                     topLevelId.ToString(),
                                                                     TOP_LEVEL_PARENT_ID.ToString()));

                //Decompose all rules and appended to results dictionary.
                var decomposedRules = DecomposeRules(rules, decompositionLevel, topLevelId);
                decomposedRules.ToList().ForEach(x => results.Add(x.Key, x.Value));

                //
                ReplaceGroupValuesWithReferences(TOP_LEVEL_PARENT_ID, results);
            }

            return results;
        }

        private static void ReplaceGroupValuesWithReferences(Guid TOP_LEVEL_PARENT_ID, Dictionary<string, ParentheseGroup> results) {
            //Go through each grouping and replace it'stringValue occurance in its' parents' string with it'stringValue ID.
            foreach (var element in results.Values.Reverse()) {
                if (element.ParentID != TOP_LEVEL_PARENT_ID.ToString()) {
                    //Get reference to parent item
                    var parentElement = results[element.ParentID.ToString()];

                    var range = element.CharacterRangeInParentString;

                    //Remove the original value from parent string
                    parentElement.GroupString = parentElement.GroupString.Remove(   range.Start.Value, 
                                                                                    range.GetLength());

                    //Insert reference to group ID
                    parentElement.GroupString = parentElement.GroupString.Insert(   range.Start.Value,
                                                                                    element.ID.ToString());

                    //Reassign.
                    results[element.ParentID.ToString()] = parentElement;
                }
            }
        }

        private static Dictionary<string, ParentheseGroup> DecomposeRules(IEnumerable<EnrolmentRule> rules, int decompositionLevel, Guid topLevelId) {
            Dictionary<string, ParentheseGroup> results = new();
           
            foreach (var rule in rules) {
                var groupings = BreakdownParentheseGroupingsRecursive(rule.Description, decompositionLevel, topLevelId.ToString()); //Use ParentID of 0 to signify there is no parent.
                foreach (var item in groupings) {
                    results.Add(item.Key, item.Value);
                }
            }

            return results;
        }
    }

    public struct ParentheseGroup
    {
        public ParentheseGroup(Range stringRange, string stringValue, bool canBeBrokenDownFurther, int decompositionLevel, string id, string parentId) {
            CharacterRangeInParentString = stringRange;
            GroupString = stringValue;
            CanBeBrokenDownFurther = canBeBrokenDownFurther;
            Level = decompositionLevel;
            ID = id;
            ParentID = parentId;
        }

        public Range CharacterRangeInParentString { get; init; }
        public string GroupString { get; set; }
        public bool CanBeBrokenDownFurther { get; init; }
        public int Level { get; init; }
        public string ParentID { get; init; }
        public string ID { get; init; }

        public override string ToString() {
            return GroupString + " LVL: " + Level;
        }
    }

}