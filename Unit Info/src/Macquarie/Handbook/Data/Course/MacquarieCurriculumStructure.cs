//#define IGNORE_UNNECESSARY

using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class MacquarieCurriculumStructureData
    {
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("curriculum_structure")]
#endif
        public KeyValueIdType CurriculumStructure { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("ai_to_cs_cl_id")]
#endif
        public string AI_TO_CS_CL_ID { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("pe_remote_id")]
#endif
        public string PeRemoteId { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public KeyValueIdType CL_ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("credit_points")]
        public string CreditPoints { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_id")]
#endif
        public KeyValueIdType ParentId { get; set; }
        [JsonProperty("effective_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("source")]
#endif
        public KeyValueIdType Source { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_table")]
#endif
        public string ParentTable { get; set; }
        [JsonProperty("container")]
        public List<UnitGroupingContainer> Container { get; set; }
    }
}