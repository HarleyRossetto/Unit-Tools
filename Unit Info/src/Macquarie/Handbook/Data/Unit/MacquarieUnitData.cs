//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;
using Macquarie.Handbook.Data.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Macquarie.Handbook.Data.Unit.Prerequisites;

namespace Macquarie.Handbook.Data.Unit
{
    public class MacquarieUnitData : MacquarieMaterialMetadata
    {
        [JsonProperty("grading_schema")]
        public LabelledValue GradingSchema { get; set; }
        [JsonProperty("study_level")]
        public LabelledValue StudyLevel { get; set; }
        [JsonProperty("quota_enrolment_requirements")]
        public string QuoteEnrolmentRequirements { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("duration_ft_max")]
#endif  
        public string DurationFullTimeMax { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("duration_pt_max")]
#endif  
        public string DurationPartTimeMax { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("duration_pt_std")]
#endif  
        public string DurationPartTimeStandard { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("duration_pt_min")]
#endif  
        public string DurationPartTimeMinimum { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("duration_pt_period")]
#endif  
        public LabelledValue DurationPartTimePeriod { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_id")]
#endif
        public KeyValueIdType ParentId { get; set; }
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }
        [JsonProperty("exclusions")]
        public string Exclusions { get; set; }
        [JsonProperty("level")]
        public KeyValueIdType Level { get; set; }
        [JsonProperty("uac_code")]
        public string UACCode { get; set; }
        [JsonProperty("special_requirements")]
        public string SpecialRequirements { get; set; }
        [JsonProperty("special_unit_type")]
        public List<LabelledValue> SpecialUnitType { get; set; }
        [JsonProperty("version_status")]
        public LabelledValue VersionStatus { get; set; }
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("sms_status")]
#endif
        public LabelledValue SMSStatus { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("sms_version")]
#endif
        public string SMSVersion { get; set; }
        [JsonProperty("learning_materials")]
        public string LearningMaterials { get; set; }
        [JsonProperty("special_topic")]
        public bool SpecialTopic { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("d_gov_cohort_year")]
#endif
        public bool d_gov_cohort_year { get; set; }
        [JsonProperty("asced_broad")]
        public KeyValueIdType AscedBroad { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("publish_tuition_fees")]
#endif  
        public bool PublishTuitionFees { get; set; }
        [JsonProperty("placement_proportion")]
        public LabelledValue PlacementProportion { get; set; }
        [JsonProperty("unit_description")]
        public List<string> UnitDescription { get; set; }
        [JsonProperty("unit_learning_outcomes")]
        public List<LearningOutcome> UnitLearningOutcomes { get; set; }
        [JsonProperty("non_scheduled_learning_activities")]
        public List<NonScheduledLearningActivity> NonScheduledLearningActivities { get; set; }
        [JsonProperty("scheduled_learning_activites")]
        public List<ScheduledLearningActivity> ScheduledLearningActivites { get; set; }
        private List<EnrolmentRule> _enrolmentRules;
        [JsonProperty("enrolment_rules")]
        public List<EnrolmentRule> EnrolmentRules {
            get {
                return _enrolmentRules;
            }
            set {
                if (value != null) {
                    _enrolmentRules = value;

                    SanitisePreRequsites();


                    //ParsePrerequisites();
                }
            }
        }
        [JsonProperty("assessments")]
        public List<Assessment> Assessments { get; set; }
        [JsonProperty("requisites")]
        public List<Requisite> Requisites { get; set; }
        [JsonProperty("unit_offering")]
        public List<UnitOffering> UnitOffering { get; set; }
        [JsonProperty("unit_offering_text")]
        public string UnitOfferingText { get; set; }
        [JsonProperty("subject_search_title")]
        public string SubjectSearchTitle { get; set; }

        private void SanitisePreRequsites() {
            foreach (var item in EnrolmentRules) {
                if (item.Type.Value == "prerequisite" && (item.Description.Contains("\n") || item.Description.Contains("\t"))) {
                    item.Description = item.Description.Replace("\n", "").Replace("\t", " ");
                }
            }
        }

