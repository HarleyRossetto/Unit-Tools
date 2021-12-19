namespace Macquarie.Handbook.Data.Unit.Prerequisites;

using System;
using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;


public record EnrolmentRule : IdentifiableRecord
{
    [JsonProperty("description")]
    [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
    public string Description { get; set; }
    [JsonProperty("type")]
    public LabelledValue Type { get; set; }
    [JsonProperty("order")]
    public UInt16 Order { get; init; }

    public override string ToString() {
        return Description;
    }
}
