using System.Collections.Generic;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class UnitGroupingContainer
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("vertical_grouping")]
        public LabelledValue VerticalGrouping { get; set; }
        [JsonProperty("horizontal_grouping")]
        public LabelledValue HorizontalGrouping { get; set; }
        [JsonProperty("preface")]
        public string Preface { get; set; }
        [JsonProperty("dynamic_query")]
        public string DynamicQuery { get; set; }
        [JsonProperty("child_table")]
        public string ChildTable { get; set; }
        [JsonProperty("child_record")]
        public KeyValueIdType ChildRecord { get; set; }
        [JsonProperty("footnote")]
        public string Footnote { get; set; }
        [JsonProperty("tidescriptionle")]
        public string Description { get; set; }
        [JsonProperty("credit_points_max")]
        public string CreditPointsMax { get; set; }
        [JsonProperty("parent_record")]
        public KeyValueIdType ParentRecord { get; set; }
        [JsonProperty("parent_table")]
        public string ParentTable { get; set; }
        [JsonProperty("parentConnector")]
        public LabelledValue ParentConnector { get; set; }
        [JsonProperty("dynamic_relationship")]
        public List<string> DynamicRelationship { get; set; }
        [JsonProperty("container")]
        public List<UnitGroupingContainer> Container { get; set; }
        [JsonProperty("relationship")]
        public List<AcademicItem> Relationships { get; set; }
    }
}