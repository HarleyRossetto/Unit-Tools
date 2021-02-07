using System.Collections.Generic;
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
        public string Description { get; set; }
        [JsonProperty("domain")]
        public LabelledValue Domain { get; set; }
        [JsonProperty("links")]
        public List<string> Links { get; set; }
    }
}
