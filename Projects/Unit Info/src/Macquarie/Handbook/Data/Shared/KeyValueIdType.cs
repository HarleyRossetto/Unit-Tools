//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public record KeyValueIdType : IdentifiableRecord
    {

        [JsonProperty("value")]
        public string Value { get; set; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("key")]
        public string Key { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("type")]
        public string Type { get; set; }

        public override string ToString() {
            return Value;
        }
    }
}