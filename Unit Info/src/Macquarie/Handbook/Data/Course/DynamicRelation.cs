using System.Collections.Generic;

using Newtonsoft.Json;

using Macquarie.Handbook;
using Macquarie.Handbook.Data.Shared;

namespace Macquarie.Handbook.Data.Course
{
    public class DynamicRelation
    {
        [JsonProperty("parent_record")]
        public KeyValueIdType ParentRecord { get; set; }
        [JsonProperty("parent_table")]
        public string ParentTable { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        private string _InnerRuleJson;
        [JsonProperty("rule")]
        public string InnerRuleJson {
            get {
                return _InnerRuleJson;
            }
            set {
                _InnerRuleJson = value;
                Rule = MacquarieHandbook.DeserialiseJsonObject<DynamicRelationRule>(value);
            }
        }
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
        [JsonProperty("order")]
        public string Order { get; set; }
        [JsonProperty("parent_record")]
        public string ParentRecord { get; set; }
    }

    public class OperatorGroupMember
    {
        [JsonProperty("parent_record")]
        public string ParentRecord { get; set; }
        [JsonProperty("order")]
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