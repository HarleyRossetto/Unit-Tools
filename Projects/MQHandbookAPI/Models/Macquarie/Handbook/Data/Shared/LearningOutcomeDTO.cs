namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

public class LearningOutcomeDto
{
    public string Description { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;

    //public string Order { get; init; }

    public override string ToString() {
        return Description;
    }
}