        private void ParsePrerequisites() {
            //Parse some prereqs while we are at it?
            //Potentially move this later into some kind of observer / notifier system to accomodate
            //more runtime data extraction.
            IEnumerable<EnrolmentRule> preReqsRaw = from rule in EnrolmentRules
                                                    where rule.Type.Value == "prerequisite"
                                                    select rule;

            ParseEnrolmentRegexParentheses(preReqsRaw);

            ParseParentheseGroups(preReqsRaw);

            //Add our extracted rules into the units' enrolement rules list.
            EnrolmentRules.AddRange(ParseEnrolmentRulesWithRegex(preReqsRaw));
        }

        //Matches 4 characters and 4 digits, beginning and ending on word boundaries.
        //i.e. COMP1000
        [JsonIgnore]
        public static Regex regex2020UnitCode = new Regex(@"\b([A-Z]{4})(\d{4})\b");
        //Matches 4 characters and 3 digits, beginning and ending on word boundaries.
        //i.e. COMP125
        [JsonIgnore]
        public static Regex regexPre2020UnitCode_variation1 = new Regex(@"\b([A-Z]{4})(\d{3})\b");
        //Matches 3 characters and 3 digits, beginning and ending on word boundaries.
        //i.e. BCM102
        [JsonIgnore]
        public static Regex regexPre2020UnitCode_variation2 = new Regex(@"\b([A-Z]{3})(\d{3})\b");
        //Matches 3 characters, a single whitespace and 3 digits, beginning and ending on word boundaries.
        //i.e. MAS 110
        [JsonIgnore]
        public static Regex regexPre2020UnitCode_variation3 = new Regex(@"\b([A-Z]{3})(\s{1})(\d{3})\b");

        //Throw these in a list
        [JsonIgnore]
        public List<Regex> regexFilters = new List<Regex>() {  regex2020UnitCode,
                                                                regexPre2020UnitCode_variation1,
                                                                regexPre2020UnitCode_variation2,
                                                                regexPre2020UnitCode_variation3};


        private IEnumerable<EnrolmentRule> ParseEnrolmentRulesWithRegex(IEnumerable<EnrolmentRule> rules) {

            //We need a temporary list to hold new rules because we cannot modify EnrolementRules
            //whilst we operating on the results of the LINQ query;
            List<EnrolmentRule> tempNewRules = new List<EnrolmentRule>(3);

            foreach (var prerequsite in rules) {
                foreach (var filter in regexFilters) {
                    var matches = filter.Match(prerequsite.Description);

                    foreach (var prerequisiteSubject in matches.Captures) {
                        EnrolmentRule newRule = new EnrolmentRule();
                        //Use "prerequsiteparsed" as a flag to let us know this is a value we can work with directly.
                        newRule.Type = new LabelledValue() { Label = "Pre-requsite Parsed", Value = "prerequisiteparsed" };
                        newRule.Description = prerequisiteSubject.ToString();
                        tempNewRules.Add(newRule);
                    }
                }
            }
            return tempNewRules;
        }

        class Term
        {
            public string Value { get; set; }
        }

        class Join
        {
            public string LeftHandSide { get; set; }
            public string RightHandSide { get; set; }
            public JoinType JointType { get; init; }
        }

        enum JoinType
        {
            AND,
            OR
        }

        private IEnumerable<EnrolmentRule> ParseEnrolmentRegexParentheses(IEnumerable<EnrolmentRule> rules) {
            //Capture Parenthese groups, timeout of 1 second.
            Regex regexParentheses = new Regex(@"\([\s\S]*?\)", RegexOptions.None, new TimeSpan(0, 0, 1));

            List<Match> matches = new List<Match>();

            foreach (var item in rules) {
                matches.Add(regexParentheses.Match(item.Description));
            }

            return null;
        }


