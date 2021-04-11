//#define IGNORE_UNNECESSARY

using System;
using System.Collections.Generic;

using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit.Prerequisites;
using Macquarie.Handbook.Helpers;
using static Macquarie.JSON.JsonSerialisationHelper;

using Newtonsoft.Json;

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
                if (value is not null) {
                    _enrolmentRules = value;

                    //Sanitise the input.
                    RemoveEscapeSequencesFromPrerequisites();

                    //Original Implementation
                    PrerequisiteParserOld.ParsePrerequisites(value, Code);

                    //Testing implementations
                    //EnrolmentRules.ForEach(x => PrerequisiteParserOld.ParsePrerequisiteString(x.Description));
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
    }
}