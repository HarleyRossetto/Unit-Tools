using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class Rule
    {
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("domain")]
        public LabelledValue Domain { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("links")]
        public List<string> Links { get; set; }
    }
}