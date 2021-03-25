//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;
using Macquarie.Handbook.Data.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using Macquarie.Handbook.Data.Helpers;

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

                    //Sanitise the input.
                    RemoveEscapeSequencesFromPrerequisites();


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


        private void RemoveEscapeSequencesFromPrerequisites() {
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

            //ParseEnrolmentRegexParentheses(preReqsRaw);

            var parentheseGroups = EnrolmentRuleParentheseParser.ParseParentheseGroups(preReqsRaw);

            var hashedStrings = from v in parentheseGroups.Keys
                                select v.ToString();

            var connectorStructureDictionary = new Dictionary<int, Tuple<Connector, ParentheseGroup>>();

            foreach (var group in parentheseGroups.Reverse()) {
                //If the group cannot be broken down further, we can beging parsing for other
                //tokens (AND, OR)
                // if (!group.Value.CanBeBrokenDownFurther) {
                Connector result = null;
                //result = TryParseOrStructure(group.Value);
                result = TryParseStructure(ConnectorType.OR, group.Value);
                // result = (result == null ? TryParseAndStructure(group.Value) : result);
                result = (result == null ? TryParseStructure(ConnectorType.AND, group.Value) : result);


                if (result != null)
                    connectorStructureDictionary.Add(group.Value.ID, new Tuple<Connector, ParentheseGroup>(result, group.Value));
                else {
                    result = new Connector() { ConnectionType = ConnectorType.STATEMENT };
                    result.StringValues.Add(group.Value.GroupString);
                    connectorStructureDictionary.Add(group.Value.ID, new Tuple<Connector, ParentheseGroup>(result, group.Value));
                }
            }

            //Regex braceExpression = new Regex(@"\{-?(\d{10})\}");
            Regex hashcodeRegex = new Regex(@"-?\d{6,10}"); // USING HASHCODE IS TERRIBLE IDEA. CHANGE TO A BETTER METHOD.
            foreach (var expression in connectorStructureDictionary.Values) {
                /*
                    If the expression is in its most basic form, find its parent
                    and begin rebuilding the sequence as a graph.
                */
                if (expression.Item2.CanBeBrokenDownFurther) {
                    var removeOnCompletion = new List<string>(10);
                    foreach (var parentIdHash in expression.Item1.StringValues) {
                        // bool conStructContainsHash = connectorStructureDictionary.Keys.Contains(Convert.ToInt32(parentIdHash));

                        // if (conStructContainsHash) {

                        // }

                        var bracedValue = hashcodeRegex.Match(parentIdHash);
                        int hashCode = Convert.ToInt32(bracedValue.Value);
                        expression.Item1.ConnectorValues.Add(connectorStructureDictionary[hashCode].Item1);

                        removeOnCompletion.Add(parentIdHash);
                    }

                    removeOnCompletion.ForEach(s => expression.Item1.StringValues.Remove(s));
                }
            }

            if (connectorStructureDictionary.Values.Count > 0) {
                var topLevelConnector = connectorStructureDictionary.Values.Last().Item1;
                topLevelConnector.OriginalString = preReqsRaw.First().Description;

                MacquarieHandbook.SerialiseObjectToJsonFile(topLevelConnector, $"data/parsed/prerequisites/{Code}.json");

                PrintPrereqGraph(topLevelConnector, 0);
            }
        }

        private void PrintPrereqGraph(Connector connector, int level) {
            if (connector.IsMostBasic) {
                var lastString = connector.StringValues.Last();
                foreach (var preReqs in connector.StringValues) {
                    Console.WriteLine(preReqs);
                    if (preReqs != lastString)
                        System.Console.WriteLine(level + " " + connector.ConnectionType.ToString());
                }
            } else {
                var lastConnector = connector.ConnectorValues.Last();
                foreach (var child in connector.ConnectorValues) {
                    PrintPrereqGraph(child, level++);
                    if (child != lastConnector)
                        System.Console.WriteLine(level + " " + connector.ConnectionType.ToString());
                }
            }
        }

        private void WriteNTabs(int n) {
            for (int i = 0; i < n; i++)
                Console.Write("\t");
        }

        private Connector TryParseStructure(ConnectorType type, ParentheseGroup group) {
            string filter = string.Format(" {0} ", type.ToString().ToLower());
            if (group.GroupString.Contains(filter)) {
                var split = group.GroupString.Split(filter, StringSplitOptions.TrimEntries);

                var connector = new Connector() { ConnectionType = type };
                connector.StringValues.AddRange(split);
                return connector;
            }
            return null;
        }

        #region REGEX_STUFF

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

        private IEnumerable<EnrolmentRule> ParseEnrolmentRegexParentheses(IEnumerable<EnrolmentRule> rules) {
            //Capture Parenthese groups, timeout of 1 second.
            Regex regexParentheses = new Regex(@"\([\s\S]*?\)", RegexOptions.None, new TimeSpan(0, 0, 1));

            List<Match> matches = new List<Match>();

            foreach (var item in rules) {
                matches.Add(regexParentheses.Match(item.Description));
            }

            return null;
        }

        #endregion

        #region OLDER_VERSION_OF_SOME_CODE_DEAL_WITH_SOON
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
    }
    #endregion
}

public class Connector
{
    public string OriginalString { get; set; }
    public ConnectorType ConnectionType { get; init; }
    public List<string> StringValues { get; set; } = new List<string>(2);
    public List<Connector> ConnectorValues { get; set; } = new List<Connector>(2);

    [JsonIgnore]
    public bool IsMostBasic {
        get {
            if (ConnectorValues == null) {
                return false;
            } else {
                return ConnectorValues.Count == 0;
            }
        }
    }

    public override string ToString() {
        return "Connector: " + ConnectionType.ToString();
    }
}

public enum ConnectorType
{
    AND,
    OR,
    NONE,
    STATEMENT
}

#region  OLD_CODE_PROB_WONT_REUSE


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

#endregion