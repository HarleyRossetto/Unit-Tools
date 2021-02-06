using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class LearningOutcome 
    {
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("order")]
        public string Order { get; set; }
        public string LinkingId { get; set; }
        [JsonProperty("lo_cl_id")]
        public string LO_CL_ID { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItem { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
    }
}