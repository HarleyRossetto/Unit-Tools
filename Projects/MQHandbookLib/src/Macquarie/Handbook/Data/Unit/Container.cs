//#define IGNORE_UNNECESSARY

using System;
using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit
{
    public record ContainerRequisiteTemporaryName : IdentifiableRecord
    {
        [JsonProperty("parent_container_value")]
        public string ParentContainerTable { get; init; }
        [JsonProperty("parent_record")]
        public KeyValueIdType ParentRecord { get; init; }
        [JsonProperty("title")]
        public string Title { get; init; }
        [JsonProperty("parent_connector")]
        public LabelledValue ParentConnector { get; init; }
        [JsonProperty("containers")]
        public List<ContainerRequisiteTemporaryName> Containers { get; init; }
        [JsonProperty("relationships")]
        public List<AcademicItem> Relationships { get; init; }
    }
}