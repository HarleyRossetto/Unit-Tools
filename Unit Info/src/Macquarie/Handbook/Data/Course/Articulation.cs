//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record Articulation : IdentifiableRecord
    {
        [JsonProperty("course")]
        public string Course {get;set;}
        [JsonProperty("articulation_conditions")]
        public string ArticulationConditions {get;set;}
        [JsonProperty("details")]
        public string Details {get;set;}
        [JsonProperty("credit_transfer_arrangements")]
        public LabelledValue CreditTransferArrangements {get;set;}
    }
}