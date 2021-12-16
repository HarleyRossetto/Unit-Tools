namespace Macquarie.Handbook.Data.Shared;

using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;


public class Rule
{
    [JsonProperty("description")]
    [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
    public string Description { get; set; }
    [JsonProperty("domain")]
    public LabelledValue Domain { get; set; }

    [JsonProperty("links")]
    public List<string> Links { get; set; }
}
