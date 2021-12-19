using Macquarie.Handbook.Converters;
using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;

namespace Macquarie.Handbook.Data.Unit;

public record LearningActivity : IdentifiableRecord
{
    [JsonProperty("description")]
    [JsonConverter(typeof(MacquarieHtmlStripperConverter))]
    public string Description { get; init; }
    [JsonProperty("activity")]
    public LabelledValue Activity { get; init; }
    [JsonProperty("offerings")]
    public string Offerings { get; init; }

    public override string ToString() {
        return Description;
    }
}

public record ScheduledLearningActivity : LearningActivity { }
public record NonScheduledLearningActivity : LearningActivity { }
