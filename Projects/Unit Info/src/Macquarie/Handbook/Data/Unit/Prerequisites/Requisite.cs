//#define IGNORE_UNNECESSARY

using System;
using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public record Requisite
    {
        [JsonProperty("academic_item_code")]
        public string AcademicItemCode { get; init; }
        [JsonProperty("active")]
        public bool Active { get; init; }
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; init; }
        [JsonProperty("requisite_type")]
        public LabelledValue RequisiteType { get; init; }
        [JsonProperty("academic_item_version_number")]
        public string AcademicItemVersionNumber { get; init; }
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; init; }
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; init; }
        [JsonProperty("containers")]
        public List<ContainerRequisiteTemporaryName> Requisites { get; init; }


#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("order")]
        public string Order { get; init; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("cl_id")]
        public KeyValueIdType CL_ID { get; init; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("requisite_cl_id")]
        public string RequisiteClId { get; init; }

        public override string ToString() {
            return Description;
        }
    }
}