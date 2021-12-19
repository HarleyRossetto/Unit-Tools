using Macquarie.Handbook.Data.Shared;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;

public record AdmissionRequirementPointDto
{
    public string AdmissionRequirement { get; init; } = string.Empty;
    public string VolumeOfLearning { get; init; } = string.Empty;
    public uint CreditPoints { get; init; } = 0;
    public List<string>? StructureZones { get; init; }
}
