using System;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class AcademicItem
    {
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItemInnerId { get; set; }
        [JsonProperty("parent_record")]
        public KeyValueIdType ParentRecord { get; set; }
        [JsonProperty("order")]
        public string Order { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("academic_item_type")]
        public LabelledValue AcademicItemType { get; set; }
        [JsonProperty("abbr_name")]
        public string AbbreviationName { get; set; }
        [JsonProperty("academic_item_version_name")]
        public string AcademicItemVersionName { get; set; }
        [JsonProperty("academic_item_credit_points")]
        public string AcademicItemCreditPoints { get; set; }
        [JsonProperty("abbreviated_name_and_major")]
        public string AbbreviatedNameAndMajor { get; set; }
        [JsonProperty("academic_item_name")]
        public string AcademicItemName { get; set; }
        [JsonProperty("academic_item_code")]
        public string AcademicItemCode { get; set; }
        [JsonProperty("academic_item_url")]
        public string AcademicItemUrl { get; set; }
    }
}