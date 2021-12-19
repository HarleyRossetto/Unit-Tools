using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared;

public class Requirement
{
    /*
        Domain

        One of:
            Behaviour
            Physical capability
            Communication
            Cognition

            Working with children check
            Fitness to practice
            Other requirements
    */
    [JsonProperty("domain")]
    public string Domain { get; set; }


    [JsonProperty("rules")]
    public List<Rule> Rules { get; set; }
}
