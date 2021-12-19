namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;

using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

public record CourseDto : MetadataDto
{
    public CourseDataDto? CourseData { get; init; }
    public CurriculumStructureDataDto? CurriculumData { get; init; }
    public string UrlYear { get; init; } = string.Empty;
    public string Generic { get; init; } = string.Empty;
    public string UrlMap { get; init; } = string.Empty;
}