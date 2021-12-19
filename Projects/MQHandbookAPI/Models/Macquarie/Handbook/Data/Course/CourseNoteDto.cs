using Macquarie.Handbook.Data.Shared;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;

public record CourseNoteDto
{
    public string Note { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string DisplayValue { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
}