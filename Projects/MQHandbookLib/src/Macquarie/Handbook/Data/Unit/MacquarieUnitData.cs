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
    public record MacquarieUnitData : MacquarieMaterialMetadata
    {
        [JsonProperty("grading_schema")]
        public LabelledValue GradingSchema { get; init; }
        [JsonProperty("study_level")]
        public LabelledValue StudyLevel { get; init; }
        [JsonProperty("quota_enrolment_requirements")]
        public string QuoteEnrolmentRequirements { get; init; }
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; init; }
        [JsonProperty("exclusions")]
        public string Exclusions { get; init; }
        [JsonProperty("level")]
        public KeyValueIdType Level { get; init; }
        [JsonProperty("uac_code")]
        public string UACCode { get; init; }
        [JsonProperty("special_requirements")]
        public string SpecialRequirements { get; init; }
        [JsonProperty("special_unit_type")]
        public List<LabelledValue> SpecialUnitType { get; init; }
        [JsonProperty("version_status")]
        public LabelledValue VersionStatus { get; init; }
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; init; }
        [JsonProperty("learning_materials")]
        public string LearningMaterials { get; init; }
        [JsonProperty("special_topic")]
        public bool SpecialTopic { get; init; }

        [JsonProperty("asced_broad")]
        public KeyValueIdType AscedBroad { get; init; }
        [JsonProperty("placement_proportion")]
        public LabelledValue PlacementProportion { get; init; }
        [JsonProperty("unit_description")]
        public List<string> UnitDescription { get; init; }
        [JsonProperty("unit_learning_outcomes")]
        public List<LearningOutcome> UnitLearningOutcomes { get; init; }
        [JsonProperty("non_scheduled_learning_activities")]
        public List<NonScheduledLearningActivity> NonScheduledLearningActivities { get; init; }
        [JsonProperty("scheduled_learning_activites")]
        public List<ScheduledLearningActivity> ScheduledLearningActivites { get; init; }
        private List<EnrolmentRule> _enrolmentRules;
        [JsonProperty("enrolment_rules")]
        public List<EnrolmentRule> EnrolmentRules {
            get {
                return _enrolmentRules;
            }
            init {
                if (value is not null) {
                    _enrolmentRules = value;

                    //Original Implementation
                    //PrerequisiteParserOld.ParsePrerequisites(value, Code);

                    //Testing implementations
                    //EnrolmentRules.ForEach(x => PrerequisiteParserOld.ParsePrerequisiteString(x.Description));
                }
            }
        }
        [JsonProperty("assessments")]
        public List<Assessment> Assessments { get; init; }
        [JsonProperty("requisites")]
        public List<Requisite> Requisites { get; init; }
        [JsonProperty("unit_offering")]
        public List<UnitOffering> UnitOffering { get; init; }
        [JsonProperty("unit_offering_text")]
        public string UnitOfferingText { get; init; }
        [JsonProperty("subject_search_title")]
        public string SubjectSearchTitle { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("publish_tuition_fees")]
        public bool PublishTuitionFees { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("d_gov_cohort_year")]
        public bool D_gov_cohort_year { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("sms_status")]
        public LabelledValue SMSStatus { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("sms_version")]
        public string SMSVersion { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("duration_ft_max")]
        public string DurationFullTimeMax { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("duration_pt_max")]
        public string DurationPartTimeMax { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("duration_pt_std")]
        public string DurationPartTimeStandard { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("duration_pt_min")]
        public string DurationPartTimeMinimum { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("duration_pt_period")]
        public LabelledValue DurationPartTimePeriod { get; init; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("parent_id")]
        public KeyValueIdType ParentId { get; init; }
    }
}