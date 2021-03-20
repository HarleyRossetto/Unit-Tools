using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public class Requisite
    {
        [JsonProperty("academic_item_code")]
        public string AcademicItemCode { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("requisite_cl_id")]
        public string RequisiteClId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("requisite_type")]
        public LabelledValue RequisiteType { get; set; }
        [JsonProperty("cl_id")]
        public KeyValueIdType CL_ID { get; set; }
        [JsonProperty("academic_item_version_number")]
        public string AcademicItemVersionNumber { get; set; }
        [JsonProperty("order")]
        public string order { get; set; }
        [JsonProperty("start_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }
        [JsonProperty("end_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }
        [JsonProperty("containers")]
        public List<ContainerRequisiteTemporaryName> Requisites { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}