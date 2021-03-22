//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class Articulation
    {
        [JsonProperty("course")]
        public string Course {get;set;}
        [JsonProperty("articulation_conditions")]
        public string ArticulationConditions {get;set;}
        [JsonProperty("details")]
        public string Details {get;set;}
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID {get;set;}
        [JsonProperty("credit_transfer_arrangements")]
        public LabelledValue CreditTransferArrangements {get;set;}
    }
}