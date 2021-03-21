using Newtonsoft.Json;
using Macquarie.Handbook.Data.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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

        [JsonProperty("duration_ft_max")]
        public string DurationFullTimeMax { get; set; }
        [JsonProperty("duration_pt_max")]
        public string DurationPartTimeMax { get; set; }
        [JsonProperty("duration_pt_std")]
        public string DurationPartTimeStandard { get; set; }
        [JsonProperty("duration_pt_min")]
        public string DurationPartTimeMinimum { get; set; }
        [JsonProperty("duration_pt_period")]
        public LabelledValue DurationPartTimePeriod { get; set; }
        [JsonProperty("parent_id")]
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
        [JsonProperty("sms_status")]
        public LabelledValue SMSStatus { get; set; }
        [JsonProperty("sms_version")]
        public string SMSVersion { get; set; }
        [JsonProperty("learning_materials")]
        public string LearningMaterials { get; set; }
        [JsonProperty("special_topic")]
        public bool SpecialTopic { get; set; }
        [JsonProperty("d_gov_cohort_year")]
        public bool d_gov_cohort_year { get; set; }
        [JsonProperty("asced_broad")]
        public KeyValueIdType AscedBroad { get; set; }
        [JsonProperty("publish_tuition_fees")]
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

                    ParsePrerequisites();
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

        private void ParsePrerequisites() {
            //Parse some prereqs while we are at it?
            //Potentially move this later into some kind of observer / notifier system to accomodate
            //more runtime data extraction.
            IEnumerable<EnrolmentRule> preReqsRaw = from rule in EnrolmentRules
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

            foreach (var prerequsite in preReqsRaw) {
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

            //Add our extracted rules into the units' enrolement rules list.
            EnrolmentRules.AddRange(tempNewRules);
        }
    }
}
