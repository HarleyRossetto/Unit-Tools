//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record CourseNote : IdentifiableRecord
    {
        [JsonProperty("note")]
        public string Note { get; set; }
        [JsonProperty("type")]
        public LabelledValue Type { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("number")]
#endif
        public string Number { get; set; }
        [JsonProperty("display_value")]
        public string DisplayValue { get; set; }
    }
}