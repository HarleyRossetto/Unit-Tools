namespace Macquarie.Handbook.Data;

using System;
using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Macquarie.Handbook.Data.Unit;
using Newtonsoft.Json;

public class MacquarieUnit : MacquarieMetadata
{
    [JsonProperty("data")]
    [JsonConverter(typeof(MacquarieEmbeddedJsonConverter<MacquarieUnitData>))]
    public MacquarieUnitData UnitData { get; set; }
    [JsonProperty("creditPoints")]
    public ushort CreditPoints { get; set; }
    [JsonProperty("description")]
    [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
    public string Description { get; set; }
    [JsonProperty("level")]
    public string Level { get; set; }
    [JsonProperty("publishedInHandbook")]
    public ushort PublishedInHandbook { get; set; }
    [JsonProperty("levelDisplay")]
    public string LevelDisplay { get; set; }
    [JsonProperty("effectiveDate", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? EffectiveDate { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
    public override string ToString() => $"{UnitData.Code} {ImplementationYear}";
    [JsonProperty("sysId")]
    public string SysId { get; set; }
    [JsonProperty("location")]
    public string Location { get; set; }
    [JsonProperty("version")]
    public string Version { get; set; }
    [JsonProperty("teachingPeriod")]
    public string TeachingPeriod { get; set; }
    [JsonProperty("active")]
    public string Active { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("baseType")]
    public string BaseType { get; set; }
    [JsonProperty("mode")]
    public string Mode { get; set; }
    [JsonProperty("academicOrg")]
    public string AcademicOrg { get; set; }
}
