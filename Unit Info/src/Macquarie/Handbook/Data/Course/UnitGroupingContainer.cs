//#define IGNORE_UNNECESSARY

using System.Collections.Generic;
using Macquarie.Handbook.Data.Helpers;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Course
{
    public class UnitGroupingContainer
    {
        private string description;

        [JsonProperty("title")]
        public string Title { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("vertical_grouping")]
#endif
        public LabelledValue VerticalGrouping { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("horizontal_grouping")]
#endif
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
        [JsonProperty("description")]
        public string Description { get => description; set => description = HTMLTagStripper.StripHtmlTags(value); }
        [JsonProperty("credit_points_max")]
        public string CreditPointsMax { get; set; }
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
        [JsonProperty("parentConnector")]
        public LabelledValue ParentConnector { get; set; }
        [JsonProperty("dynamic_relationship")]
        public List<DynamicRelation> DynamicRelationship { get; set; }
        [JsonProperty("container")]
        public List<UnitGroupingContainer> Container { get; set; }
        [JsonProperty("relationship")]
        public List<AcademicItem> Relationships { get; set; }
    }
}