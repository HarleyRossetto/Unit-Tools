//#define IGNORE_UNNECESSARY

using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public record MacquarieCurriculumStructureData //: IdentifiableRecord
    {
        [JsonProperty("credit_points")]
        public string CreditPoints { get; set; }
        [JsonProperty("effective_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EffectiveDate { get; set; }
        [JsonProperty("container")]
        public List<UnitGroupingContainer> Container { get; set; }


#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("ai_to_cs_cl_id")]
        public string AI_TO_CS_CL_ID { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("cl_id")]
        public KeyValueIdType CL_ID { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("curriculum_structure")]
        public KeyValueIdType CurriculumStructure { get; set; }       

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("name")]
        public string Name { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("parent_id")]
        public KeyValueIdType ParentId { get; set; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("parent_table")]
        public string ParentTable { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("pe_remote_id")]
        public string PeRemoteId { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("source")]
        public KeyValueIdType Source { get; set; }
    }
}