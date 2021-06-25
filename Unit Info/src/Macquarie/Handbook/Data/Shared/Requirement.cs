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
}
