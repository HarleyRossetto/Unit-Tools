//#define IGNORE_UNNECESSARY

using System.Collections.Generic;

using Newtonsoft.Json;

using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Helpers;

namespace Macquarie.Handbook.Data.Course
{
    public class DynamicRelation
    {
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_record")]
#endif
        public KeyValueIdType ParentRecord { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_table")]
#endif
        public string ParentTable { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
        [JsonProperty("description")]
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("rule")]
        [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<DynamicRelationRule>))]
        public DynamicRelationRule Rule { get; set; }
        [JsonProperty("wildcard")]
        public string Wildcard { get; set; }
        [JsonProperty("encodedURL")]
        public string EncodedUrl { get; set; }
    }

    public class DynamicRelationRule
    {
        [JsonProperty("operator_groups")]
        public List<OperatorGroup> OperatorGroups { get; set; }
        [JsonProperty("operator_group_members")]
        public List<OperatorGroupMember> OperatorGroupMembers { get; set; }
    }

    public class OperatorGroup
    {
        [JsonProperty("group_connector")]
        public string GroupConnector { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public string Order { get; set; }
        [JsonProperty("parent_record")]
        public string ParentRecord { get; set; }
    }

    public class OperatorGroupMember
    {
        [JsonProperty("parent_record")]
        public string ParentRecord { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public string Order { get; set; }
        [JsonProperty("id")]
        public string ID { get; set; }
        [JsonProperty("map")]
        public OperatorGroupMemberMap Map { get; set; }
    }

    public class OperatorGroupMemberMap
    {
        [JsonProperty("field")]
        public string Field { get; set; }
        [JsonProperty("operator_value")]
        public string OperatorValue { get; set; }
        [JsonProperty("input_value")]
        public string InputValue { get; set; }
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}