
//#define IGNORE_UNNECESSARY

using System;
using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit.Prerequisites
{
    public record EnrolmentRule : IdentifiableRecord
    {
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("type")]
        public LabelledValue Type { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public UInt16 Order { get; init; }

        public override string ToString() {
            return Description;
        }
    }
}