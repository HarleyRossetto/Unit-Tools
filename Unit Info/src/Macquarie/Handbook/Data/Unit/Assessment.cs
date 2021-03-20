using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Helpers;
using Newtonsoft.Json;

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
        private string _Description;
        [JsonProperty("description")]
        public string Description {
            get {
                return _Description;
            }
            set {
                _Description = HTMLTagStripper.StripHtmlTags(value);
            }
        }
        [JsonProperty("cl_id")]
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