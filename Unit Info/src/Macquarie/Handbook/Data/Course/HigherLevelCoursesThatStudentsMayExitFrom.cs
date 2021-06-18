//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record HigherLevelCoursesThatStudentsMayExitFrom : IdentifiableRecord
    {
        [JsonProperty("code")]
        public KeyValueIdType Code { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}