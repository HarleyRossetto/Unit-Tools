//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public record UnitOffering : IdentifiableRecord
    {
        [JsonProperty("publish")]
        public string Publish { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("fees_domestic")]
        public string FeesDomestic { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("location")]
        public LabelledValue Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("clarification_to_appear_in_handbook")]
        public string ClarificationToAppearInHandbook { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("teaching_period")]
        public KeyValueIdType TeachingPeriod { get; set; }
        [JsonProperty("attendance_mode")]
        public KeyValueIdType AttendanceMode { get; set; }
        [JsonProperty("quota_number")]
        public string QuotaNumber { get; set; }
        [JsonProperty("study_level")]
        public LabelledValue StudyLevel { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("self_enrol")]
        public string SelfEnrol { get; set; }
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItem { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("order")]
        public string Order { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("fees_commonwealth")]
        public string FeesCommonwealth { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("fees_international")]
        public string FeesInternational { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("course_restrictions")]
        public string CourseRestrictions { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("quota_limit")]
        public string QuotaLimit { get; set; }

        public override string ToString() {
            return DisplayName;
        }
    }
}