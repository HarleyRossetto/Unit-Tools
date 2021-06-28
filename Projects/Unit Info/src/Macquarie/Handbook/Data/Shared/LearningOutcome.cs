//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;
using Macquarie.Handbook.Helpers;
using Macquarie.Handbook.Converters;

namespace Macquarie.Handbook.Data.Shared
{
    public record LearningOutcome : IdentifiableRecord
    {

        [JsonProperty("description")]
       // [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("number")]
        public string Number { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("order")]
        public string Order { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("linking_id")]
        public string LinkingId { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("lo_cl_id")]
        public string LO_CL_ID { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItem { get; set; }

        public override string ToString() {
            return Description;
        }
    }
}