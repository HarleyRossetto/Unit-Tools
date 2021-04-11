//#define IGNORE_UNNECESSARY

using Newtonsoft.Json;
using Macquarie.Handbook.Helpers;
using Macquarie.Handbook.Converters;

namespace Macquarie.Handbook.Data.Shared
{
    public class LearningOutcome
    {
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("number")]
#endif
        public string Number { get; set; }
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public string Order { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("linking_id")]
#endif
        public string LinkingId { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("lo_cl_id")]
#endif
        public string LO_CL_ID { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("academic_item")]
#endif
        public KeyValueIdType AcademicItem { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }

        public override string ToString() {
            return Description;
        }
    }
}