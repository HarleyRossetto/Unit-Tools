using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class Requirement
    {
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("rules")]
        public List<Rule> Rules { get; set; }
    }
    public class Rule
    {
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("domain")]
        public LabelledValue Domain { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("links")]
#endif
        public List<string> Links { get; set; }
    }
}
