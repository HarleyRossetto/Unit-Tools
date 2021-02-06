using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public class MacquarieMaterialMetadata
    {
        [JsonProperty("published_in_handbook")]
        public LabelledValue published_in_handbook { get; set; }
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
        public string Description { get; set; }
        [JsonProperty("search_title")]
        public string SearchTitle { get; set; }
        [JsonProperty("cl_id")]
        public string CL_ID { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("version")]
        public string Version { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        [JsonProperty("class_name")]
        public string ClassName { get; set; }
        [JsonProperty("overview")]
        public string Overview { get; set; }
        [JsonProperty("credit_points_header")]
        public string CreditPointsHeader { get; set; }
        [JsonProperty("academic_item_type")]
        public string AcademicItemType { get; set; }
        [JsonProperty("inherent_requirements")]
        public List<string> InherentRequirements { get; set; }
        [JsonProperty("other_requirements")]
        public List<string> OtherRequirements { get; set; }
        [JsonProperty("external_provider")]
        public string ExternalProvider { get; set; }
        [JsonProperty("links")]
        public List<string> Links { get; set; }
    }
}