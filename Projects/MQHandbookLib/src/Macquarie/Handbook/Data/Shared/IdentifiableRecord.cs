namespace Macquarie.Handbook.Data.Shared;

using Newtonsoft.Json;

public record IdentifiableRecord
{

    [JsonProperty("cl_id")]
    public string CL_ID { get; init; }
}
