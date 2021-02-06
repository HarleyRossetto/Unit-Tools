using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public class container_requisit_temporaryname
    {
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("parent_container_value")]
        public string ParentContainerTable { get; set; }
        [JsonProperty("parent_record")]
        public KeyValueIdType ParentRecord { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("parent_connector")]
        public LabelledValue ParentConnector { get; set; }
        [JsonProperty("containers")]
        public List<container_requisit_temporaryname> Containers { get; set; }
        [JsonProperty("relationships")]
        public List<AcademicItem> Relationships { get; set; }
    }
}