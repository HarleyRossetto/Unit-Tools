using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public record IdentifiableRecord
    {

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("cl_id")]
        public string CL_ID { get; init; }
    }
}