//#define IGNORE_UNNECESSARY

using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public class LearningActivity
    {
        [JsonProperty("description")]     
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("activity")]
        public LabelledValue Activity { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
        [JsonProperty("offerings")]
        public string Offerings { get; set; }

        public override string ToString() {
            return Description;
        }
    }

    public class ScheduledLearningActivity : LearningActivity { }
    public class NonScheduledLearningActivity : LearningActivity { }
}