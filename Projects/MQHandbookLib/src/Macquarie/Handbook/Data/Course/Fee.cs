//#define IGNORE_UNNECESSARY

using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record Fee : IdentifiableRecord
    {

        [JsonProperty("fee_per_credit_point")]
        public string FeePerCreditPoint { get; set; }
        [JsonProperty("fee_note")]
        public string FeeNote { get; set; }
        [JsonProperty("other_fee_type")]
        public string OtherFeeType { get; set; }
        [JsonProperty("intakes")]
        public List<KeyValueIdType> Intakes { get; set; }
        [JsonProperty("applies_to_all_intakes")]
        public bool AppliesToAllIntakes { get; set; }
        [JsonProperty("estimated_annual_fee")]
        public string EstimatedAnnualFee { get; set; }
        [JsonProperty("fee_type")]
        public LabelledValue FeeType { get; set; }
    }
}