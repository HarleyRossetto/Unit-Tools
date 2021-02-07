using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class HigherLevelCoursesThatStudentsMayExitFrom
    {
        [JsonProperty("code")]
        public KeyValueIdType Code { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
    }
}