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
        public string InnerJsonData { 
            get {
                return _InnerJsonData;
            }
            set {
                //Let's not worry storing the json string again, we don't need it.
                //_InnerJsonData = value;

                //Deserialise the inner json.
                //UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref _InnerJsonData);
                UnitData = DeserialiseInnerJson<MacquarieUnitData>(ref value);

                //Parse some prereqs while we are at it?
                IEnumerable<EnrolmentRule> preReqsRaw =     from rule in UnitData.EnrolmentRules
                                                            where rule.Type.Value == "prerequisite"
                                                            select rule;

                Regex regex2020UnitCode                 = new Regex(@"\b([A-Z]{4})(\d{4})\b");
                Regex regexPre2020UnitCode_variation1   = new Regex(@"\b([A-Z]{4})(\d{3})\b");
                Regex regexPre2020UnitCode_variation2   = new Regex(@"\b([A-Z]{3})(\d{3})\b");
                Regex regexPre2020UnitCode_variation3   = new Regex(@"\b([A-Z]{3})(\s{1})(\d{3})\b");

                List<Regex> regexFilters = new List<Regex>() {  regex2020UnitCode,
                                                                regexPre2020UnitCode_variation1,
                                                                regexPre2020UnitCode_variation2,
                                                                regexPre2020UnitCode_variation3};

                List<EnrolmentRule> tempNewRules = new List<EnrolmentRule>(3);

                foreach (var prerequsite in preReqsRaw) {
                    foreach (var filter in regexFilters) {
                        var matches = filter.Match(prerequsite.Description);

                        foreach (var prerequisiteSubject in matches.Captures) {
                            EnrolmentRule newRule = new EnrolmentRule();
                            newRule.Type = new LabelledValue() { Label = "Pre-requsite Parsed", Value = "prerequisiteparsed"};
                            newRule.Description = prerequisiteSubject.ToString();
                            tempNewRules.Add(newRule);
                        }
                    }
                }

                UnitData.EnrolmentRules.AddRange(tempNewRules);
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
    }
}