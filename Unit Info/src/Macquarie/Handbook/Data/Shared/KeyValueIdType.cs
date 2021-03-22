//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class KeyValueIdType
    {
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("key")]
#endif
        public string Key { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("type")]
#endif
        public string Type { get; set; }

        public override string ToString() {
            return Value;
        }
    }
}