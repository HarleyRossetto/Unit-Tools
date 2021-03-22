

using System;
using System.Collections.Generic;
using System.Linq;
using Macquarie.Handbook.Data.Unit.Prerequisites;

namespace Macquarie.Handbook.Data.Helpers {
    public static class EnrolmentRuleParentheseParser
    {
       public static Dictionary<int, ParentheseGroup> BreakdownParentheseGroupingsRecursive(string rule, int level, int parentID) {
            var groups = new Dictionary<int, ParentheseGroup>();

            int levels = 0;
            int indexOfFirstOpenBracket = 0;
            bool ignoreNextClose = false;

            for (int stringIndexer = 0; stringIndexer < rule.Length; stringIndexer++) {
                char currentCharacter = rule.ElementAt(stringIndexer);
                bool isValidPreviousIndex = stringIndexer - 1 > 0;
                char? previousCharacter = isValidPreviousIndex ? rule.ElementAt(stringIndexer - 1) : null;

                if (currentCharacter == '(') {
                    if (previousCharacter != null) {
                        bool previousCharIsLetter = char.IsLetter((char)previousCharacter);
                        if (previousCharIsLetter) {
                            //Otherwise ignore it because it is part of a name.
                            ignoreNextClose = true;
                        }
                    }
                    if (!ignoreNextClose) {
                        //Looking at top level parenthese only
                        if (levels == 0) {
                            indexOfFirstOpenBracket = stringIndexer;
                        }
                        levels++;
                    }
                } else if (currentCharacter == ')') {
                    if (ignoreNextClose)
                        ignoreNextClose = false;
                    else
                        levels--;

                    if (levels == 0) {
                        int lengthOfSubstring = (stringIndexer + 1) - (indexOfFirstOpenBracket);

                        //Select the substring, NOT removing the parenthese on each end, because it might NOT be a parenthese!.
                        string groupSubstring = rule.Substring(indexOfFirstOpenBracket, lengthOfSubstring);
                        //Remove ( ) here!
                        if (groupSubstring.ElementAt(0) == '(') {
                            groupSubstring = groupSubstring.Remove(0, 1);
                            //Assume we have matching closing parenthese
                            groupSubstring = groupSubstring.Remove(groupSubstring.Length - 1, 1);
                        }

                        int indexOfFirstOpenParen = groupSubstring.IndexOf('(');
                        bool furtherWorkRequired = false;
                        //if we can look back at previous char
                        if (indexOfFirstOpenParen >= 1) {
                            char prevChar = groupSubstring.ElementAt(indexOfFirstOpenParen - 1);
                            //Previous character is not a letter, then yes. If it is a letter then the paren belongs with previous words.
                            if (!char.IsLetter(prevChar)) {
                                furtherWorkRequired = true;
                            } else {
                                if (groupSubstring.Count(i => i == '(') > 1) {
                                    furtherWorkRequired = true;
                                }
                            }
                        }
                        //If the string still contains ( or ) then we need to parse it more.
                        //furtherWorkRequired = (groupSubstring.Contains('(') || groupSubstring.Contains(')'));
                        var range = new Range(indexOfFirstOpenBracket, stringIndexer);
                        int hashCode = groupSubstring.GetHashCode() + range.GetHashCode();
                        groups.Add(hashCode, new ParentheseGroup(range, groupSubstring, furtherWorkRequired, level, hashCode, parentID));
                    }
                }
            }

            //We have a parenthese mismatch somewhere!
            if (levels != 0) {

            }

            level += 1;

            //Go through 
            var tempResults = new Dictionary<int, ParentheseGroup>();
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

        public static Dictionary<int, ParentheseGroup> ParseParentheseGroups(IEnumerable<EnrolmentRule> rules) {
            //Get breakdown for all rules.
            const int TOP_LEVEL_PARENT_ID = 0;
            var results = new Dictionary<int, ParentheseGroup>();
            foreach (var rule in rules) {
                var groupings = BreakdownParentheseGroupingsRecursive(rule.Description, 0, TOP_LEVEL_PARENT_ID); //Use ParentID of 0 to signify there is no parent.
                foreach (var item in groupings) {
                    results.Add(item.Key, item.Value);
                }
            }

            //Go through each grouping and replace it's occurance in its' parents' string with it's ID.
            for (int i = results.Values.Count - 1; i >= 0; i--) {
                var element = results.Values.ElementAt(i);
                if (element.ParentID != 0) {
                    //Get reference to parent item
                    var parentElement = results[element.ParentID];
                    //Remove the original value from parent string
                    parentElement.GroupString = parentElement.GroupString.Remove(element.CharacterRangeInParentString.Start.Value,
                                                                                    element.CharacterRangeInParentString.End.Value - element.CharacterRangeInParentString.Start.Value + 1);

                    //Insert reference to group ID
                    parentElement.GroupString = parentElement.GroupString.Insert(element.CharacterRangeInParentString.Start.Value, "{" + element.ID.ToString() + "}");

                    //Reassign.
                    results[element.ParentID] = parentElement;
                }
            }

            return results;
        }
    }

    public struct ParentheseGroup
        {
            public ParentheseGroup(Range r, String s, Boolean b, int l, int id, int parentId) {
                CharacterRangeInParentString = r;
                GroupString = s;
                CanBeBrokenDownFurther = b;
                Level = l;
                ID = id;
                ParentID = parentId;
            }

            public Range CharacterRangeInParentString { get; init; }
            public String GroupString { get; set; }
            public Boolean CanBeBrokenDownFurther { get; init; }
            public int Level { get; init; }
            public int ParentID { get; init; }
            public int ID { get; init; }

            public override string ToString() {
                return GroupString + " LVL: " + Level;
            }
        }

}