        //"(ELEC2070 or ELEC270) and (ELEC2005 or (ELCT2005 or ELEC295) or (ELEC2075 or ELEC275))"

        /// <summary>
        /// Splits the string into multiple elements based upon level 0 '(' ')' pairs.
        /// </summary>
        private List<Tuple<Range, string, bool>> MatchStringParentheseGroups(string s) {
            var capturedGroups = new List<Tuple<Range, string, bool>>();

            int levels = 0;
            int indexOfOpenBracket = -1;

            for (int i = 0; i < s.Length; i++) {
                char c = s.ElementAt(i);
                if (c == '(') {
                    levels++;
                    if (levels - 1 == 0) {
                        indexOfOpenBracket = i;
                    }
                } else if (c == ')') {
                    levels--;
                    if (levels == 0) {
                        int lastOpen = indexOfOpenBracket;
                        int startSubString = lastOpen;
                        int lengthSubString = i - (lastOpen - 1);
                        capturedGroups.Add(new Tuple<Range, string, bool>(new Range(startSubString, i), s.Substring(startSubString, lengthSubString), false));
                    }
                }
            }

            if (levels == 0) {
                //We are all good,
            } else {
                //We have mismatch in parentheses.
                //throw new Exception($"Mismatch of parenthese levels={levels} but should be 0");
            }

            var returnable = new List<Tuple<Range, string, bool>>(capturedGroups.Count);

            bool mustIterateAgain = false;

            foreach (var item in capturedGroups) {
                int firstOpen = item.Item2.IndexOf('(');
                int lastClose = item.Item2.LastIndexOf(')');
                //Drop first and last characters (should be '(' and ')')
                string subString = item.Item2.Substring(firstOpen + 1, lastClose - (firstOpen + 1));

                mustIterateAgain = (subString.Contains('(') || subString.Contains(')'));


                returnable.Add(new Tuple<Range, string, bool>(item.Item1, subString, mustIterateAgain));
            }

            return returnable;
        }

        struct ParentheseGroup
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

        private Dictionary<int, ParentheseGroup> BreakdownParentheseGroupingsRecursive(string rule, int level, int parentID) {
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

        private Dictionary<int, ParentheseGroup> ParseParentheseGroups(IEnumerable<EnrolmentRule> rules) {
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
}


/// HOLD THIS OLD MESS

// foreach (var element in results.Values) {
//     if (element.ParentID != 0) {
//         //Get reference to parent item
//         var parentElement = results[element.ParentID];
//         //Remove the original value from parent string
//         parentElement.GroupString = parentElement.GroupString.Remove(   element.CharacterRangeInParentString.Start.Value, 
//                                                                         element.CharacterRangeInParentString.End.Value - element.CharacterRangeInParentString.Start.Value + 2);

//         parentElement.GroupString = parentElement.GroupString.Insert(element.CharacterRangeInParentString.Start.Value, "{" + element.ID.ToString() + "}");

//         results[element.ParentID] = parentElement;
//     }
// }

/*
    Use recursion to grab groups between parentheses, each group also specifies its length within the parent string.
    Build compound tree based on results of above.

*/


/*
            var parentheseGroups = new List<List<Tuple<Range, string, bool>>>();
            foreach (var rule in rules) {
                parentheseGroups.Add(MatchStringParentheseGroups(rule.Description));
            }

            var result = MatchStringParentheseGroups(parentheseGroups[0][1].Item2);

            var matches = ParseEnrolmentRegexParentheses(rules);


            List<string[]> andSplits = new List<string[]>();

            foreach (var item in rules) {
                int index = item.Description.IndexOf("and");
                string[] split = item.Description.Split("and", 2);

                andSplits.Add(split);
            }

            List<string[]> orSplits = new List<string[]>();

            foreach (var item in rules) {
                int index = item.Description.IndexOf("or");
                string[] split = item.Description.Split("or");
                // string[] split = item.Description.Split("or", 2);

                orSplits.Add(split);
            }
    */