//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record OrgUnitData : IdentifiableRecord
    {
        [JsonProperty("parent")]
        public LabelledValue Parent { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}