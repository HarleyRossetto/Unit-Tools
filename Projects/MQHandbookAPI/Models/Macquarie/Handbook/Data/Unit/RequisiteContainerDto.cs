using MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Unit;

public record RequisiteContainerDto
{
    public string Connector { get; init; } = string.Empty;
    public List<RequisiteContainerDto>? Containers { get; init; }
    public List<AcademicItemDto>? Relationships { get; init; }
}
