using Macquarie.Handbook.Data.Shared;

namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Course;

public record FeeDto
{
    public string FeePerCreditPoint { get; init; } = string.Empty; 
    public string FeeNote { get; init; } = string.Empty;
    public string OtherFeeType { get; init; } = string.Empty;
    public List<KeyValueIdType>? Intakes { get; init; }
    public bool AppliesToAllIntakes { get; init; }
    public string EstimatedAnnualFee { get; init; } = string.Empty;
    public string FeeType { get; init; } = string.Empty;
}
