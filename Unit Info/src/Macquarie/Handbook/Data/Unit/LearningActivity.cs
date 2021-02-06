using System;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public class LearningActivity
    {        
        [JsonProperty("description")]
        public string Description { get; set; }    
        [JsonProperty("activity")]
        public LabelledValue Activity { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("offerings")]
        public string Offerings { get; set; }
    }

    public class ScheduledLearningActivity : LearningActivity { }
    public class NonScheduledLearningActivity : LearningActivity { }
}