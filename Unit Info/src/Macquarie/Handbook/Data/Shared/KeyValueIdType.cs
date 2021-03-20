using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class KeyValueIdType
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}