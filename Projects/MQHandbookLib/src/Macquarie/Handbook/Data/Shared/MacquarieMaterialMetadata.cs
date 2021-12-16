namespace Macquarie.Handbook.Data.Shared;

using System;
using System.Collections.Generic;
using Macquarie.Handbook.Converters;
using Newtonsoft.Json;

public record MacquarieMaterialMetadata : IdentifiableRecord
{

    [JsonProperty("implementationYear")]
    public ushort ImplementationYear { get; set; }
    [JsonProperty("status")]
    public LabelledValue Status { get; set; }
    [JsonProperty("academic_org")]
    public KeyValueIdType AcademicOrganisation { get; set; }
    [JsonProperty("school")]
    public KeyValueIdType School { get; set; }
    [JsonProperty("credit_points")]
    public UInt16 CreditPoints { get; set; }
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

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("class_name")]
    public string ClassName { get; set; }

    [JsonProperty("overview")]
    public string Overview { get; set; }

    [JsonProperty("academic_item_type")]
    public string AcademicItemType { get; set; }

    [JsonProperty("inherent_requirements")]
    public List<Requirement> InherentRequirements { get; set; }

    [JsonProperty("other_requirements")]
    public List<Requirement> OtherRequirements { get; set; }

    [JsonProperty("external_provider")]
    public string ExternalProvider { get; set; }

    [JsonProperty("links")]
    public List<string> Links { get; set; }

    [JsonProperty("published_in_handbook")]
    public LabelledValue PublishedInHandbook { get; set; }
}
