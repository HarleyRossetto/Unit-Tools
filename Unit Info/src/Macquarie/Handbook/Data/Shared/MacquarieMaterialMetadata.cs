//#define IGNORE_UNNECESSARY

using System.Collections.Generic;
using Macquarie.Handbook.Data.Helpers;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieMaterialMetadata
    {
        private string description;
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("published_in_handbook")]
#endif
        public LabelledValue PublishedInHandbook { get; set; }
        [JsonProperty("implementationYear")]
        public string ImplementationYear { get; set; }
        [JsonProperty("status")]
        public LabelledValue Status { get; set; }
        [JsonProperty("academic_org")]
        public KeyValueIdType AcademicOrganisation { get; set; }
        [JsonProperty("school")]
        public KeyValueIdType School { get; set; }
        [JsonProperty("credit_points")]
        public string CreditPoints { get; set; }
        [JsonProperty("type")]
        public LabelledValue Type { get; set; }
        [JsonProperty("description")]
        public string Description { get => description; set => description = HTMLTagStripper.StripHtmlTags(value); }
        [JsonProperty("search_title")]
        public string SearchTitle { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("cl_id")]
#endif
        public string CL_ID { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("version")]
#endif
        public string Version { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("class_name")]
#endif
        public string ClassName { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("overview")]
#endif 
        public string Overview { get; set; }
        [JsonProperty("credit_points_header")]
        public string CreditPointsHeader { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("academic_item_type")]
#endif  
        public string AcademicItemType { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("inherent_requirements")]
#endif  
        public List<Requirement> InherentRequirements { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("other_requirements")]
#endif  
        public List<Requirement> OtherRequirements { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("external_provider")]
#endif  
        public string ExternalProvider { get; set; }
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#else
        [JsonProperty("links")]
#endif  
        public List<string> Links { get; set; }
    }
}