using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class OrgUnitData
    {
        [JsonProperty("parent")]
        public LabelledValue Parent { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
    }
}