using Newtonsoft.Json;
using Macquarie.Handbook.Data.Shared;
using System;
using System.Collections.Generic;

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
        [JsonProperty("enrolment_rules")]
        public List<EnrolmentRule> EnrolmentRules { get; set; }
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
    }
}
