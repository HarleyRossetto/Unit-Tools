//#define IGNORE_UNNECESSARY

using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Shared
{
    public record MacquarieMaterialMetadata : IdentifiableRecord
    {

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
        [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
        public string Description { get; set; }
        [JsonProperty("search_title")]
        public string SearchTitle { get; set; }
        [JsonProperty("code")]
        public string Code { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("content_type")]
        public string ContentType { get; set; }
        [JsonProperty("credit_points_header")]
        public string CreditPointsHeader { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("version")]
        public string Version { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("class_name")]
        public string ClassName { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("overview")]
        public string Overview { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("academic_item_type")]
        public string AcademicItemType { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("inherent_requirements")]
        public List<Requirement> InherentRequirements { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("other_requirements")]
        public List<Requirement> OtherRequirements { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("external_provider")]
        public string ExternalProvider { get; set; }
        
#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("links")]
        public List<string> Links { get; set; }

#if IGNORE_UNNECESSARY
        [JsonIgnore]
#endif
        [JsonProperty("published_in_handbook")]
        public LabelledValue PublishedInHandbook { get; set; }
    }
}