using System;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Macquarie.Handbook.Data
{
    public class MacquarieUnit : MacquarieMetadata
    {
        //Must be populated after object deserialised
        public MacquarieUnitData UnitData { get; set; }

        [JsonProperty("data")]
        public string InnerJsonData
        {
            get
            {
                return _InnerJsonData;
            }
            set
            {
                //Let's not worry storing the json string again, we don't need it.
                //_InnerJsonData = value;

                //Deserialise the inner json.
                //UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref _InnerJsonData);
                UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref value);

                ParsePrerequisites();
            }
        }

        [JsonProperty("creditPoints")]
        public UInt16 CreditPoints { get; set; }
        [JsonProperty("sysId")]
        public string SysId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("baseType")]
        public string BaseType { get; set; }
        [JsonProperty("mode")]
        public string Mode { get; set; }
        [JsonProperty("academicOrg")]
        public string AcademicOrg { get; set; }
        [JsonProperty("level")]
        public string Level { get; set; }
        [JsonProperty("active")]
        public string Active { get; set; }
        [JsonProperty("teachingPeriod")]
        public string TeachingPeriod { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("publishedInHandbook")]
        public UInt16 PublishedInHandbook { get; set; }
        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("levelDisplay")]
        public string LevelDisplay { get; set; }
        [JsonProperty("effectiveDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }


        private void ParsePrerequisites()
        {
            //Parse some prereqs while we are at it?
            //Potentially move this later into some kind of observer / notifier system to accomodate
            //more runtime data extraction.
            IEnumerable<EnrolmentRule> preReqsRaw = from rule in UnitData.EnrolmentRules
                                                    where rule.Type.Value == "prerequisite"
                                                    select rule;

            //Matches 4 characters and 4 digits, beginning and ending on word boundaries.
            //i.e. COMP1000
            Regex regex2020UnitCode = new Regex(@"\b([A-Z]{4})(\d{4})\b");
            //Matches 4 characters and 3 digits, beginning and ending on word boundaries.
            //i.e. COMP125
            Regex regexPre2020UnitCode_variation1 = new Regex(@"\b([A-Z]{4})(\d{3})\b");
            //Matches 3 characters and 3 digits, beginning and ending on word boundaries.
            //i.e. BCM102
            Regex regexPre2020UnitCode_variation2 = new Regex(@"\b([A-Z]{3})(\d{3})\b");
            //Matches 3 characters, a single whitespace and 3 digits, beginning and ending on word boundaries.
            //i.e. MAS 110
            Regex regexPre2020UnitCode_variation3 = new Regex(@"\b([A-Z]{3})(\s{1})(\d{3})\b");

            //Throw these in a list
            List<Regex> regexFilters = new List<Regex>() {  regex2020UnitCode,
                                                                regexPre2020UnitCode_variation1,
                                                                regexPre2020UnitCode_variation2,
                                                                regexPre2020UnitCode_variation3};

            //We need a temporary list to hold new rules because we cannot modify UnitData.EnrolementRules
            //whilst we operating on the results of the LINQ query;
            List<EnrolmentRule> tempNewRules = new List<EnrolmentRule>(3);

            foreach (var prerequsite in preReqsRaw)
            {
                foreach (var filter in regexFilters)
                {
                    var matches = filter.Match(prerequsite.Description);

                    foreach (var prerequisiteSubject in matches.Captures)
                    {
                        EnrolmentRule newRule = new EnrolmentRule();
                        //Use "prerequsiteparsed" as a flag to let us know this is a value we can work with directly.
                        newRule.Type = new LabelledValue() { Label = "Pre-requsite Parsed", Value = "prerequisiteparsed" };
                        newRule.Description = prerequisiteSubject.ToString();
                        tempNewRules.Add(newRule);
                    }
                }
            }

            //Add our extracted rules into the units' enrolement rules list.
            UnitData.EnrolmentRules.AddRange(tempNewRules);
        }
    }
}