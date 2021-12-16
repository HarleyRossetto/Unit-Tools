namespace Macquarie.Handbook.Data.Course;

using Macquarie.Handbook.Data.Shared;
using Newtonsoft.Json;


public record HigherLevelCoursesThatStudentsMayExitFrom : IdentifiableRecord
{
    [JsonProperty("code")]
    public KeyValueIdType Code { get; set; }
    [JsonProperty("status")]
    public string Status { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
}
