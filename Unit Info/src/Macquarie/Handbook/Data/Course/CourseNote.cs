using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class CourseNote
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("type")]
        public LabelledValue Type { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("display_value")]
        public string DisplayValue { get; set; }
    }
}