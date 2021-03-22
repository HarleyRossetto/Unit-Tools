using System.Collections.Generic;
using Macquarie.Handbook.Data.Helpers;
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
        private string description;

        [JsonProperty("description")]
        public string Description { get => description; set => description = HTMLTagStripper.StripHtmlTags(value); }
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
