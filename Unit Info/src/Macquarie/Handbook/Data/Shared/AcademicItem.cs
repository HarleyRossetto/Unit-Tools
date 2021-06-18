//#define IGNORE_UNNECESSARY

using System;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public record AcademicItem : IdentifiableRecord
    {
        [JsonProperty("academic_item")]
        public KeyValueIdType AcademicItemInnerId { get; init; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("parent_record")]
#endif
        public KeyValueIdType ParentRecord { get; init; }
      #if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("order")]
#endif
        public string Order { get; init; }
        [JsonProperty("academic_item_type")]
        public LabelledValue AcademicItemType { get; init; }
        [JsonProperty("abbr_name")]
        public string AbbreviationName { get; init; }
        [JsonProperty("academic_item_version_name")]
        public string AcademicItemVersionName { get; init; }
        [JsonProperty("academic_item_credit_points")]
        public string AcademicItemCreditPoints { get; init; }
        [JsonProperty("abbreviated_name_and_major")]
        public string AbbreviatedNameAndMajor { get; init; }
        [JsonProperty("academic_item_name")]
        public string AcademicItemName { get; init; }
        [JsonProperty("academic_item_code")]
        public string AcademicItemCode { get; init; }
        [JsonProperty("academic_item_url")]
        public string AcademicItemUrl { get; init; }
    }
}