using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class MacquarieCurriculumStructure
    {
        [JsonProperty("curriculum_structure")]
        public KeyValueIdType CurriculumStructure { get; set; }
        [JsonProperty("ai_to_cs_cl_id")]
        public string AI_TO_CS_CL_ID { get; set; }
        [JsonProperty("pe_remote_id")]
        public string PeRemoteId { get; set; }
        [JsonProperty("cl_id")]
        public KeyValueIdType CL_ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("credit_points")]
        public string CreditPoints { get; set; }
        [JsonProperty("parent_id")]
        public KeyValueIdType ParentId { get; set; }
        [JsonProperty("effective_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("source")]
        public KeyValueIdType Source { get; set; }
        [JsonProperty("parent_table")]
        public string ParentTable { get; set; }
        [JsonProperty("container")]
        public List<UnitGroupingContainer> Container { get; set; }
    }
}