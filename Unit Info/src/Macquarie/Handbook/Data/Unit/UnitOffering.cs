//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public class UnitOffering
    {
        [JsonProperty("publish")]
        public string Publish { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("fees_domestic")]
#endif  
        public string FeesDomestic { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("location")]
#endif
        public LabelledValue Location { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("clarification_to_appear_in_handbook")]
#endif
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
#else
        [JsonProperty("self_enrol")]
#endif
        public string SelfEnrol { get; set; }
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItem { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public string Order { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("fees_commonwealth")]
#endif  
        public string FeesCommonwealth { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("fees_international")]
#endif  
        public string FeesInternational { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("course_restrictions")]
#endif  
        public string CourseRestrictions { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("quota_limit")]
#endif
        public string QuotaLimit { get; set; }

        public override string ToString() {
            return DisplayName;
        }
    }
}