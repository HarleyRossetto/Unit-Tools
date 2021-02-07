using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class AdmissionRequirementPoint
    {
        [JsonProperty("admission_requirement")]
        public string AdmissionRequirement { get; set; }
        [JsonProperty("volume_of_learning")]
        public LabelledValue VolumeOfLearning { get; set; }
        [JsonProperty("credit_points")]
        public uint CreditPoints { get; set; }
        [JsonProperty("structure_zones")]
        public List<KeyValueIdType> StructureZones { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
    }
}