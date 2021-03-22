//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Converters;
using Macquarie.Handbook.Data.Course;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data
{
    public class MacquarieCourse : MacquarieMetadata
    {
        [JsonProperty("data")]
        [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieCourseData>))]
        public MacquarieCourseData CourseData { get; set; }
        [JsonProperty("CurriculumStructure")]
        [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieCurriculumStructureData>))]
        public MacquarieCurriculumStructureData CurriculumData { get; set; }
        [JsonProperty("urlYear")]
        public string UrlYear { get; set; }
        [JsonProperty("generic")]
        public string Generic { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("modUserName")]
#endif
        public string ModUserName { get; set; }
        [JsonProperty("urlMap")]
        public string UrlMap { get; set; }
    }
}