//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;
using Macquarie.Handbook.Converters;

namespace Macquarie.Handbook.Data.Unit
{
    public class Assessment
    {
        [JsonProperty("assessment_title")]
        public string AssessmentTitle { get; set; }
        [JsonProperty("type")]
        public LabelledValue Type { get; set; }
        [JsonProperty("weight")]
        public string Weight { get; set; }
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
        [JsonProperty("applies_to_all_offerings")]
        public string AppliesToAllOfferings { get; set; }
        [JsonProperty("hurdle_task")]
        public string HurdleTask { get; set; }
        [JsonProperty("offerings")]
        public string Offerings { get; set; }
        [JsonProperty("individual")]
        public LabelledValue Individual { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}